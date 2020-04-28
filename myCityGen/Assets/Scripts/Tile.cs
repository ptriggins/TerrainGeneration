using UnityEngine;

public class Tile
{
    public DensityType Type;
    public float Value;
    public Color Color;

    public Tile(DensityType type, float value)
    {
        Type = type;
        Value = value;
        Color = type.Color;
    }
}
