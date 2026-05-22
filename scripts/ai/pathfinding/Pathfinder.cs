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

	public static List<PathNode> CalculatePathDijkstra(PathNode startingNode, PathNode goalNode)
	{
		PriorityQueue<KeyValuePair<PathNode, List<PathNode>>, float> toExplore = new PriorityQueue<KeyValuePair<PathNode, List<PathNode>>, float>();
		toExplore.Enqueue(new KeyValuePair<PathNode, List<PathNode>> (startingNode, new List<PathNode>() {startingNode}), 0f);
		List<PathNode> explored = new List<PathNode>();

		while(toExplore.Count > 0)
		{
			toExplore.TryDequeue(out KeyValuePair<PathNode, List<PathNode>> currentNode, out float distanceFromStart);
			if(currentNode.Key == goalNode)
			{
				return currentNode.Value;
			}
			explored.Add(currentNode.Key);

			foreach(PathNode node in currentNode.Key.ConnectedPathNodes)
			{
				if (!explored.Contains(node))
				{
					toExplore.Enqueue(new KeyValuePair<PathNode, List<PathNode>> (node, new List<PathNode>(currentNode.Value) {node}),
						currentNode.Key.Position.DistanceTo(node.Position) + distanceFromStart);
				}
			}
		}

		return null;
	}
}
