using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    [Header("Road Size")]
    [SerializeField]
    public float Length = 4;
    [SerializeField]
    public float Thickness = 4;

    public Vector3 TopLeft;
    public float[,] Values;
    CityType[] Types;

    public RoadNetwork(float[,] values, CityType[] types)
    {
        Values = values;
        Types = types;
    }

    public void Generate(Vector3 start)
    {
        Queue<Road> candidates = new Queue<Road>();
        candidates.Enqueue(new Road(start, start + Random.insideUnitCircle.normalized * 4));

        int i = 0;
        int limit = 10;

        while (i < limit && candidates.Count > 0)
        {
            Road current = candidates.Dequeue();
            current.DrawRoad();
            //GetNextRoads(current, candidateQueue);
            
        }
        i++;
    }

    public void GetCandidates(Road current, Queue<Road> queue)
    {
        Vector3 start = current.End;

        Vector3[] varients = new Vector3[3];
        varients[0] = current.GetExtension(-20);
        varients[1] = current.GetExtension(0);
        varients[2] = current.GetExtension(20);

        float[] values = new float[3];
        values[0] = Values[(int)varients[0].x, (int)varients[0].z];
        values[1] = Values[(int)varients[1].x, (int)varients[1].z];
        values[2] = Values[(int)varients[2].x, (int)varients[2].z];

        for (int i = 0; i < 3; i++)
        {
            if (values[i] == values.Max())
                queue.Enqueue(new Road(start, varients[i]));
        }

        float val = Values[(int)start.x, (int)start.z];
        CityType type = DensityMap.GetType(val);

        if (Random.Range(0, 1) < Type.Percentile / 2)
        {
            Quaternion lPerp = Quaternion.Euler(0, angle - 90, 0);
            Vector2 leftBranch = start + (Vector2)(lPerp * Vector2.right) * 4;
            queue.Enqueue(new Road(start, leftBranch));
        }

        if (Random.Range(0, 1) < Type.Percentile / 2)
        {
            Quaternion rPerp = Quaternion.Euler(0, angle - 90, 0);
            Vector2 rightBranch = start + (Vector2)(rPerp * Vector2.right) * 4;
            queue.Enqueue(new Road(start, rightBranch));
        }

    }

}
