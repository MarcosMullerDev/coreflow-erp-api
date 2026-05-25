using CoreFlow.Application.Common;
using CoreFlow.Domain.Entities;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/Vehicles/{vehicleId:guid}/Images")]
public class VehicleImagesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public VehicleImagesController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetImages(Guid vehicleId)
    {
        var vehicleExists = await _context.Vehicles
            .AnyAsync(v => v.Id == vehicleId && !v.IsDeleted);

        if (!vehicleExists)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        var images = await _context.VehicleImages
            .AsNoTracking()
            .Where(i => i.VehicleId == vehicleId && !i.IsDeleted)
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.CreatedAt)
            .Select(i => new
            {
                i.Id,
                i.FileName,
                i.IsPrimary,
                Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}"
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(images, "Images retrieved successfully."));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Upload(Guid vehicleId, List<IFormFile> files)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == vehicleId && !v.IsDeleted);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        if (files is null || files.Count == 0)
            return BadRequest(ApiResponse<object>.Fail("No files uploaded."));

        var uploadPath = GetUploadPath();
        Directory.CreateDirectory(uploadPath);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var uploadedImages = new List<object>();

        var alreadyHasPrimary = await _context.VehicleImages
            .AnyAsync(i => i.VehicleId == vehicleId && i.IsPrimary && !i.IsDeleted);

        foreach (var file in files)
        {
            if (file.Length == 0)
                continue;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(ApiResponse<object>.Fail("Invalid image format."));

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var isPrimary = !alreadyHasPrimary && uploadedImages.Count == 0;
            var vehicleImage = new VehicleImage(vehicleId, fileName, isPrimary);

            await _context.VehicleImages.AddAsync(vehicleImage);

            uploadedImages.Add(new
            {
                vehicleImage.Id,
                vehicleImage.FileName,
                vehicleImage.IsPrimary,
                Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{fileName}"
            });

            if (isPrimary)
                alreadyHasPrimary = true;
        }

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(uploadedImages, "Images uploaded successfully."));
    }

    [HttpPatch("{imageId:guid}/primary")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> SetPrimary(Guid vehicleId, Guid imageId)
    {
        var images = await _context.VehicleImages
            .Where(i => i.VehicleId == vehicleId && !i.IsDeleted)
            .ToListAsync();

        if (!images.Any())
            return NotFound(ApiResponse<object>.Fail("Images not found."));

        var selectedImage = images.FirstOrDefault(i => i.Id == imageId);

        if (selectedImage is null)
            return NotFound(ApiResponse<object>.Fail("Image not found."));

        foreach (var image in images)
        {
            image.SetPrimary(image.Id == imageId);
        }

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(new { }, "Primary image updated successfully."));
    }

    [HttpDelete("{imageId:guid}")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Delete(Guid vehicleId, Guid imageId)
    {
        var image = await _context.VehicleImages
            .FirstOrDefaultAsync(i =>
                i.Id == imageId &&
                i.VehicleId == vehicleId &&
                !i.IsDeleted);

        if (image is null)
            return NotFound(ApiResponse<object>.Fail("Image not found."));

        var wasPrimary = image.IsPrimary;
        var filePath = Path.Combine(GetUploadPath(), image.FileName);

        image.MarkAsDeleted();

        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        if (wasPrimary)
        {
            var nextImage = await _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId && i.Id != imageId && !i.IsDeleted)
                .OrderBy(i => i.CreatedAt)
                .FirstOrDefaultAsync();

            if (nextImage is not null)
                nextImage.SetPrimary(true);
        }

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(new { }, "Image deleted successfully."));
    }

    private string GetUploadPath()
    {
        return Path.Combine(
            _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "uploads",
            "vehicles"
        );
    }
}