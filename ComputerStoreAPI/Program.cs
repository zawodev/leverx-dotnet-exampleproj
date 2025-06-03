using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ComputerStore.Infrastructure.Data;
using ComputerStore.Application.Repositories;
using ComputerStore.Infrastructure.Repositories;
using ComputerStore.Application.Behaviors;
using ComputerStore.Application.Features.Commands;
using ComputerStore.Application.Features.Validators;
using ComputerStore.Infrastructure.Mapping;

using AutoMapper;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(typeof(CreateProductCommand).Assembly);

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));





// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Dapper context
builder.Services.AddSingleton<IDapperContext, DapperContext>();

// repozytoria
builder.Services
    .AddScoped<IProductRepository, ProductRepository>()
    //.AddScoped<ICustomerRepository, CustomerRepository>()
    // and so on for other repositories, which i dont have time to add
    ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
