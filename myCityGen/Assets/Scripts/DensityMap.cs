using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoise;

public class DensityMap
{
    public int Width;
    public int Length;
    public List<CityType> CityTypes;

    public Tile[,] Tiles;
    public Color[] Colors;
    public List<Zone>[] Zones;
    public List<PopCenter> PopCenters;

    public MapData MapData;
    public MeshData MeshData;
    public Texture2D Texture;

    public DensityMap(int width, int length, List<CityType> cityTypes)
    {
        Width = width;
        Length = length;
        CityTypes = cityTypes;

        Tiles = new Tile[width, length];
        Colors = new Color[width * length];
        Zones = new List<Zone>[cityTypes.Count];
        PopCenters = new List<PopCenter>();

        for (int i = 0; i < cityTypes.Count; i++)
        {
            Zones[i] = new List<Zone>();
        }

        MapData = new MapData(Width, Length);
        MeshData = new MeshData(width, length, 16);

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
                    int T = GetTypeIndex(MapData.Values[x, z]);
                    zone.Type = CityTypes[T];

                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    float max = float.MinValue;
                    Vector2 maxPos = new Vector2();

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;

                        float val = MapData.Values[x1, z1];
                        float leftVal = 0, rightVal = 0, topVal = 0, bottomVal = 0;

                        int count = 0;
                        if (x1 > 0)
                        {
                            leftVal = MapData.Values[x1 - 1, z1];
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
                            rightVal = MapData.Values[x1 + 1, z1];
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
                            topVal = MapData.Values[x1, z1 - 1];
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
                            bottomVal = MapData.Values[x1, z1 + 1];
                            if (GetTypeIndex(bottomVal) == T)
                            {
                                count++;
                                if (Tiles[x1, z1 + 1] == null)
                                    stack.Push(current + new Vector2(0, 1));
                            }
                        }
                        else
                            count++;

                        CityType type = CityTypes[T];

                        Color color;
                        if (count == 4 && val > max)
                        {
                            max = val;
                            maxPos.x = x1; maxPos.y = z1;
                        }
                        color = type.Color;
                        //else
                            //color = Color.black;
                        Colors[x1 + z1 * Length] = color;

                        Tile tile = new Tile(current, type, color);
                        zone.Tiles.Add(tile);
                        Tiles[x1, z1] = tile;
                    }

                    if (max > 0)
                    {
                        Colors[(int)maxPos.x + (int)maxPos.y * Length] = Color.black;
                    }

                    Zones[T].Add(zone);     
                }

            }
        }
    }

    public int GetTypeIndex(float val)
    {
        for (int i = 0; i < CityTypes.Count; i++)
        {
            if (val < CityTypes[i].Percentile)
            {
                return i;
            }
        }
        return CityTypes.Count - 1;
    }

    public CityType GetType(float val)
    {
        return CityTypes[GetTypeIndex(val)];
    }

}
