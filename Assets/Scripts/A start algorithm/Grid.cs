using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    Node usedPlayerNode;

    public Transform player;

    public bool displayGridGizmo;

    public LayerMask doorMask;
    public LayerMask unwalkableMask;
    public LayerMask playerMask;

    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public List<Node> path;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void Update()
    {
        if (grid != null)
        {
            //path = transform.GetComponent<Path_Finding>().returnPath();
            Node playerNode = NodeFromWorldPoint(player.position);
            updatePlayerNode(playerNode);
            /*foreach (Node n in grid)
            {
                if (playerNode == n)
                {

                }
            }*/
            //print("gridX: " + playerNode.gridX + ",gridY: " + playerNode.gridY);
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool _walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                bool _door = (Physics.CheckSphere(worldPoint, nodeRadius, doorMask));
                //bool _player = (Physics.CheckSphere(worldPoint, nodeRadius, playerMask));
                grid[x, y] = new Node(_door, _walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
        float percentY = (worldPosition.z - transform.position.z) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null && displayGridGizmo)
        {
            //path = transform.GetComponent<Path_Finding>().returnPath();
            Node playerNode = NodeFromWorldPoint(player.position);
            //updatePlayerNode(playerNode);
            foreach (Node n in grid)
            {


                //Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (n.walkable)
                    Gizmos.color = Color.white;
                if (n.door)
                    Gizmos.color = Color.blue;
                if (playerNode == n)
                    Gizmos.color = Color.cyan;

                if (path != null)
                {
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                }

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
               
            }
        }
    }

    public void updatePlayerNode(Node other)
    {
        usedPlayerNode = other;
    }

    public int getPlayerNodeX()
    {
        return usedPlayerNode.gridX;
    }

    public int getPlayerNodeY()
    {
        return usedPlayerNode.gridY;
    }

    public float getGridX()
    {
        return gridSizeX;
    }
    public float getGridY()
    {
        return gridSizeY;
    }
}
