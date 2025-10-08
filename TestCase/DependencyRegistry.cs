using LiteDB;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestCase.Repository;
using TestCase.Services;

namespace TestCase
{
    public static class DependencyRegistry
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add LiteDB with Scoped lifetime
            services.AddSingleton<ILiteDatabase>(provider =>
            {
                // Get connection string from configuration
                var connectionString = configuration.GetConnectionString("Default");
                return new LiteDatabase(connectionString);
            });
            
            // Add AppSettings configuration
            services.Configure<AppSettngs>(configuration.GetSection("AppSettings"));
            
            // Add generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            // Add services
            services.AddScoped<ProjectService>();
            services.AddScoped<ModuleService>();
            services.AddScoped<TestCaseService>();
            services.AddScoped<AttachmentService>();
            services.AddScoped<TagService>();
            services.AddScoped<ImageService>();
            services.AddScoped<TestResultService>();
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();


            //services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add controllers
            services.AddControllers();

            services.AddScoped<IImagePreviewService, ImagePreviewService>();
        }
    }
}