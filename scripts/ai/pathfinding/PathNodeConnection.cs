using System.Diagnostics;
using Godot;

[Tool]
public partial class PathNodeConnection : Node3D
{
	private PathNode _node1;
	private PathNode _node2;

    public override void _Process(double delta)
    {
		if (Engine.IsEditorHint())
		{
			UpdateConnection();
		}
    }

	public void SetPathNodes(PathNode node1, PathNode node2)
	{
		_node1 = node1;
		_node2 = node2;
	}

	public bool ConnectingNode(PathNode node)
	{
		return _node1 == node || _node2 == node;
	}

	private void UpdateConnection()
	{
		if(_node1 == null || _node2 == null)
		{
			QueueFree();
			return;
		}

		if(!_node1.IsInsideTree() || !_node2.IsInsideTree())
		{
			_node1 = null;
			_node2 = null;
			return;
		}

		if(_node1.Position == _node2.Position)
		{
			return;
		}

		Position = (_node1.Position + _node2.Position) / 2.0f;
		LookAt(_node2.Position);
		Scale = new Vector3(1, 1, _node1.Position.DistanceTo(_node2.Position));
	}
}
