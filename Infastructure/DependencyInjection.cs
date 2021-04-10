using Application.Interfaces;
using CardanoSharp.DbSync.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CardanoContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Cardano")));

            services.AddScoped<IApplicationCardanoSharpEFCoreDbContext>(provider => provider.GetService<IApplicationCardanoSharpEFCoreDbContext>());

            return services;


        }
    }
}