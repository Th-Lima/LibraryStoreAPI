using AutoMapper;
using LibraryStore.Api.Dtos;
using LibraryStore.Business.Interfaces;
using LibraryStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProvidersController : MainController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository, IProviderService providerService, IMapper mapper)
        {
            _providerRepository = providerRepository;
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
                return BadRequest();

            var provider = _mapper.Map<Provider>(providerDto);

            var result = await _providerService.Add(provider);

            if (!result)
                return BadRequest();

            return Ok(_mapper.Map<ProviderDto>(provider));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Update(Guid id, ProviderDto providerDto)
        {
            if (id != providerDto.Id)
                return NotFound("Não encontramos este fornecedor");

            if (!ModelState.IsValid)
                return BadRequest();

            var provider = _mapper.Map<Provider>(providerDto);

            var result = await _providerService.Update(provider);

            if (!result)
                return BadRequest();

            return Ok(provider);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProviderDto>> Delete(Guid id)
        {
            var provider = await GetProviderAddress(id);

            if (provider == null)
                return NotFound();

            var result = await _providerService.Remove(id);

            if (!result)
                return BadRequest();

            return Ok(provider);

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
