using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Diagnostics;

[Tool]
public partial class NavigationGraph : Node3D
{
	[ExportCategory("Editor tools")]
	[Export] private bool Refresh
	{
		get => _refresh;
		set
		{
			RefreshGraph();
			_refresh = false;
		}
	}

	[Export(PropertyHint.Enum)] PathCalculationType _pathCalculationType;

	[Export] private bool CalculatePath
	{
		get => _calculatePath;
		set
		{
			if(_startingNode != null && _endingNode != null)
			{
				RefreshGraph();
				switch (_pathCalculationType)
				{
					case PathCalculationType.DFS:
						RenderPath(CalculatePathDFS());
						break;
					case PathCalculationType.BFS:
						RenderPath(CalculatePathBFS());
						break;
				}
			}
		}
	}

	[Export] private PathNode _startingNode;
	[Export] private PathNode _endingNode;

	[ExportCategory("Path connections")]
	[Export] private Node _pathNodeConnectionHolder;
	[Export] private PackedScene _pathNodeConnectionScene;
	[Export] private PackedScene _pathConnectionScene;

	[ExportCategory("Graph settings")]
	[Export] private float _maxDistanceBetweenPathNodes = 5;

	[Export(PropertyHint.Layers3DPhysics)] private uint _mapMask = 1;

	public static NavigationGraph Instance;

	private bool _refresh = false;
	private bool _calculatePath = false;

	private int _lastNodeCount;
	private List<PathNode> _pathNodes = new List<PathNode>();

	public override void _EnterTree()
	{
		if (Engine.IsEditorHint())
		{
			if(Instance == null)
			{
				Instance = this;
				return;
			}
			if (IsInsideTree())
			{
				QueueFree();
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			if(_lastNodeCount != GetChildCount())
			{
				_lastNodeCount = GetChildCount();
				RefreshGraph();
			}
		}
	}

	public override void _ExitTree()
	{
		if (Engine.IsEditorHint())
		{
			if(Instance == this)
			{
				Instance = null;
			}
		}
	}

	public void AddNodeToPath(PathNode node)
	{
		node.Reparent(this);
		_pathNodes.Add(node);
	}

	public void RefreshGraph()
	{
		Instance = this;
		GlobalPosition = Vector3.Zero;

		_maxDistanceBetweenPathNodes = 5;
		_pathNodeConnectionHolder = _pathNodeConnectionHolder = GetChild(0);
		_pathNodeConnectionScene = ResourceLoader.Load<PackedScene>("res://scenes/objects/pathfinding/path_node_connection.tscn");
		_mapMask = 1;
		_pathNodes.Clear();

		foreach(Node node in GetChildren())
		{
			if(node is PathNode pathNode)
			{
				_pathNodes.Add(pathNode);
			}
		}
		CalculateNavigationGraph();
	}

	public void CalculateNavigationGraph()
	{
		foreach(PathNode node in _pathNodes)
		{
			node.ResetConnections();
		}

		foreach(Node connection in _pathNodeConnectionHolder.GetChildren())
		{
			connection.QueueFree();
		}

		foreach(PathNode node in _pathNodes)
		{
			CalculateNodeConnections(node);
		}
	}

	public void CalculateNodeConnections(PathNode node)
	{
		foreach(PathNode connectingNode in _pathNodes)
		{
			if(node == connectingNode)
			{
				continue;
			}

			if(NodesCanConnect(node, connectingNode))
			{
				if (node.PathConnectionExists(connectingNode))
				{
					continue;
				}

				PathNodeConnection connection = (PathNodeConnection)_pathNodeConnectionScene.Instantiate();
				connection.SetPathNodes(node, connectingNode);

				node.AddConnection(connectingNode, connection);
				connectingNode.AddConnection(node, connection);

				_pathNodeConnectionHolder.AddChild(connection);
				continue;
			}

			if (node.PathConnectionExists(connectingNode))
			{
				node.RemoveConnection(connectingNode);
				connectingNode.RemoveConnection(node);
			}
		}
	}

	private bool NodesCanConnect(PathNode node1, PathNode node2)
	{
		if(node1.GlobalPosition.DistanceTo(node2.GlobalPosition) > _maxDistanceBetweenPathNodes)
		{
			return false;
		}

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(node1.GlobalPosition, node2.GlobalPosition,
			_mapMask);
		Dictionary result = spaceState.IntersectRay(query);

		return result.Count == 0;
	}

	private void RenderPath(List<PathNode> path)
	{
		if(path == null)
		{
			return;
		}

		for(int i = 0; i < path.Count - 1; i++)
		{
			PathNodeConnection connection = (PathNodeConnection)_pathConnectionScene.Instantiate();
			connection.SetPathNodes(path[i], path[i + 1]);

			_pathNodeConnectionHolder.AddChild(connection);
		}
	}

	private List<PathNode> CalculatePathDFS()
	{
		Stack<(PathNode, List<PathNode>)> toExplore = new Stack<(PathNode, List<PathNode>)>();
		toExplore.Push((_startingNode, new List<PathNode>() {_startingNode}));
		List<PathNode> explored = new List<PathNode>();

		while(toExplore.Count > 0)
		{
			var (currentNode, path) = toExplore.Pop();
			if(currentNode == _endingNode)
			{
				return path;
			}
			explored.Add(currentNode);

			foreach(PathNode node in currentNode.ConnectedPathNodes)
			{
				if (!explored.Contains(node))
				{
					toExplore.Push((node, new List<PathNode>(path) {node}));
				}
			}
		}

		return null;
	}

	private List<PathNode> CalculatePathBFS()
	{
		Queue<(PathNode, List<PathNode>)> toExplore = new Queue<(PathNode, List<PathNode>)>();
		toExplore.Enqueue((_startingNode, new List<PathNode>() {_startingNode}));
		List<PathNode> explored = new List<PathNode>();

		while(toExplore.Count > 0)
		{
			var (currentNode, path) = toExplore.Dequeue();
			if(currentNode == _endingNode)
			{
				return path;
			}
			explored.Add(currentNode);

			foreach(PathNode node in currentNode.ConnectedPathNodes)
			{
				if (!explored.Contains(node))
				{
					toExplore.Enqueue((node, new List<PathNode>(path) {node}));
				}
			}
		}
		return null;
	}
}
