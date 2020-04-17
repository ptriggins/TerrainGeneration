using UnityEngine;
using System.Collections.Generic;


// Defines system to classify regions based on population density
public enum DensityType
{
    Urban = 1,
    Suburban = 2,
    Rural = 3,
    Wilderness = 4
}

public class Tile
{

    // Density
    public DensityType DensityType;
    public float DensityValue { get; set; }

    // Location and Appearance
    public int X, Y;
    public Color Color = Color.black;
    public int Bitmask;

    // Neighbors
    public Tile Left;
    public Tile Right;
    public Tile Top;
    public Tile Bottom;

    // Flags
    public bool FloodFilled;

    // Constructor
    public Tile()
    {
    }

    // Classifies tile based on position within a group of like tiles (edge vs internal)
    public void UpdateBitmask()
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
