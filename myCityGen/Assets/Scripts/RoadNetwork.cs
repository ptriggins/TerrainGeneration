using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    public Vector2 Start;
    public Vector2 Destinations;
    public DensityMap DensityMap;

    public RoadNetwork(DensityMap densityMap)
    {
        DensityMap = densityMap;
    }

    public void GenerateRoads()
    {
        Queue<Road> candidates = new Queue<Road>();
        List<Road> accepted = new List<Road>();
        potential.Enqueue(new Road());

        int i = 0;
        int limit = 10000;

        while (i < limit && candidates.Count > 0)
        {
            Road current = candidates.Dequeue();
            accepted.Add(current);

            GetNextRoads
            
        }
        i++;
    }

    /*
    public Road CheckCrossings(Road road)
    {

    }
    */

    public Road GetNextRoads(Road current, Queue<Road> queue)
    {
        Vector2 start = current.End;

        MapData mapdata = DensityMap.MapData;

        Vector2 direction = (current.Start - current.End).normalized;
        float angle = Vector2.Angle(Vector2.zero, direction);

        Vector2 straight = start + direction * 4;
        float valS = mapdata.GetVal((int)straight.x, (int)straight.y);

        Quaternion lRotation = Quaternion.Euler(0, angle - 20, 0);
        Vector2 slightLeft = start + (Vector2)(lRotation * Vector2.right) * 4;
        float valL = mapdata.GetVal((int)slightLeft.x, (int)slightLeft.y);

        Quaternion rRotation = Quaternion.Euler(0, angle + 20, 0);
        Vector2 slightRight = start + (Vector2)(lRotation * Vector2.right) * 4;
        float valR = mapdata.GetVal((int)slightRight.x, (int)slightRight.y);

        if (valS > valL && valS > valR)
            queue.Enqueue();
        else if (valL > valS && valL > valR)
            queue.Enqueue();
        else
            queue.Enqueue();

        float val = mapdata.GetVal((int)start.x, (int)start.y);
        CityType Type = DensityMap.GetType(val);

        if (Random.Range(0, 1) < Type.Percentile / 2)
        {
           Quaternion lPerp = Quaternion.Euler(0, angle - 90, 0);
           Vector2 leftBranch = start + (Vector2)(lPerp * Vector2.right) * 4;
        }
        



    }

}
