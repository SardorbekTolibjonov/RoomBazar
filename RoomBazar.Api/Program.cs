using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoomBazar.Api.Extensions;
using RoomBazar.Api.Middlewares;
using RoomBazar.Api.Models;
using RoomBazar.Data.DbContexts;
using RoomBazar.Service.Interfaces.Auths;
using RoomBazar.Service.Services.Auths;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// api url
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
                                        new ConfigurationApiUrlName()));
});

// CustomService
builder.Services.AddCustomService();
// JWT
builder.Services.AddJwtService(builder.Configuration);
// Swagger
builder.Services.AddSwaggerService();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
app.UseAuthorization();


app.MapControllers();

app.Run();
