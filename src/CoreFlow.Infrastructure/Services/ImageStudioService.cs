using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CoreFlow.Infrastructure.Services;

public class ImageStudioService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ImageStudioService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task ApplyStoreBackgroundAsync(
        string vehicleImagePath,
        string backgroundUrl)
    {
        var apiKey = _configuration["RemoveBg:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Remove.bg API key not configured.");

        var transparentVehicleBytes = await RemoveBackgroundAsync(
            vehicleImagePath,
            apiKey
        );

        if (backgroundUrl.Contains("localhost"))
        {
            backgroundUrl = backgroundUrl.Replace(
                "localhost",
                "host.docker.internal"
            );
        }

        var backgroundBytes = await _httpClient.GetByteArrayAsync(backgroundUrl);

        using var background = Image.Load<Rgba32>(backgroundBytes);
        using var vehicle = Image.Load<Rgba32>(transparentVehicleBytes);

        background.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(1200, 800),
            Mode = ResizeMode.Crop,
            Position = AnchorPositionMode.Center
        }));

        vehicle.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(980, 560),
            Mode = ResizeMode.Max
        }));

        var vehicleX = (background.Width - vehicle.Width) / 2;
        var vehicleY = background.Height - vehicle.Height - 80;

        using var shadow = new Image<Rgba32>(vehicle.Width, 80);

        shadow.Mutate(x =>
        {
            x.BackgroundColor(Color.FromRgba(0, 0, 0, 90));
            x.GaussianBlur(22);
        });

        var shadowX = vehicleX;
        var shadowY = vehicleY + vehicle.Height - 30;

        background.Mutate(x =>
        {
            x.DrawImage(shadow, new Point(shadowX, shadowY), 0.35f);
            x.DrawImage(vehicle, new Point(vehicleX, vehicleY), 1f);
        });

        await background.SaveAsPngAsync(vehicleImagePath, new PngEncoder());
    }

    private async Task<byte[]> RemoveBackgroundAsync(
        string imagePath,
        string apiKey)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.remove.bg/v1.0/removebg"
        );

        request.Headers.Add("X-Api-Key", apiKey);

        using var form = new MultipartFormDataContent();

        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        var imageContent = new ByteArrayContent(imageBytes);

        form.Add(imageContent, "image_file", Path.GetFileName(imagePath));
        form.Add(new StringContent("auto"), "size");

        request.Content = form;

        using var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Remove.bg error: {error}");
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
}