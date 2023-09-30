using Core.Models;

namespace Core.Interfaces;

public interface IContactService
{
    Task<Contact> SaveContactAsync(Contact contact);
    Task<Contact?> GetByIdAsync(Guid id);
    Task<List<Contact>> ListByUserIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid contactId);
}