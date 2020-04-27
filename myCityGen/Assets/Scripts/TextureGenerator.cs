using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D GetDensityTexture(int width, int length, Color[] pixels)
    {
        var texture = new Texture2D(width, length);
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
}