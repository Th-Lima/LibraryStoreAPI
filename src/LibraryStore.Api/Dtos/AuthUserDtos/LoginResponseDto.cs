namespace LibraryStore.Api.Dtos.AuthUserDtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }

        public double ExpiresIn { get; set; }

        public UserTokenDto UserToken { get; set; }
    }
}
