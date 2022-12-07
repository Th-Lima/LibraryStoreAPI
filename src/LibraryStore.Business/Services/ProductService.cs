using LibraryStore.Business.Interfaces;
using LibraryStore.Business.Models.Validations;
using LibraryStore.Models;

namespace LibraryStore.Business.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUser _user;

        public ProductService(IProductRepository productRepository, INotifier notifier, IUser appUser) : base(notifier)
        {
            _productRepository = productRepository;
            _user = appUser;
        }

        public async Task Add(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product)) 
                return;

            //var userEmail = _user.GetUserEmail();

            await _productRepository.Add(product);
        }

        public async Task Update(Product product)
        {
            if (!ExecuteValidation(new ProductValidation(), product))
                return;

            await _productRepository.Edit(product);
        }

        public async Task Remove(Guid id)
        {
            await _productRepository.Delete(id);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }
    }
}
