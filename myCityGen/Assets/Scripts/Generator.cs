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
    private RoadMap RoadMap;

    public void Generate()
    {
        DensityMap = (DensityMap)FindObjectOfType(typeof(DensityMap));
        RoadMap = (RoadMap)FindObjectOfType(typeof(RoadMap));

        DensityMap.Initialize(Width, Length);
        DensityMap.Generate();
        DensityMap.Draw();

        if (RoadMap.Lines != null)
            RoadMap.Clear();

        RoadMap.Initialize();
        RoadMap.Generate(DensityMap.MaxPosition, DensityMap.MapData);
        RoadMap.Draw();
    }
}
