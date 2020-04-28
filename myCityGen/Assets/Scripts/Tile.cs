using UnityEngine;

public class Tile
{
    public DensityType Type;
    public Color Color;

    public Tile(DensityType type)
    {
        Type = type;
        Color = type.Color;
    }
}
