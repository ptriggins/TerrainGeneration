using UnityEngine;
using System.Collections.Generic;
using AccidentalNoise;

[System.Serializable]
public struct DensityType
{
    public string Name;
    public float Percentile;
    public Color Color;
}

public class Generator : MonoBehaviour
{

    [Header("Map Size")]
    [SerializeField]
    public int Width = 512;
    [SerializeField]
    public int Height = 512;

    [Header("Noise")]
    [SerializeField]
    public int Octaves = 6;                 // Level of complexity
    [SerializeField]
    public double Frequency = 1.25;         // Interval between samples

    public enum DrawMode {Texture, Mesh};
    [Header("Draw Mode")]
    [SerializeField]
    public DrawMode Mode;

    [Header("Density Definitions")]
    [SerializeField]
    public DensityType[] DensityTypes;

    // Noise sampling
    private ImplicitFractal HeightMap;
    private MapData DensityMap;

    // Store density data
    private Tile[,] Tiles;
    private List<Zone>[] Zones;

    //public AnimationCurve heightCurve;

    public void Generate()
    {
        HeightMap = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC, 
            Octaves, Frequency, Random.Range(0, int.MaxValue));
        DensityMap = new MapData(Width, Height, HeightMap);

        LoadTiles();
        SetNeighbors();
        SetBitmasks();
        ZoneMap();
        Draw(); 
    }

    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                float density = DensityMap.Values[x, y];
                density = (density - DensityMap.Min) / (DensityMap.Max - DensityMap.Min);            // Density as percentage of noise range

                DensityType densityType = DensityTypes[0];
                for (int i = 0; i < DensityTypes.Length; i++)
                {
                    if (density < DensityTypes[i].Percentile)
                    {
                        densityType = DensityTypes[i];
                        break;
                    }
                }

                Tile tile = new Tile(x, y, density, densityType);
                Tiles[x, y] = tile;
            }
        }
    }

    private void SetNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile tile = Tiles[x, y];
                
                if (y != 0)
                   tile.TopNeighbor = Tiles[x, y - 1];
                if (y != Height - 1)
                    tile.BottomNeighbor = Tiles[x, y + 1];
                if (x != 0)
                    tile.LeftNeighbor = Tiles[x - 1, y];
                if (x != Width - 1)
                    tile.RightNeighbor = Tiles[x + 1, y];
            }
        }
    }

    private void SetBitmasks()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tiles[x, y].SetBitmask();
            }
        }
    }

    private void ZoneMap()
    {
        Zones = new List<Zone>[DensityTypes.Length];
        for (int i = 0; i < DensityTypes.Length; i++)
        {
            Zones[i] = new List<Zone>();
        }

        Tile tile = Tiles[0, 0];
        Zone zone = new Zone(tile);

        for (int i = 0; i < DensityTypes.Length; i++)
        {
            if (zone.DensityType.Name == DensityTypes[i].Name)
                Zones[i].Add(zone);
        }

        /*
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                // Constructs new zone from first available unzoned tile
                Tile tile = Tiles[x, y];
                if (tile.Zoned) continue;
                Zone zone = new Zone(tile);

                for (int i = 0; i < DensityTypes.Length; i++)
                {
                    if (zone.DensityType.Name == DensityTypes[i].Name)
                        Zones[i].Add(zone);
                }

            }
        }
        */
    }

    private void Draw()
    {
        Display display = FindObjectOfType<Display>();
        Texture2D DensityTexture = TextureGenerator.GetDensityTexture(Width, Height, Tiles);

        if (Mode == DrawMode.Texture)
        {
            display.DrawTexture(DensityTexture);
        }
        else if (Mode == DrawMode.Mesh)
        {
            display.DrawMesh(new MeshData(DensityMap.Values), DensityTexture);
        }
    }

}
