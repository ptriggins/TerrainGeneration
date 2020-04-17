
// Represents a contigious area where all tiles have the same density
public class Zone
{
    public DensityType DensityType;
    public List<Tile> Tiles;

    public Zone()
    {
        Tiles = new List<Tile>();
    }
}
