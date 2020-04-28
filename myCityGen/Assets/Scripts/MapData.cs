﻿using UnityEngine;
using AccidentalNoise;

public class MapData
{
    public float[,] Values;
    public float Min;
    public float Max;
    public Vector3 MaxPos;

    public MapData(int width, int length)
    {
        Values = new float[width, length];
        Min = float.MaxValue;
        Max = float.MinValue;
        MaxPos = new Vector3(0, 0, 0);
    }

    public void Calculate(ImplicitModuleBase module)
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
                {
                    Max = value;
                    MaxPos.x = x1;
                    MaxPos.z = z1;
                }
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
}