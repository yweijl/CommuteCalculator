using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class AuthenticationRepository : GenericRepository, IAuthenticationRepository
{

    public AuthenticationRepository(CommuteCalculatorContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var userEntity = await this.SingleOrDefaultAsync<UserEntity>(x => x.Email.ToLower() == email.ToLower());
        return _mapper.Map<User?>(userEntity);
    }

    public Task<bool> UserExistsAsync(string email)
    {
        return this.AnyAsync<UserEntity>(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<User> SaveUserAsync(User user)
    {
       var newEntity = _mapper.Map<UserEntity>(user);
       var savedEntity = await this.AddAsync(newEntity);
       return _mapper.Map<User>(savedEntity);
    }
}