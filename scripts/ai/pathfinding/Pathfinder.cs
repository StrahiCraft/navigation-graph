using System.Collections.Generic;

public static class Pathfinder
{
	public static List<PathNode> CalculatePathDFS(PathNode startingNode, PathNode goalNode)
	{
		Stack<(PathNode, List<PathNode>)> toExplore = new Stack<(PathNode, List<PathNode>)>();
		toExplore.Push((startingNode, new List<PathNode>() {startingNode}));
		List<PathNode> explored = new List<PathNode>();

		while(toExplore.Count > 0)
		{
			var (currentNode, path) = toExplore.Pop();
			if(currentNode == goalNode)
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

	public static List<PathNode> CalculatePathBFS(PathNode startingNode, PathNode goalNode)
	{
		Queue<(PathNode, List<PathNode>)> toExplore = new Queue<(PathNode, List<PathNode>)>();
		toExplore.Enqueue((startingNode, new List<PathNode>() {startingNode}));
		List<PathNode> explored = new List<PathNode>();

		while(toExplore.Count > 0)
		{
			var (currentNode, path) = toExplore.Dequeue();
			if(currentNode == goalNode)
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
