class RoadSegment
{
    RoadData data;
}

function BuildCity()
{
    Queue<RoadSegment> evaluationQueue = new Queue<RoadSegment>();

    List<RoadSegment> acceptedSegments = new List<RoadSegment>();

    // Add an initial road segment to branch out from
    evaluationQueue.Enqueue(new RoadSegment());

    int i = 0;
    int segmentLimit = 10000;

    // Loop until we reach a limit or can't build any more roads
    while (i < segmentLimit || evaluationQueue.Count != 0)
    {
        // Take the oldest segment in the queue
        RoadSegment currentSegment = evaluationQueue.Dequeue();

        // Check if it's acceptable and modify it as needed
        bool segmentAccepted = CheckLocalConstraints(ref currentSegment);

        if (segmentAccepted)
        {
            // Add this segment to the actual road network
            acceptedSegments.Add(currentSegment);

            // Propose new road segments branching from this one
            foreach (RoadSegment possibleSegment in GeneratePossibleSegments(currentSegment))
            {
                evaluationQueue.Enqueue(possibleSegment);
            }
        }

        i++;
    }
}

function CheckLocalConstraints(ref RoadSegment segment)
{
    // This function checks if the proposed road segment is valid ('local constraints' - not underwater or passing through a house, etc.) and if it is, applies any last minute modification like connecting it to nearby road segments that have already been placed
}

function GeneratePossibleSegments(RoadSegment segment)
{
    // This is where you generate new proposed road segments based on 'global constraints' - height, nearby roads, etc.
}
