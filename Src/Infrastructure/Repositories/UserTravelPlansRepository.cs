using AutoMapper;
using Core.Interfaces;
using Core.Models.Travelplans;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class UserTravelplansRepository : GenericRepository, IUserTravelplansRepository
{
    public UserTravelplansRepository(CommuteCalculatorContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<bool> DeleteTravelplanByIdAsync(Guid id, Guid userId)
    {
        var userTravelplans = await this.SingleAsync<UserTravelplanEntity>(x => x.UserId == userId, true);
        var plan = userTravelplans.Travelplans.Single(x => x.Id == id);
        userTravelplans.Travelplans.Remove(plan);

        await this.SaveChangesAsync();
        return true;
    }

    public async Task<UserTravelplan> GetByUserIdAsync(Guid userId)
    {
        var userTravelplan = await this.SingleOrDefaultAsync<UserTravelplanEntity>(userId);
        return _mapper.Map<UserTravelplan>(userTravelplan) ?? new UserTravelplan { UserId = userId };
    }

    public async Task<List<Travelplan>> GetTravelplansByMonthAsync(int monthNumber, Guid UserId)
    {
        var userTravelplan = await GetByUserIdAsync(UserId);
        var travelplans = userTravelplan.Travelplans.Where(x => x.RegistrationDate.Month == monthNumber).ToList();
        return _mapper.Map<List<Travelplan>>(travelplans);
    }

    public async Task<bool> SaveAsync(Guid userId, Travelplan travelplan)
    {
        var travelplanEntity = _mapper.Map<TravelplanEntity>(travelplan);
        var userTravelPlan = await this.SingleOrDefaultAsync<UserTravelplanEntity>(userId, true);
        if (userTravelPlan == null)
        {
            await this.AddAsync(new UserTravelplanEntity
            {
                UserId = userId,
                Travelplans = new() { travelplanEntity }
            });
        } else
        {
            userTravelPlan.Travelplans.Add(travelplanEntity);
            await this.SaveChangesAsync();
        }
        return true;
    }
}