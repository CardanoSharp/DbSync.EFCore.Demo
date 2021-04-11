using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using CardanoSharp.DbSync.EntityFramework;
using Application.Common.Interfaces;
using Infastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var builder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("Cardano"));

        services.AddDbContext<CardanoContext>(options => options.UseNpgsql(builder.ConnectionString));
        services.AddTransient<IQueries, Queries>();

        return services; 


    }
}
