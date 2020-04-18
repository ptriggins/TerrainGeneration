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

    public Tile(int x, int y, float density, DensityType DensityType)
    {
        X = x;
        Y = y;
        Density = density;
        TopNeighbor = null;
        BottomNeighbor = null;
        LeftNeighbor = null;
        RightNeighbor = null;
    }

    // Classifies tile based on position within a region of like tiles
    public void SetBitmask()
    {
        int count = 0;

        if (TopNeighbor != null && TopNeighbor.DensityType == DensityType)
            count += 1;
        if (BottomNeighbor != null && BottomNeighbor.DensityType == DensityType)
            count += 4;
        if (LeftNeighbor != null && LeftNeighbor.DensityType == DensityType)
            count += 8;
        if (RightNeighbor != null && RightNeighbor.DensityType == DensityType)
            count += 2;

        Bitmask = count;
    }

}
