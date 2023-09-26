using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Services;
using WebCrawlerApp.Infrastructure.Repositories;
using WebCrawlerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Text.Json;
using WebCrawlerApp.API.GraphQL;
using WebCrawlerApp.API.GraphQL.Schemas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });;

// Dependency Injection
builder.Services.AddScoped<IWebsiteService, WebsiteService>();
builder.Services.AddScoped<IExecutionService, ExecutionService>();
builder.Services.AddScoped<ICrawlService, CrawlService>();

builder.Services.AddScoped<IWebsiteRepository, WebsiteRepository>();
builder.Services.AddScoped<IExecutionRepository, ExecutionRepository>();
builder.Services.AddScoped<ICrawlRepository, CrawlRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddGraphQLServer().AddQueryType<Query>().AddObjectType<WebPage>().AddObjectType<Node>();

builder.Services.AddAutoMapper(typeof(WebsiteService).Assembly); 

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOriginsPolicy",
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

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7056);
    serverOptions.ListenAnyIP(7059, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

var presentaionMode = true;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || presentaionMode)
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

app.UseCors("AllowAllOriginsPolicy");

//app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

app.MapGraphQL("/graphql");
app.UseAuthorization();
app.MapControllers();

app.Run();