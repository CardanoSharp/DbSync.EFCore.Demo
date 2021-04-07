using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Npgsql;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=yourPassword;Database=mydb;");
    }
}
