//biome map
private static Color Ice = Color.white;
private static Color Desert = new Color(238/255f, 218/255f, 130/255f, 1);
private static Color Savanna = new Color(177/255f, 209/255f, 110/255f, 1);
private static Color TropicalRainforest = new Color(66/255f, 123/255f, 25/255f, 1);
private static Color Tundra = new Color(96/255f, 131/255f, 112/255f, 1);
private static Color TemperateRainforest = new Color(29/255f, 73/255f, 40/255f, 1);
private static Color Grassland = new Color(164/255f, 225/255f, 99/255f, 1);
private static Color SeasonalForest = new Color(73/255f, 100/255f, 35/255f, 1);
private static Color BorealForest = new Color(95/255f, 115/255f, 62/255f, 1);
private static Color Woodland = new Color(139/255f, 175/255f, 90/255f, 1);


    public static Texture2D GetBiomeMapTexture(int width, int height, Tile[,] tiles, float coldest, float colder, float cold)
{
    var texture = new Texture2D(width, height);
    var pixels = new Color[width * height];

    for (var x = 0; x < width; x++)
    {
        for (var y = 0; y < height; y++)
        {
            BiomeType value = tiles[x, y].BiomeType;

            switch(value){
            case BiomeType.Ice:
                pixels[x + y * width] = Ice;
                break;
            case BiomeType.BorealForest:
                pixels[x + y * width] = BorealForest;
                break;
            case BiomeType.Desert:
                pixels[x + y * width] = Desert;
                break;
            case BiomeType.Grassland:
                pixels[x + y * width] = Grassland;
                break;
            case BiomeType.SeasonalForest:
                pixels[x + y * width] = SeasonalForest;
                break;
            case BiomeType.Tundra:
                pixels[x + y * width] = Tundra;
                break;
            case BiomeType.Savanna:
                pixels[x + y * width] = Savanna;
                break;
            case BiomeType.TemperateRainforest:
                pixels[x + y * width] = TemperateRainforest;
                break;
            case BiomeType.TropicalRainforest:
                pixels[x + y * width] = TropicalRainforest;
                break;
            case BiomeType.Woodland:
                pixels[x + y * width] = Woodland;
                break;
            }

            // Water tiles
            if (tiles[x,y].HeightType == HeightType.DeepWater) {
                pixels[x + y * width] = DeepColor;
            }
            else if (tiles[x,y].HeightType == HeightType.ShallowWater) {
                pixels[x + y * width] = ShallowColor;
            }

            // draw rivers
            if (tiles[x,y].HeightType == HeightType.River)
            {
                float heatValue = tiles[x,y].HeatValue;

                if (tiles[x,y].HeatType == HeatType.Coldest)
                    pixels[x + y * width] = Color.Lerp (IceWater, ColdWater, (heatValue) / (coldest));
                else if (tiles[x,y].HeatType == HeatType.Colder)
                    pixels[x + y * width] = Color.Lerp (ColdWater, RiverWater, (heatValue - coldest) / (colder - coldest));
                else if (tiles[x,y].HeatType == HeatType.Cold)
                    pixels[x + y * width] = Color.Lerp (RiverWater, ShallowColor, (heatValue - colder) / (cold - colder));
                else
                    pixels[x + y * width] = ShallowColor;
            }


            // add a outline
            if (tiles[x,y].HeightType >= HeightType.Shore && tiles[x,y].HeightType != HeightType.River)
            {
                if (tiles[x,y].BiomeBitmask != 15)
                    pixels[x + y * width] = Color.Lerp (pixels[x + y * width], Color.black, 0.35f);
            }
        }
    }

    texture.SetPixels(pixels);
    texture.wrapMode = TextureWrapMode.Clamp;
    texture.Apply();
    return texture;
}
