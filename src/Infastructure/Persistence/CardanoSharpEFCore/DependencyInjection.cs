using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Npgsql;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CardanoDbSharp;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("Cardano"));

        services.AddDbContext<CardanoContext>(options => options.UseNpgsql(builder.ConnectionString));

        return services; 


    }
}
