using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DensityType
{
    public string Name;
    public float Percentile;
    public Color Color;

}

public class DensityTypes
{
    public List<DensityType> Types;

    public int GetIndex(float val)
    {
        for (int i = 0; i < Types.Count - 1; i++)
        {
            if (val < Types[i].Percentile)
                return i;
        }
        return -1;
    }

    public DensityType GetType(float val)
    {
        return Types[GetIndex(val)];
    }

}
