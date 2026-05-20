using CoreFlow.Application.Common;
using CoreFlow.Application.Sales.DTOs;
using CoreFlow.Application.Sales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Sales")]
    public async Task<IActionResult> Create(CreateSaleRequest request)
    {
        var sale = await _saleService.CreateAsync(request);

        return Ok(ApiResponse<object>.Ok(sale, "Sale created successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleService.GetAllAsync();

        return Ok(ApiResponse<object>.Ok(sales, "Sales retrieved successfully."));
    }
}