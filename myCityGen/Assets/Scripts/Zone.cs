using UnityEngine;
using System.Collections.Generic;

public class Zone
{
    public DensityType Type;
    public List<Tile> Tiles;

    public Zone()
    {
        Tiles = new List<Tile>();
    }

}
