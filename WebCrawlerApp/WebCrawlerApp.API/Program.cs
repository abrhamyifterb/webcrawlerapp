using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Services;
using WebCrawlerApp.Infrastructure.Repositories;
using WebCrawlerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Text.Json;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.API.GraphQL.Schemas;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });;

// builder.Services.AddGraphQLServer()
//                 .AddQueryType<QueryType>();


// Dependency Injection
builder.Services.AddScoped<IWebsiteService, WebsiteService>();
builder.Services.AddScoped<IExecutionService, ExecutionService>();
builder.Services.AddScoped<ICrawlService, CrawlService>();

builder.Services.AddScoped<IWebsiteRepository, WebsiteRepository>();
builder.Services.AddScoped<IExecutionRepository, ExecutionRepository>();
builder.Services.AddScoped<ICrawlRepository, CrawlRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddGraphQLServer().AddQueryType<Query>().AddProjections().AddFiltering().AddSorting();

builder.Services.AddAutoMapper(typeof(WebsiteService).Assembly);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost:5432;Username=admin;Password=admin;Database=postgres"));

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

//Hangfire configuration
builder.Services.AddHangfire(configuration => configuration
    .UseMemoryStorage());
builder.Services.AddHangfireServer();

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My Web Crawler API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Web Crawler API V1");
    });
}

//Hangfire
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<ExecutionScheduler>(x => x.CheckAndExecutePendingWebsites(), Cron.Minutely);

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGraphQL("/graphql");

app.Run();