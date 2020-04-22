using UnityEngine;
using AccidentalNoise;

public class MapData
{
    public float[,] Values;
    public float Min;
    public float Max;

    public MapData(int width, int height, ImplicitModuleBase module)
    {
        Values = new float[width, height];
        Min = float.MaxValue;
        Max = float.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Sample intervals
                float x1 = x / (float)width;
                float y1 = y / (float)height;

                float value = (float)module.Get(x1, y1);

                if (value > Max) Max = value;
                if (value < Min) Min = value;

                Values[x, y] = value;
            }
        }
    }
}