using UnityEngine;
using System.Collections.Generic;

public class Zone
{
    public DensityType DensityType;
    public List<Tile> Tiles;

    // Flood fills form a start tile
    public Zone(Tile start)
    {
        Tiles = new List<Tile>();
        DensityType = start.DensityType;

        Stack<Tile> stack = new Stack<Tile>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            Tile tile = stack.Pop();
            Tiles.Add(tile);

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

    private bool InZone(Tile neighbor)
    {
        if (neighbor != null && !neighbor.Zoned && neighbor.DensityType == DensityType)
            return true;
        return false;
    }

}
