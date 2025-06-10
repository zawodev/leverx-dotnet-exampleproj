using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ComputerStore.Infrastructure.Data;
using ComputerStore.Application.Repositories;
using ComputerStore.Infrastructure.Repositories;
using ComputerStore.Application.Behaviors;
using ComputerStore.Application.Features.Commands;
using ComputerStore.Application.Features.Validators;
using ComputerStore.Infrastructure.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using System.Text;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;
using FluentValidation;
using MediatR;
using ComputerStore.Infrastructure.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Microsoft.Extensions.DependencyInjection;

using Serilog;
using Serilog.Events;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// health check
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "sql",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql" }
    );

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Logging (default)
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();

var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = jwt["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwt["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("CanManageProducts", policy => policy.RequireRole("Manager", "Admin"));
});

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
Log.Information("Application starting up");

app.MapHealthChecks("/health", new HealthCheckOptions {
    ResponseWriter = async (ctx, report) => {
        ctx.Response.ContentType = "application/json";
        var result = new {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        };
        await ctx.Response.WriteAsJsonAsync(result);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();


try {
    app.Run();
    Log.Information("Application started successfully");
} catch (Exception ex) {
    Log.Fatal(ex, "Application failed to start");
} finally {
    Log.CloseAndFlush();
}
