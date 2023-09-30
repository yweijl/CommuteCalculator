namespace CommuteCalculator.Dto.Travelplans.Requests;

public class RouteRegistrationRequest
{
    public NavigationAddress Origin { get; set; } = new NavigationAddress();
    public NavigationAddress Destination { get; set; } = new NavigationAddress();   
    public int Distance { get; set; }
}