using UnityEngine;
using System.Collections.Generic;
using AccidentalNoise;

public class Generator : MonoBehaviour
{

    [Header("Size")]
    [SerializeField]
    public int Width = 512;
    [SerializeField]
    public int Length = 512;

    private ImplicitFractal NoiseMap;
    public DensityMap DensityMap;
    private RoadNetwork RoadNetwork;
    private Display Display;

    void Instantiate()
    {
        DensityMap = new DensityMap(Width, Length);
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

        RoadNetwork = (RoadNetwork)FindObjectOfType(typeof(RoadNetwork));
        RoadNetwork.DensityMap = DensityMap;
        RoadNetwork.GenerateRoads(DensityMap.topleft + DensityMap.MapData.MaxCoords);

    }

 
    

}
