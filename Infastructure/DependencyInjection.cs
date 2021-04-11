using Application.Common.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CardanoContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Cardano")));

            services.AddTransient<IQueries, Queries>();
            
            return services;


        }
    }
}