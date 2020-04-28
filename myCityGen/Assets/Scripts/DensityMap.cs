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

    public MapData MapData;
    public MeshData MeshData;
    public Texture2D Texture;

    public void Initialize(int width, int length)
    {
        Width = width;
        Length = length;
        MapData = new MapData(Width, Length);
        MeshData = new MeshData(width, length, 16);

    }

    public void Instantiate(int width, int length)
    {
        Tiles = new Tile[width, length];
        Colors = new Color[width * length];
        MapData = new MapData(Width, Length);
        MeshData = new MeshData(4, 4);
    }

    public void Initialize(int width, int length)
    {
        Width = width;
        Length = length;
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
                    Stack<Vector2> stack = new Stack<Vector2>();
                    stack.Push(new Vector2(x, z));

                    float max = float.MinValue;
                    Vector2 maxPos = new Vector2();

                    int T = Densities.GetIndex(MapData.Values[x, z]);

                    while (stack.Count > 0)
                    {
                        Vector2 current = stack.Pop();
                        int x1 = (int)current.x;
                        int z1 = (int)current.y;
                        float val = MapData.Values[x1, z1];

                        int n = 0;

                        if (x1 > 0)
                            n += CheckAndPush(x1 - 1, z1, T, stack);
                        else
                            n++;

                        if (x1 < Width - 1)
                            n += CheckAndPush(x1 + 1, z1, T, stack);
                        else
                            n++;

                        if (z1 > 0)
                            n += CheckAndPush(x1, z1 - 1, T, stack);               
                        else
                            n++;

                        if (z1 < Length - 1)
                            n += CheckAndPush(x1, z1 + 1, T, stack);
                        else
                            n++;

                        if (n == 4 && val > max)
                        {
                            max = val;
                            maxPos.x = x1; maxPos.y = z1;
                        }

                        DensityType type = Densities.GetType(T);
                        Colors[x1 + z1 * Length] = type.Color;
                        Tiles[x1, z1] = new Tile(type);
                    }

                    if (max > 0)
                    {
                        Colors[(int)maxPos.x + (int)maxPos.y * Length] = Color.black;
                    }
    
                }

            }
        }
    }

    private int CheckAndPush(int x, int z, int t, Stack<Vector2> stack)
    {
        float value = MapData.Values[x, z];
        if (Densities.GetIndex(value) == t)
        {
            if (Tiles[x, z] == null)
                stack.Push(new Vector2(x, z));
            return 1;
        }
        return 0;
    }

}
