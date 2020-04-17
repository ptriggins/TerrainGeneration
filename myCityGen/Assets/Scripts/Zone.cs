using UnityEngine;
using System.Collections.Generic;

// Represents a contigious area where all tiles have the same density
public class Zone
{
    public DensityType DensityType;
    public List<Tile> Tiles;

    // Constructs a zone by flood filling from a start tile
    public Zone(Tile start)
    {
        Tiles = new List<Tile>();
        DensityType = start.DensityType;

        // Initializes a stack with a start tile
        Stack<Tile> stack = new Stack<Tile>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Tile tile = stack.Pop();
            Tiles.Add(tile);

            // Adds neighbors to stack if they are of the same density type as tiles in the zone
            if (InZone(tile.TopNeighbor))
                stack.Push(tile.TopNeighbor);
            if (InZone(tile.BottomNeighbor))
                stack.Push(tile.BottomNeighbor);
            if (InZone(tile.LeftNeighbor))
                stack.Push(tile.LeftNeighbor);
            if (InZone(tile.RightNeighbor))
                stack.Push(tile.RightNeighbor);
        }

    }

    // Checks if foreign tile has the same density type as tiles in the zone
    private bool InZone(Tile neighbor)
    {
        if (neighbor != null && !neighbor.Zoned && neighbor.DensityType == DensityType)
            return true;
        return false;
    }

}
