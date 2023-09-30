using Core.Models;

namespace Core.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> SaveContactAsync(Contact addContactDto);
        Task<Contact?> GetByIdAsync(Guid id);
        Task<List<Contact>> GetUserContactsAsync(Guid id);
        Task<bool> DeleteByIdAsync(Guid contactId);
    }
}