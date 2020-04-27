using UnityEngine;
using System.Collections.Generic;

public class Zone
{
    public DensityType Type;
    public List<Tile> Tiles;
    public float MaxVal;
    public Vector2 MaxPos;

    public Zone()
    {
        Tiles = new List<Tile>();
        MaxVal = float.MinValue;
    }

}
