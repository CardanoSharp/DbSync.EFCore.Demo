using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Npgsql;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "User ID=postgrest_ro;Password=CHANGEME;Host=localhost;Port=5432;Database=cdbs;";

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        services.AddDbContext<IApplicationCardanoSharpEFCoreDbContext>(options => options.UseNpgsql(builder.ConnectionString));

        return services; 


    }
}
