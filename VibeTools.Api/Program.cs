using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using VibeTools.Api.Data;
using VibeTools.Api.Interfaces;
using VibeTools.Api.Repositories;
using VibeTools.Api.Routes;
using VibeTools.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services
builder.Services.AddDbContext<VibeToolsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ToolValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReviewValidator>();

builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration["Redis:ConnectionString"]
);

var app = builder.Build();

// Use middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// Add Endpoints
app.MapToolRoutes();
app.MapReviewRoutes();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VibeToolsContext>();
    db.Database.Migrate(); // Ensures DB is up to date
    SeedData.Initialize(db); // Custom method to insert test data
}

app.Run();