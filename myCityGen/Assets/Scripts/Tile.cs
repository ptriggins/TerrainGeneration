using UnityEngine;

public class Tile
{
    public Vector2 Coords;
    public CityType Type;
    public Color Color;

    public Tile(Vector2 coords, CityType type, Color color)
    {
        Coords = coords;
        Type = type;
        Color = color;
    }

}
