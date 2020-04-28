using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AccidentalNoise;

[System.Serializable]
public struct DensityType
{
    public string Name;
    public float Percentile;
    public Color Color;
}

public class DensityMap : MonoBehaviour
{
    private int Width;
    private int Length;

    [Header("Density Types")]
    [SerializeField]
    public DensityType[] Types;

    [Header("Noise")]
    [SerializeField]
    public int Octaves = 6;
    [SerializeField]
    public double Frequency = 1.25;

    public Tile[,] Tiles;
    private Color[] Colors;
    private Texture2D Texture;
    private Display Display;

    public MapData MapData;
    public MeshData MeshData;
    private ImplicitFractal Noise;

    public void Initialize(int width, int length)
    {
        Width = width;
        Length = length;
        Display = (Display)FindObjectOfType(typeof(Display));
        Instantiate();
    }

    public void Instantiate()
    {
        Tiles = new Tile[Width, Length];
        Colors = new Color[Width * Length];
        MapData = new MapData(Width, Length);
        MeshData = new MeshData(4, 4);
        Noise = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC,
            Octaves, Frequency, Random.Range(0, int.MaxValue));
    }

    public void Generate()
    {
        Noise = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC,
            Octaves, Frequency, Random.Range(0, int.MaxValue));

        MapData.Calculate(Noise);
        MeshData.Calculate(MapData.Values);

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

                    int T = GetTypeIndex(MapData.Values[x, z]);

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

                        DensityType type = Types[GetTypeIndex(val)];
                        Colors[x1 + z1 * Length] = type.Color;
                        Tiles[x1, z1] = new Tile(type, val);
                    }

                    if (max > 0)
                        Colors[(int)maxPos.x + (int)maxPos.y * Length] = Color.black;
                }
            }
        }
        Texture = TextureGenerator.GetDensityTexture(Width, Length, Colors);

        int CheckAndPush(int x, int z, int t, Stack<Vector2> stack)
        {
            float value = MapData.Values[x, z];
            if (GetTypeIndex(value) == t)
            {
                if (Tiles[x, z] == null)
                    stack.Push(new Vector2(x, z));
                return 1;
            }
            return 0;
        }

        int GetTypeIndex(float val)
        {
            for (int i = 0; i < Types.Length; i++)
            {
                if (val <= Types[i].Percentile)
                    return i;
            }
            return -1;
        }
    }

    public void Draw()
    {
        MeshData.RefreshMesh(Display.Mesh);
        Display.Draw(Texture);
    }
}
