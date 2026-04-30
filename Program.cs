using FluentValidation;
using FluentValidation.AspNetCore;
using GestionProducto.Application.Interfaces;
using GestionProducto.Application.Services;
using GestionProducto.Domain.Interfaces;
using GestionProducto.DTOs.Producto;
using GestionProducto.Infra.Persistence;
using GestionProducto.Infra.Repositories;
using GestionProducto.Validators.Producto;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conexion_string = builder.Configuration.GetConnectionString("DefaultConnection")
?? throw new InvalidOperationException("Te hace falta la conexion a la bd.");

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(conexion_string);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

// Services
builder.Services.AddScoped<IProductoService, ProductoService>();

// Validators
builder.Services.AddScoped<IValidator<ProductoAgregarDto>, ProductoAgregarValidator>();
builder.Services.AddScoped<IValidator<ProductoActualizarDto>, ProductoActualizarValidator>();

// FluentValidation auto integration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();