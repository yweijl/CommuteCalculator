using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class ContactRepository : GenericRepository, IContactRepository
{
    public ContactRepository(CommuteCalculatorContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public Task<bool> DeleteByIdAsync(Guid contactId)
    {
        return this.DeleteAsync<ContactEntity>(contactId);
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        var contact = await this.SingleAsync<ContactEntity>(id);
        return _mapper.Map<Contact>(contact);
    }

    public async Task<List<Contact>> GetUserContactsAsync(Guid id)
    {
        var contacts = await this.ListAsync<ContactEntity>(x => x.UserId == id);
        return _mapper.Map<List<Contact>>(contacts) ?? new();
    }

    public async Task<Contact> SaveContactAsync(Contact contact)
    {
        var newEntity = _mapper.Map<ContactEntity>(contact);
        var AddedEntity = await this.AddAsync(newEntity);
        return _mapper.Map<Contact>(contact);
    }
}

