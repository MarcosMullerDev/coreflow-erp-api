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

        var uploadPath = Path.Combine(
            _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "uploads",
            "vehicles"
        );

        Directory.CreateDirectory(uploadPath);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        var uploadedImages = new List<object>();

        var alreadyHasPrimary = await _context.VehicleImages
            .AnyAsync(i => i.VehicleId == vehicleId && i.IsPrimary);

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
}