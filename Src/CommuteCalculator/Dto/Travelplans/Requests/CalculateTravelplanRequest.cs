namespace CommuteCalculator.Dto.Travelplans.Requests;

public class CalculateTravelplanRequest
{
    public List<WayPointsRequest> Waypoints { get; set; } = new();
}