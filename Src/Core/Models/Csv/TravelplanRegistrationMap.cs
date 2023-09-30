using CsvHelper.Configuration;

namespace Core.Models.Csv
{
    public class TravelplanRegistrationMap : ClassMap<TravelplanRegistration>
    {
        public TravelplanRegistrationMap()
        {
            Map(x => x.Datum).Index(0).Name("Datum");
            Map(x => x.Postcode_Vertrek).Index(1).Name("Postcode vertrekadres");
            Map(x => x.HuisNr_Vertrek).Index(2).Name("HuisNr");
            Map(x => x.Postcode_Aankomst).Index(3).Name("Postcode aankomstadres");
            Map(x => x.HuisNr_Aankomst).Index(4).Name("HuisNr");
            Map(x => x.Retour).Index(5).Name("Retour");
            Map(x => x.Omleiding).Index(6).Name("Omleiding");
            Map(x => x.SubTotaal_Km).Index(7).Name("Subtotaal km's");
        }
    }
}
