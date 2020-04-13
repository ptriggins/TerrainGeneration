public static Texture2D GetMoistureMapTexture(int width, int height, Tile[,] tiles)
{
    var texture = new Texture2D(width, height);
    var pixels = new Color[width * height];

    for (var x = 0; x < width; x++)
    {
        for (var y = 0; y < height; y++)
        {
            Tile t = tiles[x,y];

            if (t.MoistureType == MoistureType.Dryest)
                pixels[x + y * width] = Dryest;
            else if (t.MoistureType == MoistureType.Dryer)
                pixels[x + y * width] = Dryer;
            else if (t.MoistureType == MoistureType.Dry)
                pixels[x + y * width] = Dry;
            else if (t.MoistureType == MoistureType.Wet)
                pixels[x + y * width] = Wet;
            else if (t.MoistureType == MoistureType.Wetter)
                pixels[x + y * width] = Wetter;
            else
                pixels[x + y * width] = Wettest;
        }
    }

    texture.SetPixels(pixels);
    texture.wrapMode = TextureWrapMode.Clamp;
    texture.Apply();
    return texture;
}
