using AuthService.Application.Mapper;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstrations;
using AuthService.Application.Services.Implementations;
using AuthService.Infrastructure.Models;
using AuthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AuthService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add service registrations here
            string connectionString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add other infrastructure services as needed
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService , UserService>();
            //services.AddAutoMapper(cfg => cfg.AddProfile<UserMapper>());
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(UserMapper).Assembly);
        }
    }
}
