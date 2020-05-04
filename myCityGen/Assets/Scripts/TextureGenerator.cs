using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D GenerateDensityTexture(int width, int length, Color[] pixels)
    {
        var texture = new Texture2D(width, length);
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    public static Texture2D GenerateRoadTexture(int width, int length, List<Segment> segments)
    {
        var texture = new Texture2D(width, length);
        Color[] pixels = new Color[width * length];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[x + z * length] = new Color(0, 0, 0, 0);
            }
        }

        for (int i = 0; i < segments.Count; i++)
        {
            Vector3 
                start = segments[i].StartNode.Position,
                end = segments[i].EndNode.Position;

            DrawLine(start, end, length, pixels, Color.black);
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    private static void DrawLine(Vector3 start, Vector3 end, int length, Color[]colors, Color color)
    {
        int
            x1 = (int)start.x, x2 = (int)end.x, z1 = (int)start.z, z2 = (int)end.z;

        int dx = (int)(x2 - x1);
        int dz = (int)(z2 - z1);
        int stepX, stepZ;

        if (dz < 0)
        {
            dz = -dz;
            stepZ = -1;
        }
        else
            stepZ = 1;

        if (dx < 0)
        {
            dx = -dx;
            stepX = -1;
        }
        else
            stepX = 1;

        dz <<= 1;
        dx <<= 1;

        float fraction = 0;
        colors[x1 + z1 * length] = color;

        if (dx > dz)
        {
            fraction = dz - (dx >> 1);
            while (Mathf.Abs(x1 - x2) > 1)
            {
                if (fraction >= 0)
                {
                    z1 += stepZ;
                    fraction -= dx;
                }
                x1 += stepX;
                fraction += dz;
                colors[x1 + z1 * length] = color;
            }
        }
        else
        {
            fraction = dx - (dz >> 1);
            while (Mathf.Abs(z1 - z2) > 1)
            {
                if (fraction >= 0)
                {
                    x1 += stepX;
                    fraction -= dz;
                }
                z1 += stepZ;
                fraction += dx;
                colors[x1 + z1 * length] = color;
            }
        }
    }
}