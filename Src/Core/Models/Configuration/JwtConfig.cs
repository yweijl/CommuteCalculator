namespace Core.Models.Configuration;

public class JwtConfig
{
    public string AppSecret { get; set; } = default!;
    public string ValidIssuer { get; set; } = default!;
    public string ValidAudience { get; set; } = default!;
    public int ExpiresInMinutes { get; set; }
}