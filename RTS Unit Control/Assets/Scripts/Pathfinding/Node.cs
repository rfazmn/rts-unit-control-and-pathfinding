using UnityEngine;

public class Node
{
    public Vector2Int gridPosition;
    public Vector3 worldPosition;
    public bool obstacle;

    public int gCost;
    public int hCost;

    public Node parent;

    public Node(Vector2Int _gridPosition, Vector3 _worldPosition, bool _obstacle)
    {
        gridPosition = _gridPosition;
        worldPosition = _worldPosition;
        obstacle = _obstacle;
    }

    public int GetFCost()
    {
        return gCost + hCost;
    }
}
