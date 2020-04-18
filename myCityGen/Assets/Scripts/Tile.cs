using UnityEngine;

public class Tile
{
    public int X, Y;
    public float Density { get; set; }
    public DensityType DensityType;

    public Tile TopNeighbor, BottomNeighbor, LeftNeighbor, RightNeighbor;
    public int Bitmask;
    public Color Color;
    public bool Zoned;

    public Tile(int x, int y, float value)
    {
        X = x;
        Y = y;
        Density = value;
        DenstiyType = GetDensityType(Value);
    }

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
