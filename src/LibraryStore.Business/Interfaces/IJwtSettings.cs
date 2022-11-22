namespace LibraryStore.Business.Interfaces
{
    public interface IJwtSettings
    {
        Task<string> GenerateJwt(string email);
    }
}
