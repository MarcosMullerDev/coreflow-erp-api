using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class CompanySettings : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public string StoreName { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;

    public string LogoUrl { get; private set; } = string.Empty;
    public string BannerUrl { get; private set; } = string.Empty;

    public string PrimaryColor { get; private set; } = "#dc2626";
    public string SecondaryColor { get; private set; } = "#09090b";

    public string Whatsapp { get; private set; } = string.Empty;
    public string Instagram { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public string VehiclePhotoBackgroundUrl { get; private set; } = string.Empty;
    public string HeroTitle { get; private set; }
        = "Encontre o carro ideal para você";

    public string HeroSubtitle { get; private set; }
        = "Estoque selecionado e veículos revisados.";

    private CompanySettings() { }

    public CompanySettings(
        Guid companyId,
        string storeName,
        string slug)
    {
        CompanyId = companyId;
        StoreName = storeName;
        Slug = slug;
    }

    public void Update(
        string storeName,
        string slug,
        string primaryColor,
        string secondaryColor,
        string whatsapp,
        string instagram,
        string address,
        string heroTitle,
        string heroSubtitle)
    {
        StoreName = storeName;
        Slug = slug;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        Whatsapp = whatsapp;
        Instagram = instagram;
        Address = address;
        HeroTitle = heroTitle;
        HeroSubtitle = heroSubtitle;

        SetUpdated();
    }

    public void SetLogo(string logoUrl)
    {
        LogoUrl = logoUrl;
        SetUpdated();
    }

    public void SetBanner(string bannerUrl)
    {
        BannerUrl = bannerUrl;
        SetUpdated();
    }
    public void SetVehiclePhotoBackground(string url)
    {
        VehiclePhotoBackgroundUrl = url;
        SetUpdated();
    }
}