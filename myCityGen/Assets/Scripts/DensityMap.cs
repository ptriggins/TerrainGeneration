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
                DensityType type = DensityTypes[GetTypeIndex(x, z)];
                Colors[x + z * Length] = type.Color;
                Tiles[x, z] = new Tile(new Vector2(x, z), type, type.Color);


                /*
                if (Tiles[x, z] == null)
                {
                    Zone zone = new Zone();
                    int T = GetTypeIndex(x, z);
                    zone.Type = DensityTypes[T];

                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;

                        int count = 0;
                        if (x1 > 0 && Tiles[x1 - 1, z1] == null)
                        {
                            if (GetTypeIndex(x1 - 1, z1) == T)
                            {
                                stack.Push(current + new Vector2(-1, 0));
                                count++;
                            }
                        }
                        if (x1 < Width - 1 && Tiles[x1 + 1, z1] == null)
                        {
                            if (GetTypeIndex(x1 + 1, z1) == T)
                            {
                                stack.Push(current + new Vector2(1, 0));
                                count++;
                            }
                        }
                        if (z1 > 0 && Tiles[x1, z1 - 1] == null)
                        {
                            if (GetTypeIndex(x1, z1 - 1) == T)
                            {
                                stack.Push(current + new Vector2(0, -1));
                                count++;
                            }
                        }
                        if (z1 < Length - 1 && Tiles[x1, z + 1] == null)
                        {
                            if (GetTypeIndex(x1, z + 1) == T)
                            {
                                stack.Push(current + new Vector2(0, 1));
                                count++;
                            }
                        }

                        DensityType type = DensityTypes[T];

                        Color color;
                        //if (count == 4)
                            color = type.Color;
                        //else
                            //color = Color.black;
                        Colors[x1 + z1 * Length] = color;

                        Tile tile = new Tile(current, type, color);
                        zone.Tiles.Add(tile);
                        Tiles[x1, z1] = tile;
                    }

                    Zones[T].Add(zone);
                   
                }
                */

            }
        }
    }

    private int GetTypeIndex(int x, int z)
    {
        float val = MapData.Values[x, z];
        float normVal = Normalize(val, MapData.Max, MapData.Min);

        for (int i = 0; i < DensityTypes.Count; i++)
        {
            if (normVal < DensityTypes[i].Percentile)
            {
                return i;
            }
        }
        return DensityTypes.Count - 1;
    }

    private float Normalize(float val, float max, float min)
    {
        return (val - min) / (max - min);
    }

}
