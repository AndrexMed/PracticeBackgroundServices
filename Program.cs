using AutomaticProcess.BackgroundServices;
using AutomaticProcess.Data.Context;
using AutomaticProcess.Interfaces;
using AutomaticProcess.Options;
using AutomaticProcess.Repositories;
using AutomaticProcess.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<TestOptions>(builder.Configuration.GetSection("TestOptions"));

builder.Services.AddDbContext<TestDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TEST")), ServiceLifetime.Transient);

builder.Services.AddScoped<ITestDbContext, TestDbContext>();
builder.Services.AddTransient<ITestRepository, TestRepository>();
builder.Services.AddTransient<ITestOneService, TestOneService>();

builder.Services.AddHostedService<TestOneBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
