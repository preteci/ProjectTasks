using DAL.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Middlewares;

namespace WebApi.Extensions
{
    public static class AplicationServiceExtension
    {
        public static IServiceCollection AddAplicationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ProjectDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            return services;
        }

        // exception middlawre
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }

    }
}
