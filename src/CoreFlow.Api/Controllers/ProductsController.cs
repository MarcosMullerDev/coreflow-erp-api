using CoreFlow.Application.Common;
using CoreFlow.Application.Products.DTOs;
using CoreFlow.Application.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = await _productService.CreateAsync(request);

        return Ok(ApiResponse<object>.Ok(product, "Product created successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
    {
        var products = await _productService.GetAllAsync(request);

        return Ok(ApiResponse<object>.Ok(products, "Products retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
            return NotFound(ApiResponse<object>.Fail("Product not found."));

        return Ok(ApiResponse<object>.Ok(product, "Product retrieved successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request)
    {
        var product = await _productService.UpdateAsync(id, request);

        if (product is null)
            return NotFound(ApiResponse<object>.Fail("Product not found."));

        return Ok(ApiResponse<object>.Ok(product, "Product updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Product not found."));

        return Ok(ApiResponse<object>.Ok(null!, "Product deleted successfully."));
    }
}