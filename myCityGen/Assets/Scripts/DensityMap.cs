﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoise;

public class DensityMap
{
    public int Width;
    public int Length;
    public List<DensityType> DensityTypes;

    public Tile[,] Tiles;
    public Color[] Colors;
    public List<Zone>[] Zones;

    public MapData MapData;
    public MeshData MeshData;
    public Texture2D Texture;

    public DensityMap(int width, int length, List<DensityType> densityTypes)
    {
        Width = width;
        Length = length;
        DensityTypes = densityTypes;

        Tiles = new Tile[width, length];
        Colors = new Color[width * length];

        Zones = new List<Zone>[densityTypes.Count];
        for (int i = 0; i < densityTypes.Count; i++)
        {
            Zones[i] = new List<Zone>();
        }

        MapData = new MapData(Width, Length);
        MeshData = new MeshData(width, length);

    }

    public void SetMap()
    {
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Length; x++)
            {
                if (Tiles[x, z] == null)
                {
                    Zone zone = new Zone();
                    int T = GetTypeIndex(GetVal(x, z));
                    zone.Type = DensityTypes[T];

                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;

                        float val = GetVal(x1, z1);
                        float leftVal = 0, rightVal = 0, topVal = 0, bottomVal = 0;

                        int count = 0;
                        if (x1 > 0)
                        {
                            leftVal = GetVal(x1 - 1, z1);
                            if (GetTypeIndex(leftVal) == T)
                            {
                                count++;
                                if (Tiles[x1 - 1, z1] == null)
                                    stack.Push(current + new Vector2(-1, 0));
                            }
                        }
                        else
                            count++;

                        if (x1 < Width - 1)
                        {
                            rightVal = GetVal(x1 + 1, z1);
                            if (GetTypeIndex(rightVal) == T)
                            {
                                count++;
                                if (Tiles[x1 + 1, z1] == null)
                                    stack.Push(current + new Vector2(1, 0));
                            }   
                        }
                        else
                            count++;

                        if (z1 > 0)
                        {
                            topVal = GetVal(x1, z1 - 1);
                            if (GetTypeIndex(topVal) == T)
                            {
                                count++;
                                if (Tiles[x1, z1 - 1] == null)
                                    stack.Push(current + new Vector2(0, -1));
                            }
                        }
                        else
                            count++;

                        if (z1 < Length - 1)
                        {
                            bottomVal = GetVal(x1, z1 + 1);
                            if (GetTypeIndex(bottomVal) == T)
                            {
                                count++;
                                if (Tiles[x1, z1 + 1] == null)
                                    stack.Push(current + new Vector2(0, 1));
                            }
                        }
                        else
                            count++;

                        DensityType type = DensityTypes[T];

                        Color color;
                        if (val > topVal && val > bottomVal && val > leftVal && val > rightVal)
                            color = Color.red;
                        else if (count == 4)
                            color = type.Color;
                        else
                            color = Color.black;
                        Colors[x1 + z1 * Length] = color;

                        Tile tile = new Tile(current, type, color);
                        zone.Tiles.Add(tile);
                        Tiles[x1, z1] = tile;
                    }

                    Zones[T].Add(zone);     
                }

            }
        }
    }

    private int GetTypeIndex(float val)
    {
        for (int i = 0; i < DensityTypes.Count; i++)
        {
            if (val < DensityTypes[i].Percentile)
            {
                return i;
            }
        }
        return DensityTypes.Count - 1;
    }

    private float GetVal(int x, int z)
    {
        float val = MapData.Values[x, z];
        return (val - MapData.Min) / (MapData.Max - MapData.Min);
    }

}
