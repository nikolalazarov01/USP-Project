namespace USP_Project.Web.Settings;

public class FacebookOptions
{
    public const string ConfigSection = "Facebook";
    
    public string AppId { get; set; } = default!;
    
    public string AppSecret { get; set; } = default!;
}