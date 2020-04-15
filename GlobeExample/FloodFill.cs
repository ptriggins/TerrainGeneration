using UnityEngine;
using System.Collections.Generic;

public enum TileGroupType
{
    Water,
    Land
}

public class TileGroup  {

    public TileGroupType Type;
    public List<Tile> Tiles;

    public TileGroup()
    {
        Tiles = new List<Tile> ();
    }
}

private void FloodFill()
{
    // Use a stack instead of recursion
    Stack<Tile> stack = new Stack<Tile>();

    for (int x = 0; x < Width; x++) {
        for (int y = 0; y < Height; y++) {

            Tile t = Tiles[x,y];

            //Tile already flood filled, skip
            if (t.FloodFilled) continue;

            // Land
            if (t.Collidable)
            {
                TileGroup group = new TileGroup();
                group.Type = TileGroupType.Land;
                stack.Push(t);

                while(stack.Count > 0) {
                    FloodFill(stack.Pop(), ref group, ref stack);
                }

                if (group.Tiles.Count > 0)
                    Lands.Add (group);
            }
            // Water
            else {
                TileGroup group = new TileGroup();
                group.Type = TileGroupType.Water;
                stack.Push(t);

                while(stack.Count > 0)   {
                    FloodFill(stack.Pop(), ref group, ref stack);
                }

                if (group.Tiles.Count > 0)
                    Waters.Add (group);
            }
        }
    }
}


private void FloodFill(Tile tile, ref TileGroup tiles, ref Stack<Tile> stack)
{
    // Validate
    if (tile.FloodFilled)
        return;
    if (tiles.Type == TileGroupType.Land && !tile.Collidable)
        return;
    if (tiles.Type == TileGroupType.Water && tile.Collidable)
        return;

    // Add to TileGroup
    tiles.Tiles.Add (tile);
    tile.FloodFilled = true;

    // floodfill into neighbors
    Tile t = GetTop (tile);
    if (!t.FloodFilled && tile.Collidable == t.Collidable)
        stack.Push (t);
    t = GetBottom (tile);
    if (!t.FloodFilled && tile.Collidable == t.Collidable)
        stack.Push (t);
    t = GetLeft (tile);
    if (!t.FloodFilled && tile.Collidable == t.Collidable)
        stack.Push (t);
    t = GetRight (tile);
    if (!t.FloodFilled && tile.Collidable == t.Collidable)
        stack.Push (t);
}
