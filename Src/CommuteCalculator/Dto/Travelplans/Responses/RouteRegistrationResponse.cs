using CommuteCalculator.Dto.Travelplans.Requests;

namespace CommuteCalculator.Dto.Travelplans.Responses;

public class RouteRegistrationResponse
{
    public NavigationAddress Origin { get; set; } = new NavigationAddress();
    public NavigationAddress Destination { get; set; } = new NavigationAddress();
    public int Distance { get; set; }
}