using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : Singleton<AStarPathfinding>
{
    [SerializeField] GridSystem grid;

    public List<Vector3> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeWithPosition(startPosition);
        Node targetNode = grid.GetNodeWithPosition(targetPosition);

        if (startNode == null || targetNode == null || targetNode.obstacle)
            return null;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);
        while (openList.Count > 0)
        {
            Node current = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].GetFCost() < current.GetFCost())
                    current = openList[i];
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == targetNode)
                break;

            List<Node> neighbourNodes = grid.GetNodeNeighbours(current);
            foreach (Node neighbour in neighbourNodes)
            {
                if (neighbour.obstacle || closedList.Contains(neighbour))
                    continue;

                int newGCost = current.gCost + GetHeuristicDistance(current.gridPosition, neighbour.gridPosition);
                bool contains = openList.Contains(neighbour);
                if (neighbour.gCost <= newGCost && contains)
                    continue;

                neighbour.gCost = newGCost;
                neighbour.hCost = GetHeuristicDistance(neighbour.gridPosition, targetNode.gridPosition);
                neighbour.parent = current;

                if (!contains)
                    openList.Add(neighbour);
            }
        }

        return TracePath(startNode, targetNode);
    }

    List<Vector3> TracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        while (start != end)
        {
            path.Add(end);
            end = end.parent;
        }

        path.Reverse();
        return OptimizePath(path);
    }

    List<Vector3> OptimizePath(List<Node> path)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector2 oldDir = Vector2.zero;
        int pathCount = path.Count;


        for (int i = 1; i < pathCount; i++)
        {
            Node current = path[i - 1];
            Node next = path[i];

            Vector2 currentDir = next.gridPosition - current.gridPosition;
            if (oldDir == currentDir)
            {
                oldDir = currentDir;
                continue;
            }

            oldDir = currentDir;
            positions.Add(current.worldPosition);
        }

        positions.Add(path[pathCount - 1].worldPosition);
        return positions;
    }

    //Heuristic diagonal distance
    //D * (dx + dy) + (D2 - 2 * D) * min(dx, dy)   
    public int GetHeuristicDistance(Vector2Int current, Vector2Int target)        //    |\              d=1 -> d2 = sqrt(2) = 1.41... multiply by 10 to get an int value = 14~
    {                                                                             //    | \                           d=10, d2=14
        int dX = Mathf.Abs(current.x - target.x);                                 //    |  \d*sqrt(2)
        int dY = Mathf.Abs(current.y - target.y);                                 //  d |   \
                                                                                  //    |____\
        return 10 * (dX + dY) + -6 * Mathf.Min(dX, dY);                           //       d
    }
}
