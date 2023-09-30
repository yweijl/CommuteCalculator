namespace Core.Models.Travelplans;

public class UserTravelplan
{
    public Guid UserId { get; set; }
    public List<Travelplan> Travelplans { get; set; } = new List<Travelplan>();
}

