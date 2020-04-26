using UnityEngine;

public class Tile
{
    public enum Direction
    {
        Top, Bottom, Left, Right
    }

    public DensityType Type;
    public Color Color;
    public Tile[] Neighbors;

    public int Bitmask;
    public bool Zoned;

    public Tile(DensityType densityType)
    {
        DensityType = densityType;
        Color = densityType.Color;

        Neighbors = new Tile[4];
        for (int i = 0; i <  4; i++)
        {
            Neighbors[i] = null;
        }
    }

    public SetValue()

    public Color GetColor()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!CheckNeighbor(i))
                return Color.black;
        }
        return DensityType.Color;

        

        
        else if (Top.DensityType.Name != DensityType.Name || Bottom.DensityType.Name != DensityType.Name)

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

    public bool CheckNeighbor(int direction)
    {
        if (Neighbors[direction] == null)
            return false;
        else if (Neighbors[direction].DensityType.Name != DensityType.Name)
            return false;
        else
            return true;
    }
}
