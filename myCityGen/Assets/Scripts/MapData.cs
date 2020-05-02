using UnityEngine;
using AccidentalNoise;

public class MapData
{
    public float[,] Values;
    public int Width, Length;
    public float Min, Max;

    public MapData(int width, int length)
    {
        Values = new float[width, length];
        Width = width;
        Length = length;
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void CalculateData(ImplicitModuleBase module)
    {
        int width = Values.GetLength(0);
        int length = Values.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                float x1 = x / (float)width;
                float z1 = z / (float)length;
                float value = (float)module.Get(x1, z1);
                Values[x, z] = value;

                if (value > Max)
                    Max = value;
                if (value < Min)
                    Min = value;         
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                Values[x, z] = (Values[x, z] - Min) / (Max - Min);
            }
        }
    }

    public float GetValue(Vector3 position)
    {
        int x = (int)Mathf.Floor(position.x);
        int z = (int)Mathf.Floor(position.z);

        if (x < 0 || x >= Width || z < 0 || z >= Length)
        {
            //Debug.Log("Error, Index out of Range: " + x + ", " + z);
            return -1f;
        }
        else
            return Values[x, z];
    }
}