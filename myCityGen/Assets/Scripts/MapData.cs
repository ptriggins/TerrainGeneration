using AccidentalNoise;

// Stores data sampled from a noise map
public class MapData
{
    public float[,] Values;
    public float Min;
    public float Max;

    // Constructs mapdata from a noise map
    public MapData(int width, int height, ImplicitModuleBase module)
    {
        Values = new float[width, height];
        Min = float.MinValue;
        Max = float.MaxValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Interval of sampling
                float x1 = x / (float)width;
                float y1 = y / (float)height;

                float value = (float)module.Get(x1, y1);

                if (value > Max) Max = value;
                if (value < Min) Min = value;

                Values[x, y] = value;
            }
        }
    }
}