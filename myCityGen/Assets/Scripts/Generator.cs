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

        Vector3 topleft = new Vector3((Width - 1) / -2f, 0, (Length - 1) / 2f);
        RoadNetwork.Initialize(DensityMap.Tiles, topleft);
        RoadNetwork.Generate();
        RoadNetwork.Draw();
    }
}
