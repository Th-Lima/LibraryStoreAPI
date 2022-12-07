using AutoMapper;
using LibraryStore.Api.Dtos;
using LibraryStore.Business.Interfaces;
using LibraryStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ProductsController(INotifier notifier,
            IProductRepository productRepository,
            IProductService productService,
            IImageService imageService,
            IMapper mapper,
            IUser appUser) : base(notifier, appUser)
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
            
            if(!_imageService.UploadFileAndConvertToBase64(productDto.ImageUpload, imageName))
                return CustomResponse(productDto);

            productDto.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productDto));

            return CustomResponse(productDto);
        }

        #region Method with IFormFile image
        [HttpPost("add-with-image-iformfile")]
        //[RequestSizeLimit(40000000)]
        public async Task<ActionResult<ProductImageDto>> PostWithImageIFormFile(ProductImageDto productImageDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var imgPrefix = $"{Guid.NewGuid()}_";

            if (!await _imageService.UploadFileWithIFormFile(productImageDto.ImageUpload, imgPrefix))
                return CustomResponse(ModelState);

            productImageDto.Image = imgPrefix + productImageDto.ImageUpload.FileName;
            await _productService.Add(_mapper.Map<Product>(productImageDto));

            return CustomResponse(productImageDto);
        }
        #endregion

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, ProductImageDto productImageDto)
        {
            if (id != productImageDto.Id)
            {
                NotificationError("Os Id's não são correspondentes");
                return CustomResponse();
            }

            var productUpdate = await GetProduct(id);
            productImageDto.Image = productUpdate.Image;
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if(productImageDto.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productImageDto.Image;
                if(!await _imageService.UploadFileWithIFormFile(productImageDto.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                productUpdate.Image = imageName;
            }

            productUpdate.Name = productImageDto.Name;
            productUpdate.Description = productImageDto.Description;
            productUpdate.Price = productImageDto.Price;
            productUpdate.Active = productImageDto.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productUpdate);
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
