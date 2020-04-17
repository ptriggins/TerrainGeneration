using UnityEngine;
using System.Collections.Generic;

public enum DensityType
{
    // urban
    // suburban
    // rural
}

public class Tile
{
    public DensityType DensityType;

    public float DensityValue { get; set; }
    public int X, Y;
    public int Bitmask;

    public Tile Left;
    public Tile Right;
    public Tile Top;
    public Tile Bottom;

    public bool Collidable;
    public bool FloodFilled;

    public Color Color = Color.black;

    public Tile()
    {
    }

    public void UpdatBitmask()
    {
        int count = 0;

        if (Collidable && Top != null && Top.DensityType == DensityType)
            count += 1;
        if (Collidable && Bottom != null && Bottom.DensityType == DensityType)
            count += 4;
        if (Collidable && Left != null && Left.DensityType == DensityType)
            count += 8;
        if (Collidable && Right != null && Right.DensityType == DensityType)
            count += 2;

        Bitmask = count;
    }

}
