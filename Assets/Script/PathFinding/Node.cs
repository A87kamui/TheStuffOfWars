using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public Node parent;    // Store connected node
    public bool isWalkable; // Will check if node is walkable or not
    public bool isExplored; // Check if node was already explored
    public bool isPath; // Check if node is in path
    public int f, g, h;


    /// <summary>
    /// Constructor to construct a Node object
    /// </summary>
    /// <param name="coordinates"></param>
    /// <param name="isWalkable"></param>
    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }

    /// <summary>
    /// Property to get coordinates
    /// </summary>
    public Vector2Int Coordinates
    {
        get { return coordinates; }
    }

    public Node Parent
    {
        get { return parent; }
    }

    /// <summary>
    /// Property to get/set total score of node
    /// </summary>
    public int F
    {
        get { return f; }
        set { f = g + h; }
    }

    /// <summary>
    /// Property to get the cost to move to node from starting node
    /// </summary>
    public int G
    {
        get { return g; }
        set { g = value; }
    }

    /// <summary>
    /// Get the heuristic cost from node to goal (no diagonal cost)
    /// </summary>
    public int H
    {
        get { return h; }
        set { h = value; }
    }

    /// <summary>
    /// Check if coordinates matches
    /// </summary>
    /// <param name="nNode"></param>
    /// <returns></returns>
    public bool Equal(object nNode)
    {
        Node n = (Node)nNode;
        return coordinates == n.coordinates;
    }

    /// <summary>
    /// Reset variables to initial state
    /// </summary>
    public void ResetValues()
    {
        parent = null;
        isExplored = false;
        isPath = false;
        f = 0;
        g = 0;
        h = 0;
    }

    /// <summary>
    /// Return string values of Node
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        if (parent != null)
        {
            return base.ToString() + " " + coordinates + ": F= " + f + " G= " + g + " H= " + h + " Parent: " + parent.Coordinates;
        }
        return base.ToString() + " " + coordinates + ": F= " + f + " G= " + g + " H= " + h + " Parent: Null";
    }

}