using UnityEngine;
using System.Collections.Generic;

public class Zone
{
    public List<Tile> Tiles;
    public DensityType Type;

    public Zone()
    {
        Tiles = new List<Tile>();
    }

    public void Build(Tile first)
    {
        Type = first.Type;
        Stack<Tile> stack = new Stack<Tile>();
        stack.Push(first);

        while (stack.Count > 0)
        {
            Tile tile = stack.Pop();
            Tiles.Add(tile);

            DensityType topType = GetDensityType(x, z + 1, mapdata);
            DensityType rightType = GetDensityType(x + 1, z, mapdata);
            DensityType bottomType = GetDensityType(x, z - 1, mapdata);
            DensityType leftType = GetDensityType(x - 1, z, mapdata);
            if ()

            if ()
                stack.Push(tile.BottomNeighbor);
            if ()
                stack.Push(tile.LeftNeighbor);
            if ()
                stack.Push(tile.RightNeighbor);
        }
    }

    private bool InZone(Tile neighbor)
    {
        if (neighbor != null && neighbor.DensityType.Name == DensityType.Name)
            return true;
        return false;
    }

}
