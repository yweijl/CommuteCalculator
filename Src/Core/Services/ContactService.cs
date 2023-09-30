using Core.Interfaces;
using Core.Models;

namespace Core.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }

    public Task<Contact> SaveContactAsync(Contact contact)
    {
        return _repository.SaveContactAsync(contact);
    }

    public Task<Contact?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<List<Contact>> ListByUserIdAsync(Guid id)
    {
        return _repository.GetUserContactsAsync(id);
    }

    public Task<bool> DeleteByIdAsync(Guid contactId)
    {
        return _repository.DeleteByIdAsync(contactId);
    }
}