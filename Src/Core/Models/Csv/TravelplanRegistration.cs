using Core.Models.Travelplans;

namespace Core.Models.Csv;

public class TravelplanRegistration
{
    public string Datum { get; }
    public string Postcode_Vertrek { get; }
    public string HuisNr_Vertrek { get; }
    public string Postcode_Aankomst { get; }
    public string HuisNr_Aankomst { get; }
    public string Retour => "Nee";
    public string Omleiding => "Nee";
    public string SubTotaal_Km { get; }


    public TravelplanRegistration(RouteRegistration registration, DateTime registrationDate)
    {
        this.Datum = registrationDate.ToShortDateString();
        this.Postcode_Vertrek = registration.Origin.PostalCode;
        this.HuisNr_Vertrek = $"{registration.Origin.HouseNumber} {registration.Origin.HouseNumberAddition}";
        this.Postcode_Aankomst = registration.Destination.PostalCode;
        this.HuisNr_Aankomst = $"{registration.Destination.HouseNumber} {registration.Destination.HouseNumberAddition}";

        double distance = registration.Distance / 1000.00;
        this.SubTotaal_Km = $"{distance.ToString("0.0")}";
    }
}