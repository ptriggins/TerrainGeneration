using UnityEngine;
using System.Collections.Generic;

public class Zone
{
    public CityType Type;
    public List<Tile> Tiles;

    public Zone()
    {
        Tiles = new List<Tile>();
    }

}
