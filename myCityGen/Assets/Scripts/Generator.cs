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

    private DensityMap DensityMap;
    private RoadNetwork RoadNetwork;

    public void Generate()
    {
        DensityMap = (DensityMap)FindObjectOfType(typeof(DensityMap));
        RoadNetwork = (RoadNetwork)FindObjectOfType(typeof(RoadNetwork));

        DensityMap.Initialize(Width, Length);
        DensityMap.Generate();
        DensityMap.Draw();

        if (RoadNetwork.Lines != null)
            RoadNetwork.Clear();

        RoadNetwork.Instantiate();
        RoadNetwork.Generate(DensityMap.MapData.MaxPosition, DensityMap.Tiles);
        RoadNetwork.Draw();
    }
}
