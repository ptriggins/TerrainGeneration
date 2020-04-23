using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoadType
{
    public string Name;
    public string Color;
}

public class RoadNetwork : MonoBehaviour
{
    /*
    public void GenerateRoads()
    {
        Queue<Road> potentialRoads = new Queue<Road>();
        List<Road> acceptedRoads = new List<Road>();
        potentialRoads.Enqueue(new Road());

        int i = 0;
        int limit = 10000;

        while (i < limit || potentialRoads.Count != 0)
        {
            Road currentRoad = potentialRoads.Dequeue();

            // Check if it's acceptable and modify it as needed
            bool segmentAccepted = CheckLocalConstraints(ref currentSegment);

            if (segmentAccepted)
            {
                // Add this segment to the actual road network
                acceptedSegments.Add(currentSegment);

                // Propose new road segments branching from this one
                foreach (Road possibleSegment in GeneratePossibleSegments(currentSegment))
                {
                    evaluationQueue.Enqueue(possibleSegment);
                }
            }

            i++;
        }
    }

    /*
    function CheckLocalConstraints(ref RoadSegment segment)
    {
        
    }

    public Road GetPotentialRoad(RoadSegment segment)
    {
        // This is where you generate new proposed road segments based on 'global constraints' - height, nearby roads, etc.
    }
    */

}
