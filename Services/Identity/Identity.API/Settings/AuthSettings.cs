namespace Identity.API.Settings;

public record AuthSettings
{
    public string IssuerUri { get; set; }
    public int CookieLifetimeSec { get; set; }
    public bool CookieSlidingExpiration { get; set; }
}
