
// Stores data sampled from a noise map
public class MapData
{
    public float[,] HeightMap;
    public float Min { get; set; }
    public float Max { get; set; }

    public MapData(int width, int height)
    {
        HeightMap = new float[width, height];
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}