using CoreFlow.Application.Common;
using CoreFlow.Application.CompanySettings.DTOs;
using CoreFlow.Application.CompanySettings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/MyStore")]
public class MyStoreController : ControllerBase
{
    private readonly ICompanySettingsService _service;
    private readonly IWebHostEnvironment _environment;

    public MyStoreController(
        ICompanySettingsService service,
        IWebHostEnvironment environment)
    {
        _service = service;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var settings = await _service.GetAsync();

        return Ok(ApiResponse<object>.Ok(settings));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCompanySettingsRequest request)
    {
        var settings = await _service.UpdateAsync(request);

        return Ok(ApiResponse<object>.Ok(
            settings,
            "Store settings updated successfully."
        ));
    }

    [HttpPost("logo")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        var result = await UploadStoreImage(file, "logo");
        var settings = await _service.SetLogoAsync(result.Url);

        return Ok(ApiResponse<object>.Ok(
            settings,
            "Logo uploaded successfully."
        ));
    }

    [HttpPost("banner")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UploadBanner(IFormFile file)
    {
        var result = await UploadStoreImage(file, "banner");
        var settings = await _service.SetBannerAsync(result.Url);

        return Ok(ApiResponse<object>.Ok(
            settings,
            "Banner uploaded successfully."
        ));
    }
    
    [HttpPost("vehicle-photo-background")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UploadVehiclePhotoBackground(IFormFile file)
    {
        var result = await UploadStoreImage(file, "vehicle-bg");
        var settings = await _service.SetVehiclePhotoBackgroundAsync(result.Url);

        return Ok(ApiResponse<object>.Ok(
            settings,
            "Vehicle photo background uploaded successfully."
        ));
    }
    private async Task<(string FileName, string Url)> UploadStoreImage(
        IFormFile file,
        string prefix)
    {
        if (file is null || file.Length == 0)
            throw new InvalidOperationException("No file uploaded.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Invalid image format.");

        var uploadPath = Path.Combine(
            _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "uploads",
            "stores"
        );

        Directory.CreateDirectory(uploadPath);

        var fileName = $"{prefix}-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var url = $"{Request.Scheme}://{Request.Host}/uploads/stores/{fileName}";

        return (fileName, url);
    }
}