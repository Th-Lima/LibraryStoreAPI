using LibraryStore.Business.Interfaces;
using LibraryStore.Business.Models.Validations;
using LibraryStore.Models;

namespace LibraryStore.Business.Services
{
    public class ProviderService : BaseService, IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;

        public ProviderService(IProviderRepository providerRepository, IAddressRepository addressRepository, INotifier notifier) : base(notifier)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider) || !ExecuteValidation(new AddressValidation(), provider.Address)) 
                 return false;

            if(_providerRepository.Search(p => p.Document == provider.Document).Result.Any())
            {
                Notification("Já existe um fornecedor com este documento informado!");
                return false;
            }

            await _providerRepository.Add(provider);
            return true;
        }

        public async Task<bool> Update(Provider provider)
        {
            if (!ExecuteValidation(new ProviderValidation(), provider))
                return false;

            if(_providerRepository.Search(p => p.Document == provider.Document && p.Id != provider.Id).Result.Any())
            {
                Notification("Já existe um fornecedor com este documento informado!");
                return false;
            }

            await _providerRepository.Edit(provider);
            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address))
                return;

            await _addressRepository.Edit(address);
        }

        public async Task<bool> Remove(Guid id)
        {
            if (_providerRepository.GetProviderProductsAddress(id).Result.Products.Any())
            {
                Notification("O fornecedor possui produtos cadastrados");
                return false;
            }

            var address = await _addressRepository.GetAddressByProvider(id);

            if (address != null)
                await _addressRepository.Delete(address.Id);

            await _providerRepository.Delete(id);
            return true;
        }

        public void Dispose()
        {
            _providerRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
