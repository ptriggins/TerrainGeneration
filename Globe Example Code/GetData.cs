private void GetData(ImplicitModuleBase module, ref MapData mapData)
{
    mapData = new MapData (Width, Height);

    // loop through each x,y point - get height value
    for (var x = 0; x < Width; x++) {
        for (var y = 0; y < Height; y++) {

            // Noise range
            float x1 = 0, x2 = 2;
            float y1 = 0, y2 = 2;
            float dx = x2 - x1;
            float dy = y2 - y1;

            // Sample noise at smaller intervals
            float s = x / (float)Width;
            float t = y / (float)Height;

            // Calculate our 4D coordinates
            float nx = x1 + Mathf.Cos (s*2*Mathf.PI) * dx/(2*Mathf.PI);
            float ny = y1 + Mathf.Cos (t*2*Mathf.PI) * dy/(2*Mathf.PI);
            float nz = x1 + Mathf.Sin (s*2*Mathf.PI) * dx/(2*Mathf.PI);
            float nw = y1 + Mathf.Sin (t*2*Mathf.PI) * dy/(2*Mathf.PI);

            float heightValue = (float)HeightMap.Get (nx, ny, nz, nw);

            // keep track of the max and min values found
            if (heightValue > mapData.Max) mapData.Max = heightValue;
            if (heightValue < mapData.Min) mapData.Min = heightValue;

            mapData.Data[x,y] = heightValue;
        }
    }
}
