﻿using UnityEngine;
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
    public List<DensityType> DensityTypes;

    private ImplicitFractal NoiseMap;
    public DensityMap DensityMap;
    private Display Display;

    void Instantiate()
    {
        NoiseMap = new ImplicitFractal(FractalType.MULTI, BasisType.SIMPLEX, InterpolationType.QUINTIC,
            Octaves, Frequency, Random.Range(0, int.MaxValue));
        DensityMap = new DensityMap(Width, Length, NoiseMap, DensityTypes);
    }

    public void Generate()
    {   
        Instantiate();

        Display = (Display)FindObjectOfType(typeof(Display));
        Display.Mesh = new Mesh();
        Display.MeshFilter.mesh = Display.Mesh;

        DensityMap.SetMap();
        DensityMap.MeshData.SetData();
        DensityMap.Texture = TextureGenerator.GetDensityTexture(Width, Length, DensityMap.Colors);
        Display.SetMesh(DensityMap.MeshData);
        Display.Draw(DensityMap.Texture);
    }

 
    

}
