using UnityEngine;

public class Tile
{
    public Vector2 Coords;
    public DensityType Type;
    public Color Color;

    public Tile(Vector2 coords, DensityType type, Color color)
    {
        Coords = coords;
        Type = type;
        Color = color;
    }

}
