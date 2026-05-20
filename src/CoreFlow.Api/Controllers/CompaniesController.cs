using CoreFlow.Application.Common;
using CoreFlow.Application.Companies.DTOs;
using CoreFlow.Application.Companies.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyRequest request)
        {
            var company = await _companyService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = company.Id },
                ApiResponse<object>.Ok(company, "Company created successfully.")
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllAsync();

            return Ok(ApiResponse<object>.Ok(companies, "Companies retrieved successfully."));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var company = await _companyService.GetByIdAsync(id);

            if (company is null)
                return NotFound(ApiResponse<object>.Fail("Company not found."));

            return Ok(ApiResponse<object>.Ok(company, "Company retrieved successfully."));
        }
}