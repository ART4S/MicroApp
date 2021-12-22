namespace Identity.API.Settings;

public record JwtSettings
{
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public string[] Audiences { get; set; }
}
