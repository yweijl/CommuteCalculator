using Core.Interfaces;
using Core.Models.Csv;
using Core.Models.Travelplans;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Core.Services;

public class CsvService : ICsvService
{
    public (string, byte[]) CreateTravelplanRegistration(List<Travelplan> travelplans)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvWriter(writer, config);
        csv.WriteHeader<TravelplanRegistration>();
        csv.NextRecord();
        foreach (var travelplan in travelplans.OrderByDescending(x => x.RegistrationDate))
        {
            foreach (var route in travelplan.Routes)
            {
                var routeCsv = new TravelplanRegistration(route, travelplan.RegistrationDate);

                csv.WriteRecord(routeCsv);
                csv.NextRecord();
                writer.Flush();
            }
        }

        return (travelplans.First().RegistrationDate.ToShortDateString(), stream.ToArray());
    }
}
