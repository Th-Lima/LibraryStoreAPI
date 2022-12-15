using AutoMapper;
using LibraryStore.Api.Controllers;
using LibraryStore.Api.Dtos;
using LibraryStore.Api.Extensions;
using LibraryStore.Business.Interfaces;
using LibraryStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ProvidersController : MainController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository,
            IAddressRepository addressRepository,
            IProviderService providerService,
            IMapper mapper,
            INotifier notifier,
            IUser appUser) : base(notifier, appUser)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
            _providerService = providerService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAll()
        {
            var providers = _mapper.Map<IEnumerable<ProviderDto>>(await _providerRepository.GetAll());

            return Ok(providers);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> GetProviderById(Guid id)
        {
            var provider = await GetProviderProductsAddress(id);

            if (provider == null)
                return NotFound();

            return provider;
        }

        [HttpGet("get-address/{id:guid}")]
        public async Task<AddressDto> GetProviderAddressById(Guid id)
        {
            return _mapper.Map<AddressDto>(await _addressRepository.GetById(id));
        }

        [ClaimsAuthorize("Provider", "Add")]
        [HttpPost]
        public async Task<ActionResult<ProviderDto>> Post(ProviderDto providerDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.Add(_mapper.Map<Provider>(providerDto));

            return CustomResponse(providerDto);
        }

        [ClaimsAuthorize("Provider", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Put(Guid id, ProviderDto providerDto)
        {
            if (id != providerDto.Id)
            {
                NotificationError("Não encontramos este fornecedor, o id não é o mesmo informado na query");
                
                return CustomResponse(providerDto);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.Update(_mapper.Map<Provider>(providerDto));

            return CustomResponse(providerDto);
        }

        [ClaimsAuthorize("Provider", "Update")]
        [HttpPut("update-address/{id:guid}")]
        public async Task<IActionResult> Put(Guid id, AddressDto addressDto)
        {
            if (id != addressDto.Id)
            {
                NotificationError("Não encontramos este endereço, o id não é o mesmo informado na query");

                return CustomResponse(addressDto);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.UpdateAddress(_mapper.Map<Address>(addressDto));

            return CustomResponse(addressDto);
        }

        [ClaimsAuthorize("Provider", "Remove")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Delete(Guid id)
        {
            var providerDto = await GetProviderAddress(id);

            if (providerDto == null)
                return NotFound();

            await _providerService.Remove(id);

            return CustomResponse(providerDto);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<ProviderDto> GetProviderProductsAddress(Guid id)
        {
            return _mapper.Map<ProviderDto>(await _providerRepository.GetProviderProductsAddress(id));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<ProviderDto> GetProviderAddress(Guid id)
        {
            return _mapper.Map<ProviderDto>(await _providerRepository.GetProviderAddress(id));
        }
    }
}
