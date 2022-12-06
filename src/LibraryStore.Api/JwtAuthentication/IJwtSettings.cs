using LibraryStore.Api.Dtos.AuthUserDtos;

namespace LibraryStore.Api.JwtAuthentication
{
    public interface IJwtSettings
    {
        Task<LoginResponseDto> GenerateJwt(string email);
    }
}
