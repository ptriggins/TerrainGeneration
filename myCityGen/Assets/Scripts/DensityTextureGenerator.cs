using UnityEngine;

public static class DensityTextureGenerator
{
    public static Texture2D GetDensityTexture(int width, int height, Tile[,] tiles)
    {
        var texture = new Texture2D(width, height);
        var pixels = new Color[width * height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                pixels[x + y * width] = tiles[x, y].DensityType.Color;
            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
}