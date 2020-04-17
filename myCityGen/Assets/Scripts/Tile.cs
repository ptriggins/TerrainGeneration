using UnityEngine;

// A portion of the map of a particular density level
public class Tile
{

    // Density
    public DensityType DensityType;
    public float DensityLevel { get; set; }

    // Location and Appearance
    public int X, Y;
    public Color Color = Color.black;
    public int Bitmask;

    // Neighbors
    public Tile TopNeighbor, BottomNeighbor, LeftNeighbor, RightNeighbor;

    public bool Zoned;

    // Classifies tile based on position within a group of like tiles (edge vs internal)
    public void UpdateBitmask()
    {
        int count = 0;

        if (Top != null && Top.DensityType == DensityType)
            count += 1;
        if (Bottom != null && Bottom.DensityType == DensityType)
            count += 4;
        if (Left != null && Left.DensityType == DensityType)
            count += 8;
        if (Right != null && Right.DensityType == DensityType)
            count += 2;

        Bitmask = count;
    }

}
