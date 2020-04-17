using UnityEngine;

public static class DensityTextureGenerator
{

    // Density Map Colors
    private static Color UrbanColor = new Color();
    private static Color SuburbanColor = new Color();
    private static Color RuralColor = new Color();
    private static Color WildernessColor = new Color();

    public static Texture2D GetDensityTexture(int width, int height, Tile[,] tiles)
    {
        var texture = new Texture2D(width, height);
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                switch (tiles[x, y].DensityType)
                {
                    case DensityType.Urban:
                        pixels[x + y * width] = UrbanColor;
                        break;
                    case DensityType.Suburban:
                        pixels[x + y * width] = SuburbanColor;
                        break;
                    case DensityType.Rural:
                        pixels[x + y * width] = RuralColor;
                        break;
                    case DensityType.Wilderness:
                        pixels[x + y * width] = WildernessColor;
                        break;
                }

            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }


}