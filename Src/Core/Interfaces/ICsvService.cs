using Core.Models.Travelplans;

namespace Core.Interfaces;

public interface ICsvService
{
    (string, byte[]) CreateTravelplanRegistration(List<Travelplan> travelplans);
}
