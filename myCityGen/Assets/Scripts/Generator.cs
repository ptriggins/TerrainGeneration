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
    public int Octaves = 6;              // Level of complexity
    [SerializeField]
    public double Frequency = 1.25;      // Interval between samples

    public DensityType[] DensityTypes;

    // Store noise samples
    ImplicitFractal HeightMap;
    MapData DensityData;

    public Tile[,] Tiles;

    List<Zone>[] Zones;

    // Displays map texture
    public MeshRenderer DensityRenderer;

    // First execution
    void Start()
    {
        Instantiate();
        Generate();
    }

    private void Initialize()
    {
        // Initialize the noise fractal
        HeightMap = new ImplicitFractal(FractalType.MULTI,
                                       BasisType.SIMPLEX,
                                       InterpolationType.QUINTIC,
                                       Octaves,
                                       Frequency,
                                       Random.Range(0, int.MaxValue));
    }

    // Press F5 to refresh after adjusting inspector values
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Initialize();
            Generate();
        }
    }

    protected virtual void Instantiate()
    {
        DensityRenderer = transform.Find("DensityTexture").GetComponent<MeshRenderer>();
        Initialize();
    }

    protected virtual void Generate()
    {
        GetData(HeightMap, ref DensityData);
        LoadTiles();
        SetNeighbors();
        SetBitmasks();
        ZoneMap();

        DensityRenderer.materials[0].mainTexture = DensityTextureGenerator.GetDensityTexture(Width, Height, Tiles);
    }

    private void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData(Width, Height);

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                // Interval of nosie sampling
                float x1 = x / (float)Width;
                float y1 = y / (float)Height;

                float value = (float)HeightMap.Get(x1, y1);

                // Tracks min and max values
                if (value > mapData.Max) mapData.Max = value;
                if (value < mapData.Min) mapData.Min = value;

                mapData.Data[x, y] = value;
            }
        }
    }

    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                float density = DensityData.Data[x, y];
                density = (density - DensityData.Min) / (DensityData.Max - DensityData.Min);            // Density as percentage of total range

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
                Debug.Log(Tiles[x, y].DensityType.Name);
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

}
