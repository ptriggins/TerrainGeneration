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

        Vector3 topleft = new Vector3((Width - 1) / -2f, 0, (Length - 1) / 2f);
        Vector3 StartPosition = topleft + DensityMap.MaxPosition;

        RoadNetwork.Instantiate();
        RoadNetwork.Generate(StartPosition, DensityMap.Tiles);
        RoadNetwork.Draw();
    }
}
