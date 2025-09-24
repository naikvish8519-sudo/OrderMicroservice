using BusinessLogicLayer.HttpClients;
using eCommerce.OrdersMicroservice.API.Middleware;
using eCommerce.OrdersMicroservice.BusinessLogicLayer;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Services;
using eCommerce.OrdersMicroservice.DataAccessLayer.Context;
using eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;
using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DAL (DbContext with SQL Server + retry logic)
builder.Services.AddSqlServer<ApplicationDbContext>(
    builder.Configuration.GetConnectionString("SqlServer"),
    sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });

// Add BLL and other services
builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddHttpClient<UsersMicroserviceClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["Services:UsersMicroserviceBaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<ProductMicroserviceClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["Services:ProductMicroserviceBaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
});

//builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
//builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IPizzaOrdersService, PizzaOrdersService>();
builder.Services.AddScoped<IPizzaOrderRepository, PizzaOrderRepository>();

// Add Controllers
builder.Services.AddControllers();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:6000",
            "http://127.0.0.1:6000",
            "http://localhost:3000",
            "https://react-pizza-gsdtecavejezewfr.canadacentral-01.azurewebsites.net"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Exception Handling Middleware
app.UseExceptionHandlingMiddleware();

app.UseRouting();

// CORS
app.UseCors();

// Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "eCommerce API v1");
    c.RoutePrefix = "swagger";
});

app.MapGet("/", ctx =>
{
    ctx.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Endpoints
app.MapControllers();

app.Run();
