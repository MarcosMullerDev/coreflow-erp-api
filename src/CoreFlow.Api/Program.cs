using CoreFlow.Application.Companies.Validators;

using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CoreFlow.Application.Companies.Repositories;
using CoreFlow.Infrastructure.Repositories;
using CoreFlow.Application.Companies.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();

builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();