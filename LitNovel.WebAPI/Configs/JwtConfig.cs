namespace LitNovel.WebAPI.Configs
{
    public class JwtConfig
    {
        public string Key { get; set; } = default!;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpiresInSeconds { get; set; } = 900;
    }
}
