namespace USP_Project.Web.Settings;

public class GoogleOptions
{
    public const string ConfigSection = "Google";
    
    public string ClientId { get; set; } = default!;
    
    public string ClientSecret { get; set; } = default!;
}