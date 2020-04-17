using System.Collections.Generic;

public enum TileGroupType
{
    Urban,
    Suburbarban,
    Rural,
    Wilderness
}

public class TileGroup
{
    public TileGroupType Type;
    public List<Tile> Tiles;

    public TileGroup()
    {
        Tiles = new List<Tile>();
    }
}
