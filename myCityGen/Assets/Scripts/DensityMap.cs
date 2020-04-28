using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoise;

public class DensityMap : MonoBehaviour
{
    public int Width;
    public int Length;

    [Header("Density Types")]
    [SerializeField]
    public DensityTypes Densities;

    public Tile[,] Tiles;
    public Color[] Colors;
    public List<PopCenter> PopCenters;

    public MapData MapData;
    public MeshData MeshData;
    public Texture2D Texture;

    public void Instantiate(int width, int length)
    {
        Tiles = new Tile[width, length];
        Colors = new Color[width * length];
    }

    public DensityMap(int width, int length)
    {
        Width = width;
        Length = length;

        PopCenters = new List<PopCenter>();

        MapData = new MapData(Width, Length);
        MeshData = new MeshData(width, length, 16);

    }

    public void SetMap()
    {
        float[,] values = MapData.Values;
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Length; x++)
            {
                if (Tiles[x, z] == null)
                {
                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    float max = float.MinValue;
                    Vector2 maxPos = new Vector2();

                    int T = Densities.GetIndex(values[x, z]);

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;
                        float val = values[x1, z1];
                        float left = 0, right = 0, top = 0, bottom = 0;

                        int count = 0;
                        if (x1 > 0)
                        {
                            left = values[x1 - 1, z1];
                            if (Densities.GetIndex(left) == T)
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
                            right = MapData.Values[x1 + 1, z1];
                            if (Densities.GetIndex(right) == T)
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
                            top = MapData.Values[x1, z1 - 1];
                            if (Densities.GetIndex(top) == T)
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
                            bottom = MapData.Values[x1, z1 + 1];
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
                        Tiles[x1, z1] = tile;
                    }

                    if (max > 0)
                    {
                        Colors[(int)maxPos.x + (int)maxPos.y * Length] = Color.black;
                    }
    
                }

            }
        }
    }

}
