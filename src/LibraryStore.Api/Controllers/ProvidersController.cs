using AutoMapper;
using LibraryStore.Api.Dtos;
using LibraryStore.Business.Interfaces;
using LibraryStore.Data.Repository;
using LibraryStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProvidersController : MainController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository, IAddressRepository addressRepository, IProviderService providerService, IMapper mapper, INotifier notifier) : base(notifier)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
            _providerService = providerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAll()
        {
            var providers = _mapper.Map<IEnumerable<ProviderDto>>(await _providerRepository.GetAll());

            return Ok(providers);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> GetById(Guid id)
        {
            var provider = await GetProviderProductsAddress(id);

            if (provider == null)
                return NotFound();

            return provider;
        }

        [HttpPost]
        public async Task<ActionResult<ProviderDto>> Add(ProviderDto providerDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.Add(_mapper.Map<Provider>(providerDto));

            return CustomResponse(providerDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Update(Guid id, ProviderDto providerDto)
        {
            if (id != providerDto.Id)
            {
                NotificationErro("Não encontramos este fornecedor, o id não é o mesmo informado na query");
                
                return CustomResponse(providerDto);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.Update(_mapper.Map<Provider>(providerDto));

            return CustomResponse(providerDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Delete(Guid id)
        {
            var providerDto = await GetProviderAddress(id);

            if (providerDto == null)
                return NotFound();

            await _providerService.Remove(id);

            return CustomResponse(providerDto);
        }

        [HttpGet("get-address/{id:guid}")]
        public async Task<AddressDto> GetAddressById(Guid id)
        {
            return _mapper.Map<AddressDto>(await _addressRepository.GetById(id));
        }

        [HttpPut("update-address/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressDto addressDto)
        {
            if (id != addressDto.Id)
            {
                NotificationErro("Não encontramos este endereço, o id não é o mesmo informado na query");

                return CustomResponse(addressDto);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _providerService.UpdateAddress(_mapper.Map<Address>(addressDto));

            return CustomResponse(addressDto);
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
