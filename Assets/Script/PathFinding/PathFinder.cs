using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    // Property to get startCoordinates
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    // Property to get destinationCoordinates
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    [SerializeField] private PlayerMover player;

    // Store nodes in order of f value
    PriorityQueue<Node> openQueue = new PriorityQueue<Node>();

    // Use to check if a node is already explored or not (closed)
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    // Vector array for search directions
    Vector2Int[] orthogonalDirections = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    Vector2Int[] diagonallDirections = { (Vector2Int.up + Vector2Int.left), Vector2Int.one, (Vector2Int.down + Vector2Int.left), (Vector2Int.down + Vector2Int.right) };

    // Vector array for search directions
    Vector2Int[] directions = { (Vector2Int.up + Vector2Int.left), Vector2Int.up, Vector2Int.one,
                                Vector2Int.left, Vector2Int.right,
                                (Vector2Int.down + Vector2Int.left), Vector2Int.down, (Vector2Int.down + Vector2Int.right) };

    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    /// <summary>
    /// Initialize start and destination nodes 
    /// Check if there is a gridManager
    /// </summary>
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        startCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            // Create the starting node
            startNode = grid[startCoordinates];
            // Create the destination node
            //destinationNode = grid[destinationCoordinates];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //GetNewPath();   // Find path
    }

    /// <summary>
    /// Finds new path by reseting all nodes
    /// And passing in destination coordinates
    /// </summary>
    /// <returns></returns>
    public void GetNewPath(Vector3 coordinates)
    {
        startCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        destinationCoordinates = gridManager.GetCoordinatesFromPosition(coordinates);
        gridManager.ResetNodes();   // Reset all nodes to initial state
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            // Create the starting node
            startNode = grid[startCoordinates];
            // Create the destination node
            destinationNode = grid[destinationCoordinates];
        }
        AStarSearch(startCoordinates);
        player.MovePath(BuildPath());    // Build the path
    }

    /// <summary>
    /// Only get the path
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public List<Node> ReturnNewPath(Vector3 coordinates)
    {
        startCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        destinationCoordinates = gridManager.GetCoordinatesFromPosition(coordinates);
        gridManager.ResetNodes();   // Reset all nodes to initial state
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            // Create the starting node
            startNode = grid[startCoordinates];
            // Create the destination node
            destinationNode = grid[destinationCoordinates];
        }
        AStarSearch(startCoordinates);
        return BuildPath();
    }

    /// <summary>
    /// Finds new path by reseting all nodes
    /// And passing in destination coordinates
    /// </summary>
    /// <returns></returns>
    public List<Node> GetEnemyNewPath(Vector3 coordinates)
    {
        startCoordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        destinationCoordinates = gridManager.GetCoordinatesFromPosition(coordinates);
        gridManager.ResetNodes();   // Reset all nodes to initial state
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            // Create the starting node
            startNode = grid[startCoordinates];
            // Create the destination node
            destinationNode = grid[destinationCoordinates];
        }

        AStarSearch(startCoordinates);
        return BuildPath();    // Build the path
    }

    /// <summary>
    /// Search with AStarSearch for neighbor nodes until destination is found
    /// </summary>
    void AStarSearch(Vector2Int coordinates)
    {
        //startNode.isWalkable = true;
        //destinationNode.isWalkable = true;

        //nodeOpenList.Clear();   // Clears open list
        openQueue.ClearQueue();
        reached.Clear();    // Clears close

        bool isRunning = true;

        // Set f, g, h values of node
        grid[coordinates] = SetNodeValues(grid[coordinates]);

        // Add Node to open queue
        openQueue.Enqueue(grid[coordinates]);

        // Add the coordiantes passed in and the node at coordinates passed in 
        //reached.Add(coordinates, grid[coordinates]);

        while (openQueue.count > 0 && isRunning)
        {
            currentSearchNode = openQueue.Dequeue();
            currentSearchNode.isExplored = true;    // Update the state of current node

            // If current node is the destination node, stop searching
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                grid[destinationCoordinates].parent = grid[currentSearchNode.coordinates].Parent;
                isRunning = false;
            }
            else
            {
                ExploreNeighbors(); // Find neighbor nodes
                // Add current search node to reached list
                reached.Add(currentSearchNode.coordinates, currentSearchNode);
            }
        }
    }

    /// <summary>
    /// Set f, g, and h values for node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private Node SetNodeValues(Node node)
    {
        node.g = SetGValue(node);
        node.h = SetHValue(node);
        node.f = node.g + node.h;
        return node;
    }

    /// <summary>
    /// Calculate node g value (cost 10 for orthognal
    /// </summary>
    /// <returns></returns>
    int SetGValue(Node node)
    {
        if (node.parent != null)
        {
            // Set g to 10 + parent.g value if current node is in an orthogonal location from parent
            foreach (Vector2Int direction in orthogonalDirections)
            {
                Vector2Int neighborCoordinates = node.Parent.coordinates + direction;
                if (neighborCoordinates == node.coordinates)
                {
                    return 10 + currentSearchNode.G;
                }
            }

            // Set g to 14 + parent.g value if current node is in an diagonal location from parent
            foreach (Vector2Int direction in diagonallDirections)
            {
                Vector2Int neighborCoordinates = node.Parent.coordinates + direction;
                if (neighborCoordinates == node.coordinates)
                {
                    return 14 + currentSearchNode.G;
                }
            }
        }
        // No parent = set to 0 
        return 0;
    }

    /// <summary>
    /// Get new G value
    /// </summary>
    /// <param name="node1"></param>
    /// <param name="node2"></param>
    /// <returns></returns>
    int NewGValue(Node node1, Node node2)
    {
        foreach (Vector2Int direction in orthogonalDirections)
        {
            Vector2Int neighborCoordinates = node2.coordinates + direction;
            if (neighborCoordinates == node1.coordinates)
            {
                return 10 + currentSearchNode.G;
            }
        }

        // Set g to 14 + parent.g value if current node is in an diagonal location from parent
        foreach (Vector2Int direction in diagonallDirections)
        {
            Vector2Int neighborCoordinates = node2.coordinates + direction;
            if (neighborCoordinates == node1.coordinates)
            {
                return 14 + currentSearchNode.G;
            }
        }
        return 0;
    }

    /// <summary>
    /// Calculate node h value (cost is 10/tile)
    /// </summary>
    /// <returns></returns>
    int SetHValue(Node node)
    {
        // Get the x distance
        int xValue = Mathf.Abs(Mathf.RoundToInt(gridManager.GetPositionFromCoordinates(destinationCoordinates).x)
            - Mathf.RoundToInt(gridManager.GetPositionFromCoordinates(node.coordinates).x));

        // Get the y distance
        int yValue = Mathf.Abs(Mathf.RoundToInt(gridManager.GetPositionFromCoordinates(destinationCoordinates).z)
            - Mathf.RoundToInt(gridManager.GetPositionFromCoordinates(node.coordinates).z));

        return (xValue + yValue);
    }

    /// <summary>
    /// Creates a list of nodes with their coordinates
    /// Keeps track of what is explored and walkable
    /// </summary>
    private void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        // Find neighbor nodes from current nodes
        foreach (Vector2Int direction in directions)
        {
            // Take the current coordinate and adds each direction coordinates to it
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction;

            if (grid.ContainsKey(neighborCoordinates))
            {
                // Check if neighbor is in open queue
                if (openQueue.Contains(grid[neighborCoordinates]))
                {
                    int newGValue = NewGValue(grid[neighborCoordinates], currentSearchNode);
                    // Check if need to update G cost/parent of neighbor
                    if (newGValue < grid[neighborCoordinates].G)
                    {
                        // Update date parent
                        grid[neighborCoordinates].parent = currentSearchNode;
                        // Update G value of neighbor
                        grid[neighborCoordinates].G = newGValue;
                    }
                }
                else if (!reached.ContainsValue(grid[neighborCoordinates]))
                {
                    // Update date parent
                    grid[neighborCoordinates].parent = currentSearchNode;
                    // Get values of neighbor and add to list
                    neighbors.Add(SetNodeValues(grid[neighborCoordinates]));
                }
            }
        }

        // Loop through the new neighbors nodes found and add to open list
        // Check if neighbor node is not in closed list (as to not add it twice)
        // Check if neighbor node is walkable or not (Only add walkable node to frontier
        foreach (Node neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                openQueue.Enqueue(neighbor);
            }
        }//*/
    }

    /// <summary>
    /// Will look through the found path from destination to starting node
    /// The reverse the path to starting to destination
    /// </summary>
    /// <returns></returns>
    List<Node> BuildPath()
    {
        // List for the path found
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        // Change the isPath of the current node to true
        // This will state the node is part of the path
        currentNode.isPath = true;

        // Go through the node that the current node is connected to
        // Set the connected node to be the current node, add it to the path list, and set it's isPath to true
        // Repeat until there is a node not connected to another node (starting node)
        while (currentNode.parent != null)
        {
            // Set the connected node to be the current node
            currentNode = currentNode.parent;
            // add new node to the path list
            path.Add(currentNode);
            // Change the isPath of the current node to true
            // This will state the node is part of the path
            currentNode.isPath = true;
        }

        // Reverise the list so it goes from starting to destintion
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Broadcast a message (call a method) with or without receivers
    /// </summary>
    public void NotifyReceivers()
    {
        BroadcastMessage("RecalcuatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}