using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Tool]
public partial class PathNode : Node3D
{
	[Export] private PathNode[] _connectedPathNodeArray;

	private List<PathNode> _connectedPathNodes = new List<PathNode>();
	private List<PathNodeConnection> _connections = new List<PathNodeConnection>();

	private Vector3 _lastPosition;

	public List<PathNode> ConnectedPathNodes { get => _connectedPathNodeArray.ToList(); }

    public override void _Ready()
    {
		if (Engine.IsEditorHint())
		{
			try
			{
				_lastPosition = GlobalPosition;
				NavigationGraph.Instance.AddNodeToPath(this);
			}
			catch (NullReferenceException)
			{
				if (IsInsideTree())
				{
					Debug.Print("Navigation graph not found!");
					QueueFree();
				}
			}
		}
    }

    public override void _Process(double delta)
    {
		if (Engine.IsEditorHint())
		{
			if(NavigationGraph.Instance == null)
			{
				return;
			}

			if(_lastPosition != GlobalPosition)
			{
				_lastPosition = GlobalPosition;
				NavigationGraph.Instance.CalculateNodeConnections(this);
			}
		}
    }

	public void AddConnection(PathNode node, PathNodeConnection connection)
	{
		_connectedPathNodes.Add(node);
		_connections.Add(connection);

		_connectedPathNodeArray = _connectedPathNodes.ToArray();
	}

	public void RemoveConnection(PathNode node)
	{
		_connectedPathNodes.Remove(node);

		foreach(PathNodeConnection connection in _connections)
		{
			if (connection.ConnectingNode(node))
			{
				connection.SetPathNodes(null, null);
				_connections.Remove(connection);
				connection.QueueFree();
				return;
			}
		}
	}

	public bool PathConnectionExists(PathNode otherNode)
	{
		foreach(PathNodeConnection connection in _connections)
		{
			if (connection.ConnectingNode(otherNode))
			{
				return true;
			}
		}

		return false;
	}

	public bool ContainsConnection(PathNode otherNode)
	{
		return _connectedPathNodes.Contains(otherNode);
	}

	public void ResetConnections()
	{
		_connectedPathNodes.Clear();
		_connections.Clear();
	}
}
