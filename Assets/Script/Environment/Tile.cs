using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable;

    /// <summary>
    /// A property to get isPlaceable variable
    /// Name used is the same as the return variable but
    /// starts with uppercase 
    /// </summary>
    public bool IsPlaceable { get { return isPlaceable; } }

    GridManager gridManager;    // Access to GridManager script
    Vector2Int coordinates = new Vector2Int();  // Store tile's coordinate

    /// <summary>
    /// Initialize gridManager and pathFinder
    /// </summary>
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    /// <summary>
    /// Get up coordinates from tile position
    /// </summary>
    private void Start()
    {
        if (gridManager != null)
        {
            // Pass in tile position to gridManager's method to converit it into coordinates
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            // If tile is not placeable
            // Set is as blocked
            if (!IsPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    void OnMouseDown()
    {

    }
}