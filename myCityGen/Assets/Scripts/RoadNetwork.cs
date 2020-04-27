using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    /*
    public void GenerateRoads()
    {
        Queue<Road> potential = new Queue<Road>();
        List<Road> accepted = new List<Road>();
        potential.Enqueue(new Road());

        int i = 0;
        int limit = 10000;

        while (i < limit || potential.Count != 0)
        {
            Road current = potential.Dequeue();


            //bool segmentAccepted = CheckLocalConstraints(ref currentSegment);

            if (segmentAccepted)
            {
                accepted.Add(current);

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
