using UnityEngine;
using System.Collections.Generic;

public enum DensityType
{
    Urban,
    Suburbarban,
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

    protected abstract Tile GetTop(Tile tile);
    protected abstract Tile GetBottom(Tile tile);
    protected abstract Tile GetLeft(Tile tile);
    protected abstract Tile GetRight(Tile tile);

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
        FloodFill();

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

    private void UpdateBitmask()
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

    // Colors tiles of the same density type all in one batch
    private void FloodFill()
    {
        // Works through a stack of tiles rather than recursively coloring
        Stack<Tile> stack = new Stack<Tile>();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                // Finds a tile that has not already been colored to start a new group
                Tile tile = Tiles[x, y];
                if (tile.FloodFilled) continue;

                TileGroup group = new TileGroup();
                group.DensityType = tile.DensityType;
                stack.Push(tile);

                while (stack.Count > 0)
                {
                    FloodFill(stack.Pop(), ref group, ref stack);

                    if (group.Tiles.Count > 0)
                        Lands.Add(group);
                }

                while (stack.Count > 0)
                    {
                        FloodFill(stack.Pop(), ref group, ref stack);
                    }

                    if (group.Tiles.Count > 0)
                        Lands.Add(group);
                }
            }
        }
    }

    private void FloodFill(Tile tile, ref TileGroup tiles, ref Stack<Tile> stack)
    {
        // Validate
        if (tile == null)
            return;
        if (tile.FloodFilled)
            return;
        if (tiles.Type == TileGroupType.Land && !tile.Collidable)
            return;
        if (tiles.Type == TileGroupType.Water && tile.Collidable)
            return;

        // Add to TileGroup
        tiles.Tiles.Add(tile);
        tile.FloodFilled = true;

        // floodfill into neighbors
        Tile t = GetTop(tile);
        if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetBottom(tile);
        if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetLeft(tile);
        if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
        t = GetRight(tile);
        if (t != null && !t.FloodFilled && tile.Collidable == t.Collidable)
            stack.Push(t);
    }

}
