using AutoMapper;
using LibraryStore.Api.Dtos;
using LibraryStore.Business.Interfaces;
using LibraryStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductsController(INotifier notifier, IProductRepository productRepository, IProductService productService, IImageService imageService, IMapper mapper) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetProductsProviders());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var productDto = await GetProduct(id);

            if (productDto == null)
                return NotFound();

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Post(ProductDto productDto)
        {
            if(!ModelState.IsValid)
                return CustomResponse(ModelState);

            var imageName = $"{Guid.NewGuid()}_{productDto.Image}";
            
            if(!_imageService.UploadFile(productDto.ImageUpload, imageName))
                return CustomResponse(productDto);

            productDto.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productDto));

            return CustomResponse(productDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductDto>> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if (product == null)
                return NotFound();

            await _productRepository.Delete(id);

            return CustomResponse(product);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<ProductDto> GetProduct(Guid id)
        {
            return _mapper.Map<ProductDto>(await _productRepository.GetProductProvider(id));
        }
    }
}
