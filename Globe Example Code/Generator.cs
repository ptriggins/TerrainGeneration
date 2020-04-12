using UnityEngine;
using AccidentalNoise;

public class Generator : MonoBehaviour {

    // Adjustable variables for Unity Inspector
    [SerializeField]
    int Width = 256;
    [SerializeField]
    int Height = 256;
    [SerializeField]
    int TerrainOctaves = 6;
    [SerializeField]
    double TerrainFrequency = 1.25;

    // Noise generator module
    ImplicitFractal HeightMap;

    // Height map data
    MapData HeightData;

    // Final Objects
    Tile[,] Tiles;

    // Our texture output (unity component)
    MeshRenderer HeightMapRenderer;

    void Start()
    {
        // Get the mesh we are rendering our output to
        HeightMapRenderer = transform.Find ("HeightTexture").GetComponent<MeshRenderer> ();

        // Initialize the generator
        Initialize ();

        // Build the height map
        GetData (HeightMap, ref HeightData);

        // Build our final objects based on our data
        LoadTiles();

        // Render a texture representation of our map
        HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture (Width, Height, Tiles);
    }

    private void Initialize()
    {
        // Initialize the HeightMap Generator
        HeightMap = new ImplicitFractal (FractalType.MULTI,
                                       BasisType.SIMPLEX,
                                       InterpolationType.QUINTIC,
                                       TerrainOctaves,
                                       TerrainFrequency,
                                       UnityEngine.Random.Range (0, int.MaxValue));
    }

    // Extract data from a noise module
    private void GetData(ImplicitModuleBase module, ref MapData mapData)
    {
        mapData = new MapData (Width, Height);

        // loop through each x,y point - get height value
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                //Sample the noise at smaller intervals
                float x1 = x / (float)Width;
                float y1 = y / (float)Height;

                float value = (float)HeightMap.Get (x1, y1);

                //keep track of the max and min values found
                if (value > mapData.Max) mapData.Max = value;
                if (value < mapData.Min) mapData.Min = value;

                mapData.Data[x,y] = value;
            }
        }
    }

    // Build a Tile array from our data
    private void LoadTiles()
    {
        Tiles = new Tile[Width, Height];

        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Tile t = new Tile();
                t.X = x;
                t.Y = y;

                float value = HeightData.Data[x, y];

                //normalize our value between 0 and 1
                value = (value - HeightData.Min) / (HeightData.Max - HeightData.Min);

                t.HeightValue = value;

                Tiles[x,y] = t;
            }
        }
    }
}
