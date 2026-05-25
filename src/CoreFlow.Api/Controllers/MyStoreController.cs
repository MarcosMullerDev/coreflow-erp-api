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

    public MyStoreController(ICompanySettingsService service)
    {
        _service = service;
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
}