using UnityEngine;
using System.Collections.Generic;
using AccidentalNoise;

public enum DensityType
{
    Urban,
    Suburban,
    Rural,
    Wilderness
}

public abstract class Generator : MonoBehaviour
{

    // Seed used to generate noise
    protected int Seed;

    // Sliders

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

    [Header("Density Level")]
    [SerializeField]
    public float Urban = 0.2f;
    [SerializeField]
    public float Suburban = 0.4f;
    [SerializeField]
    public float Rural = 0.5f;
    [SerializeField]
    public float Wilderness = 0.7f;


    // Noise Sampling
    ImplicitFractal HeightMap;
    MapData DensityData;

    protected Tile[,] Tiles;

    // Lists to store the zones  of the map'
    protected List<Zone> Cities = new List<Zone>();
    protected List<Zone> Suburbs = new List<Zone>();
    protected List<Zone> Farms = new List<Zone>();
    protected List<Zone> Forests = new List<Zone>();


    // Displays map texture
    protected MeshRenderer DensityRenderer;

    // Called on script's first execution
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
                                       UnityEngine.Random.Range(0, int.MaxValue));
    }

    // Called for each frame
    void Update()
    {
        // Press F5 to refresh after adjusting inspector values
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Seed = UnityEngine.Random.Range(0, int.MaxValue);
            Initialize();
            Generate();
        }
    }

    // Creates copy of the generator object, sets seed and renderer object
    protected virtual void Instantiate()
    {
        Seed = UnityEngine.Random.Range(0, int.MaxValue);
        DensityRenderer = transform.Find("DensityTexture").GetComponent<MeshRenderer>();
        Initialize();
    }

    protected virtual void Generate()
    {
        // Samples noise and builds the map
        GetData(HeightMap, ref DensityData);
        LoadTiles();
        UpdateNeighbors();

        UpdateBitmasks();
        ZoneMap();

        GenerateDensityMap();

        DensityRenderer.materials[0].mainTexture = DensityTextureGenerator.GetDensityTexture(Width, Height, Tiles);
    }

    private void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData(Width, Height);

        // loop through each x,y point - get height value
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                //Sample the noise at smaller intervals
                float x1 = x / (float)Width;
                float y1 = y / (float)Height;

                float value = (float)HeightMap.Get(x1, y1);

                //keep track of the max and min values found
                if (value > mapData.Max) mapData.Max = value;
                if (value < mapData.Min) mapData.Min = value;

                mapData.Data[x, y] = value;
            }
        }
    }

    private void GenerateDensityMap()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile tile = Tiles[x, y];
                tile.DensityType = GetDensityType(tile);
            }
        }
    }

    private void UpdateBitmasks()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tiles[x, y].UpdateBitmask();
            }
        }
    }

    public float GetDensityValue(Tile tile)
    {
        if (tile == null)
            return int.MaxValue;
        else
            return tile.DensityValue;
    }

    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile tile = new Tile();
                tile.X = x;
                tile.Y = y;

                // Calculates tiles density value
                float densityValue = DensityData.Data[x, y];
                densityValue = (densityValue - DensityData.Min) / (DensityData.Max - DensityData.Min);

                
                tile.DensityValue = densityValue;

                Tiles[x, y] = tile;
            }
        }
    }

    private void UpdateNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = Tiles[x, y];

                t.Top = GetTop(t);
                t.Bottom = GetBottom(t);
                t.Left = GetLeft(t);
                t.Right = GetRight(t);
            }
        }
    }

    private void UpdateBitmasks()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tiles[x, y].UpdateBitmask();
            }
        }
    }

    // Loops through the map to construct its zones
    private void ZoneMap()
    {

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                // Finds and constucts a zone from the first available unzoned tile
                Tile tile = Tiles[x, y];
                if (tile.Zoned) continue;
                Zone zone = new Zone(tile);

                // Adds zone to the map's corresponding list
                if (zone.DensityType == DensityType.Urban)
                    Cities.Add(zone);
                else if (zone.DensityType == DensityType.Suburban)
                    Suburbs.Add(zone);
                else if (zone.DensityType == DensityType.Rural)
                    Farms.Add(zone);
                else
                    Forests.Add(zone);

            }
        }
    }

}
