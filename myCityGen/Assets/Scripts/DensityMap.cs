using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    public int X;
    public int Y;
    public Coords(int x, int y) {X = x; Y = y;}
}

public class DensityMap
{
    public Tile[,] Tiles;
    public List<List<Zone>> Zones;
    List<DensityType> DensityTypes;
    int Width, Length, NumTypes;

    public DensityMap(int width, int length, List<DensityType> densityTypes)
    {
        Width = width; Length = length;
        Tiles = new Tile[width, length];
        Zones = new List<List<Zone>>();

        NumTypes = densityTypes.Count;
        DensityTypes = densityTypes;
        NumTypes = densityTypes.Count;
    }

    public void SetTiles(MapData mapdata)
    {
        for (int z = 0; z < Length; z++)
        {
            for (int x = 0; x < Length; x++)
            {
                if (Tiles[x, z] == null)
                {
                    Zone zone = new Zone();
                }

                DensityType type = GetDensityType(x, z, mapdata);
                Stack<Coords> stack = new Stack<Tile>();
                stack.Push(new Tile(type));


                DensityType right = GetDensityType(x + 1, z, mapdata);
                DensityType bottom = GetDensityType(x, z - 1, mapdata);
                DensityType leftType = GetDensityType(x - 1, z, mapdata);
                if
            }
        }
    }

    public void SetNeighbors()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Length; y++)
            {
                Tile tile = Tiles[x, y];

                if (y != 0)
                    tile.TopNeighbor = Tiles[x, y - 1];
                if (y != Length - 1)
                    tile.BottomNeighbor = Tiles[x, y + 1];
                if (x != 0)
                    tile.LeftNeighbor = Tiles[x - 1, y];
                if (x != Width - 1)
                    tile.RightNeighbor = Tiles[x + 1, y];
            }
        }
    }

    private void SetBitmasks()
    {
        for (var x = 0; x < Tiles.GetLength(0); x++)
        {
            for (var y = 0; y < Tiles.GetLength(1); y++)
            {
                Tiles[x, y].SetBitmask();
            }
        }
    }

    private void SetZones()
    {
        for (int i = 0; i < DensityTypes.Count; i++)
        {
            Zones[i] = new List<Zone>();
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Length; y++)
            {

                // Constructs new zone from first available unzoned tile
                Tile tile = Tiles[x, y];
                if (tile.Zoned) continue;
                Zone zone = new Zone(tile);

                for (int i = 0; i < DensityTypes.Length; i++)
                {
                    if (zone.DensityType.Name == DensityTypes[i].Name)
                        Zones[i].Add(zone);
                }

            }
        }
    }

    private DensityType GetDensityType(int x, int z, MapData mapdata)
    {
        float value = Normalize(mapdata.Values[x, z], mapdata.Max, mapdata.Min);
        for (int i = 0; i < NumTypes; i++)
        {
            if (value < DensityTypes[i].Percentile)
            {
                return DensityTypes[i];
            }
        }
        return DensityTypes[NumTypes - 1];
    }

    private float Normalize(float val, float max, float min)
    {
        return (val - min) / (max - min);
    }

}
