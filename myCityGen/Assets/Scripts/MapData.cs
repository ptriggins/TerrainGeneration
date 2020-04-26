using UnityEngine;
using AccidentalNoise;

public class MapData
{
    public float[,] Values;
    public float Min;
    public float Max;

    public MapData(int width, int length)
    {
        Values = new float[width, length];
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void SetData(ImplicitModuleBase module)
    {
        int width = Values.GetLength(0);
        int length = Values.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                float x1 = x / (float)width;
                float y1 = y / (float)length;
                float value = (float)module.Get(x1, y1);
                Values[x, y] = value;

                if (value > Max) Max = value;
                if (value < Min) Min = value;
            }
        }
    }
}