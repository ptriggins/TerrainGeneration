using UnityEngine;

public class Tile
{
    public int X, Y;
    public float Density { get; set; }
    public DensityType DensityType;
    public Color Color;

    public Tile TopNeighbor, BottomNeighbor, LeftNeighbor, RightNeighbor;
    public int Bitmask;
    public bool Zoned;

    public Tile(int x, int y, float density, DensityType densityType)
    {
        X = x;
        Y = y;
        Density = density;
        DensityType = densityType;
        Color = densityType.Color;

        TopNeighbor = null;
        BottomNeighbor = null;
        LeftNeighbor = null;
        RightNeighbor = null;
    }

    // Darkens edge tiles
    public void SetBitmask()
    {
        int count = 0;
        if (TopNeighbor != null && TopNeighbor.DensityType.Name == DensityType.Name)
            count += 1;
        if (BottomNeighbor != null && BottomNeighbor.DensityType.Name == DensityType.Name)
            count += 4;
        if (LeftNeighbor != null && LeftNeighbor.DensityType.Name == DensityType.Name)
            count += 8;
        if (RightNeighbor != null && RightNeighbor.DensityType.Name == DensityType.Name)
            count += 2;

        if (count < 15)
            Color = Color.black;
        Bitmask = count;
    }
}
