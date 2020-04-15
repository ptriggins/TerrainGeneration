public static Texture2D CalculateBumpMap(Texture2D source, float strength)
{
    Texture2D result;
    float xLeft, xRight;
    float yUp, yDown;
    float yDelta, xDelta;
    var pixels = new Color[source.width * source.height];
    strength = Mathf.Clamp(strength, 0.0F, 10.0F);
    result = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);

    for (int by = 0; by < result.height; by++)
    {
        for (int bx = 0; bx < result.width; bx++)
        {
            xLeft = source.GetPixel(bx - 1, by).grayscale * strength;
            xRight = source.GetPixel(bx + 1, by).grayscale * strength;
            yUp = source.GetPixel(bx, by - 1).grayscale * strength;
            yDown = source.GetPixel(bx, by + 1).grayscale * strength;
            xDelta = ((xLeft - xRight) + 1) * 0.5f;
            yDelta = ((yUp - yDown) + 1) * 0.5f;

            pixels[bx + by * source.width] = new Color(xDelta, yDelta, 1.0f, yDelta);
        }
    }

    result.SetPixels(pixels);
    result.wrapMode = TextureWrapMode.Clamp;
    result.Apply();
    return result;
}
