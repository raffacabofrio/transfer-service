using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TransferService.Domain;
using TransferService.Domain.Validators;
using TransferService.Infra.CrossCutting.Identity;
using TransferService.Infra.CrossCutting.Identity.Interfaces;
using TransferService.Repository;
using TransferService.Repository.UoW;
using TransferService.Service;

namespace TransferService.Api.Configuration
{
    public static class ServiceRepositoryCollectionExtensions
    {
        public static IServiceCollection RegisterRepositoryServices(
           this IServiceCollection services)
        {
            //services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransferService, Service.TransferService>();

            //repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();

            //validators
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Transfer>, TransferValidator>();

            //Auth
            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}