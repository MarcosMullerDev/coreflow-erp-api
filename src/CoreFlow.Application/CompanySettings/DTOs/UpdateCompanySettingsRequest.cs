namespace CoreFlow.Application.CompanySettings.DTOs;

public class UpdateCompanySettingsRequest
{
    public string StoreName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string PrimaryColor { get; set; } = "#dc2626";
    public string SecondaryColor { get; set; } = "#09090b";

    public string Whatsapp { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string HeroTitle { get; set; } = string.Empty;
    public string HeroSubtitle { get; set; } = string.Empty;
}