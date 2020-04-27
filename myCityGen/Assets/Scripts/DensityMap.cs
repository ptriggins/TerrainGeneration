using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityMap
{
    public Tile[,] Tiles;
    public List<List<Zone>> Zones;

    List<DensityType> DensityTypes;
    int Width, Length, NumTypes;

    public DensityMap(int width, int length, List<DensityType> densityTypes)
    {
        Width = width; Length = length;
        Tiles = new Tile[width, length];
        Zones = new List<List<Zone>>();

        NumTypes = densityTypes.Count;
        DensityTypes = densityTypes;
        NumTypes = densityTypes.Count;
    }

    public void SetTiles(MapData mapdata)
    {
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Length; x++)
            {
                if (Tiles[x, z] == null)
                {
                    Zone zone = new Zone();
                    int T = GetTypeIndex(x, z, mapdata);
                    zone.Type = DensityTypes[T];

                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;

                        int count = 0;
                        if (x1 > 0)
                        {
                            if (GetTypeIndex(x1 - 1, z1, mapdata) == T)
                            {
                                stack.Push(current + new Vector2(-1, 0));
                                count++;
                            }
                        }
                        if (x1 < Width - 1)
                        {
                            if (GetTypeIndex(x1 + 1, z1, mapdata) == T)
                            {
                                stack.Push(current + new Vector2(1, 0));
                                count++;
                            }

                        }
                        if (z1 > 0)
                        {
                            if (GetTypeIndex(x1, z1 - 1, mapdata) == T)
                            {
                                stack.Push(current + new Vector2(0, -1));
                                count++;
                            }
                        }
                        if (z1 < Length - 1)
                        {
                            if (GetTypeIndex(x1, z + 1, mapdata) == T)
                            {
                                stack.Push(current + new Vector2(0, 1));
                                count++;
                            }

                        }

                        DensityType type = DensityTypes[T];

                        Color color;
                        if (count == 4)
                            color = type.Color;
                        else
                            color = Color.black;

                        Tile tile = new Tile(current, type, color);
                        zone.Tiles.Add(tile);
                        Tiles[x1, z1] = tile;
                    }

                    Zones[T].Add(zone);
                }
                
            }
        }
    }

    private int GetTypeIndex(int x, int z, MapData mapdata)
    {
        float value = Normalize(mapdata.Values[x, z], mapdata.Max, mapdata.Min);
        for (int i = 0; i < NumTypes; i++)
        {
            if (value < DensityTypes[i].Percentile)
            {
                return i;
            }
        }
        return NumTypes - 1;
    }

    private float Normalize(float val, float max, float min)
    {
        return (val - min) / (max - min);
    }

}
