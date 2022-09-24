using LibraryStore.Models;

namespace LibraryStore.Business.Interfaces
{
    public interface IProviderService : IDisposable
    {
        Task<bool> Add(Provider provider);

        Task<bool> Update(Provider provider);

        Task<bool> Remove(Guid id);

        Task UpdateAddress(Address address);
    }
}
