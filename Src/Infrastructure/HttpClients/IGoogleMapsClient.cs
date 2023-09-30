using Infrastructure.Entities;

namespace Infrastructure.HttpClients;
public interface IGoogleMapsClient
{
    Task<CalculatedRoutesEntity> GetCalculatedRoutesAsync(RouteMatrix route);
}