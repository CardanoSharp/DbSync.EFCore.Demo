using Application.Common.Interfaces;
using AutoMapper;
using CardanoSharp.Wallet;
using CardanoSharp.Wallet.Encoding;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IKeyService, KeyService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IBech32,Bech32>();


            return services;
        }
    }
}