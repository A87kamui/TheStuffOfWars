using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;   // Store x and y size of play area

    [Tooltip("Unity Grid Size = Should match UnityEditor snap settings")]
    [SerializeField] int unityGridSize = 10;    // 10 to match Snap setting 
    public static GridManager instance;

    /// <summary>
    /// Property to get unityGridSize
    /// </summary>
    public int UnityGridSize { get { return unityGridSize; } }

    // Dictionary using (key, value)
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    /// <summary>
    /// Property to return grid
    /// </summary>
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    /// <summary>
    /// Populate grid with all nodes in game
    /// </summary>
    private void Awake()
    {
        CreateGrid();
        instance = this;
    }

    /// <summary>
    /// Check if node is in the grid
    /// Set that node as blocked, not walkable
    /// </summary>
    public void BlockNode(Vector2Int coordinate)
    {
        if (grid.ContainsKey(coordinate))
        {
            grid[coordinate].isWalkable = false;
        }
    }

    /// <summary>
    /// Loop through every node in world grid
    /// Reset the node to its initial state of unexplored/not connected to another node
    /// </summary>
    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.parent = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
            entry.Value.ResetValues();
        }
    }

    /// <summary>
    /// Get the coordinates from a position in the world grid
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();  // Create a vector2Int variable

        // Gets the x and y coordinate based on parent position location
        // Since x and z are in factors of 10, need to divide them by snap size (10)
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);
        return coordinates;
    }

    /// <summary>
    /// Get the world grid position from coordinates
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();  // Create a Vector3 variable

        // Gets the x and z position based on coordinate location
        // Since x and z are in factors of 10, need to multiy coordinates x/y by snap size (10)
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;
        return position;
    }

    /// <summary>
    /// Retruns the node from passed in coordinates if it exist
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }

        return null;
    }

    /// <summary>
    /// Create nodes and place into grid dictionary
    /// Created nodes contain the x, y value coordinate location on the grid
    /// </summary>
    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}