using UnityEngine;
using System.Collections.Generic;
using AccidentalNoise;

[System.Serializable]
public struct CityType
{
    public string Name;
    public float Percentile;
    public Color Color;
}

public class TerrainGenerator : MonoBehaviour
{

    [Header("Map Size")]
    [SerializeField]
    public int Width = 512;
    [SerializeField]
    public int Length = 512;

    [Header("Noise")]
    [SerializeField]
    public int Octaves = 6;
    [SerializeField]
    public double Frequency = 1.25;

    [Header("Density Types")]
    [SerializeField]
    public List<CityType> CityTypes;

    private ImplicitFractal NoiseMap;
    public DensityMap DensityMap;
    public RoadNetwork RoadNewtork;
    private Display Display;

    void Instantiate()
    {
        NoiseMap = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC,
            Octaves, Frequency, Random.Range(0, int.MaxValue));
        DensityMap = new DensityMap(Width, Length, CityTypes);
    }

    public void Generate()
    {   
        Instantiate();

        Display = (Display)FindObjectOfType(typeof(Display));
        Display.Mesh = new Mesh();
        Display.MeshFilter.mesh = Display.Mesh;

        DensityMap.MapData.SetData(NoiseMap);
        DensityMap.SetMap();
        DensityMap.MeshData.SetData(DensityMap.MapData.Values);
        DensityMap.Texture = TextureGenerator.GetDensityTexture(Width, Length, DensityMap.Colors);
        Display.SetMesh(DensityMap.MeshData);
        Display.Draw(DensityMap.Texture);
    }

 
    

}
