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
    public int Width = 512;     // Size in x
    [SerializeField]
    public int Length = 512;    // Size in z

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
    public List<DensityType> DensityTypes;

    private ImplicitFractal NoiseMap;
    private mapdata MapData;
    private MeshData MeshData;

    public DensityMap DensityMap;

    private Display Display;

    //public AnimationCurve heightCurve;

    void Start()
    {
        Display = (Display)FindObjectOfType(typeof(Display));
    }

    void Instantiate()
    {
        Tiles = new Tile[Width, Length];
    }

    void Initialize()
    {

    }

    public void Generate()
    {

        NoiseMap = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC,
            Octaves, Frequency, Random.Range(0, int.MaxValue));
        MapData = new mapdata(Width, Length);
        MeshData = new MeshData(Width, Length);

        SetTiles();
        SetNeighbors();
        SetBitmasks();
        ZoneMap();

    }

    private void ZoneMap()
    {
        Zones = new List<Zone>[DensityTypes.Length];
        for (int i = 0; i < DensityTypes.Length; i++)
        {
            Zones[i] = new List<Zone>();
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
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
    }

}
