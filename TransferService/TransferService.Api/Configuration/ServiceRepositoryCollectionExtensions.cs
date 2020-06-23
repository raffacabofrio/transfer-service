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


            //repositories
            services.AddScoped<IUserRepository, UserRepository>();

            //validators
            services.AddScoped<IValidator<User>, UserValidator>();

            //Auth
            services.AddScoped<IApplicationSignInManager, ApplicationSignInManager>();

            //UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}