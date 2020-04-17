using UnityEngine;
using System.Collections.Generic;

public enum DensityType
{
    Urban,
    Suburban,
    Rural,
    Wilderness
}

public abstract class Generator : MonoBehaviour
{

    // Seed used for noise generation
    protected int Seed;

    // Sliders

    [Header("Map Size")]
    [SerializeField]
    protected int Width = 512;
    [SerializeField]
    protected int Height = 512;

    [Header("Noise")]
    [SerializeField]
    protected int Octaves = 6;              // Level of complexity
    [SerializeField]
    protected double Frequency = 1.25;      // Interval between samples

    [Header("Density Level")]
    [SerializeField]
    protected float Urban = 0.2f;
    [SerializeField]
    protected float Suburban = 0.4f;
    [SerializeField]
    protected float Rural = 0.5f;
    [SerializeField]
    protected float Wilderness = 0.7f;


    // Stores a map of noise samples
    protected MapData densityData;

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

    protected abstract void Initialize();
    protected abstract void GetData();

    // Creates copy of the generator object, sets seed and renderer object
    protected virtual void Instantiate()
    {
        Seed = UnityEngine.Random.Range(0, int.MaxValue);
        DensityRenderer = transform.Find("DensityTexture").GetComponent<MeshRenderer>();
        Initialize();
    }

    protected virtual void Generate()
    {
        GetData();
        LoadTiles();
        UpdateNeighbors();

        UpdateBitmasks();
        ZoneMap();

        GenerateDensityMap();

        DensityRenderer.materials[0].mainTexture = DensityTextureGenerator.GetDensityTexture(Width, Height, Tiles);
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

    // Builds an array of tiles
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
                float densityValue = densityData.Data[x, y];
                densityValue = (densityValue - densityData.Min) / (densityData.Max - densityData.Min);

                // Sets tile's density type according to its density value
                if (densityValue < Wilderness)
                {
                    tile.DensityType = DensityType.Wilderness;
                }
                else if (densityValue < Rural)
                {
                    tile.DensityType = DensityType.Rural;
                }
                else if (densityValue < Suburban)
                {
                    tile.DensityType = DensityType.Suburban;
                }
                else
                {
                    tile.DensityType = DensityType.Urban;
                }
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
