namespace Identity.API.Settings;

public record ClientsSettings
{
    public SpaClientSetting Spa { get; set; }
}

public record SpaClientSetting
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string[] RedirectUris { get; set; }
    public string[] AllowedScopes { get; set; }
}