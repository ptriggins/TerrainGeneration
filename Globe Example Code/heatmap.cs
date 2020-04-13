// Height Map Colors
private static Color Coldest = new Color(0, 1, 1, 1);
private static Color Colder = new Color(170/255f, 1, 1, 1);
private static Color Cold = new Color(0, 229/255f, 133/255f, 1);
private static Color Warm = new Color(1, 1, 100/255f, 1);
private static Color Warmer = new Color(1, 100/255f, 0, 1);
private static Color Warmest = new Color(241/255f, 12/255f, 0, 1);

public static Texture2D GetHeatMapTexture(int width, int height, Tile[,] tiles)
{
    var texture = new Texture2D(width, height);
    var pixels = new Color[width * height];

    for (var x = 0; x < width; x++)
    {
        for (var y = 0; y < height; y++)
        {
            switch (tiles[x,y].HeatType)
            {
            case HeatType.Coldest:
                pixels[x + y * width] = Coldest;
                break;
            case HeatType.Colder:
                pixels[x + y * width] = Colder;
                break;
            case HeatType.Cold:
                pixels[x + y * width] = Cold;
                break;
            case HeatType.Warm:
                pixels[x + y * width] = Warm;
                break;
            case HeatType.Warmer:
                pixels[x + y * width] = Warmer;
                break;
            case HeatType.Warmest:
                pixels[x + y * width] = Warmest;
                break;
            }

            //darken the color if a edge tile
            if (tiles[x,y].Bitmask != 15)
                pixels[x + y * width] = Color.Lerp(pixels[x + y * width], Color.black, 0.4f);
        }
    }

    texture.SetPixels(pixels);
    texture.wrapMode = TextureWrapMode.Clamp;
    texture.Apply();
    return texture;
}

//-----------------------------------------------------------------------------------------------------

int HeatOctaves = 4;
double HeatFrequency = 3.0;

private void Initialize()
{
    // Initialize the Heat map
    ImplicitGradient gradient  = new ImplicitGradient (1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1);
    ImplicitFractal heatFractal = new ImplicitFractal(FractalType.MULTI,
                                                      BasisType.SIMPLEX,
                                                      InterpolationType.QUINTIC,
                                                      HeatOctaves,
                                                      HeatFrequency,
                                                      Seed);

        // Combine the gradient with our heat fractal
    HeatMap = new ImplicitCombiner (CombinerType.MULTIPLY);
    HeatMap.AddSource (gradient);
    HeatMap.AddSource (heatFractal);
}
