//using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
//using eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using MongoDB.Driver;

//namespace eCommerce.OrdersMicroservice.DataAccessLayer;

//public static class DependencyInjection
//{
//  public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
//  {
//    //TO DO: Add data access layer services into the IoC container
//    string connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;
//    string connectionString = connectionStringTemplate
//      .Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST"))
//      .Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT"));

//    services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

//    services.AddScoped<IMongoDatabase>(provider =>
//    {
//      IMongoClient client = provider.GetRequiredService<IMongoClient>();
//      return client.GetDatabase("OrdersDatabase");
//    });

//    services.AddScoped<IOrdersRepository, OrdersRepository>();


//    return services;
//  }
//}


using eCommerce.OrdersMicroservice.DataAccessLayer.Context; // Your DbContext namespace
using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
using eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.OrdersMicroservice.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Read SQL Server connection string from configuration
        var connectionString = configuration.GetConnectionString("SqlServer");

        // Register the ApplicationDbContext with EF Core using SQL Server provider
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register your repository
        services.AddScoped<IOrdersRepository, OrdersRepository>();

        return services;
    }
}
