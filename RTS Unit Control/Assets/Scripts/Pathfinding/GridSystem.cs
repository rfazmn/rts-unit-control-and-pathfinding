using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    Node[,] grid;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] float nodeRadius = .5f;
    [SerializeField] LayerMask obstacleLayer;

    float nodeSize;
    public bool drawGrid;

    void Start()
    {
        nodeSize = nodeRadius * 2f;
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            Vector3 offset = new Vector3(gridSize.x / 2f, 0f, gridSize.y / 2f) - new Vector3(nodeRadius, 0f, nodeRadius);
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 gridPos = new Vector3(x, 0f, y);
                Vector3 worldPos = gridPos - offset;
                bool isObstacle = Physics.CheckBox(worldPos, Vector3.one * nodeRadius, Quaternion.identity, obstacleLayer);
                Node node = new Node(new Vector2Int(x, y), worldPos, isObstacle);
                grid[x, y] = node;
            }
        }
    }

    public Node GetNodeWithPosition(Vector3 position)
    {
        Vector3 offset = new Vector3(gridSize.x / 2f, 0f, gridSize.y / 2f) - new Vector3(nodeRadius, 0f, nodeRadius);
        int x = Mathf.RoundToInt(position.x + offset.x);
        int y = Mathf.RoundToInt(position.z + offset.z);

        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y)
            return null;

        return grid[x, y];
    }

    public List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Vector2Int gridPos = node.gridPosition;
        for (int x = Mathf.Max(0, gridPos.x - 1); x <= Mathf.Min(gridPos.x + 1, gridSize.x - 1); x++)
        {
            for (int y = Mathf.Max(0, gridPos.y - 1); y <= Mathf.Min(gridPos.y + 1, gridSize.y - 1); y++)
            {
                if (grid[x, y] == node)
                    continue;

                neighbours.Add(grid[x, y]);
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (!drawGrid || grid == null)
            return;

        foreach (var node in grid)
        {
            Gizmos.color = node.obstacle ? Color.red : Color.white;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeSize - Vector3.one * .1f);
        }
    }
}
