using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YaEcs.Bootstrap;

namespace YaEcs.MicrosoftDependencyInjectionExtensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEcs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<UpdateStepRegistry>()
                .AddScoped(_ => UpdateSteps.First)
                .AddScoped(_ => UpdateSteps.EarlyUpdate)
                .AddScoped(_ => UpdateSteps.Update)
                .AddScoped(_ => UpdateSteps.LateUpdate);
            services.AddScoped<IWorld, World>()
                .AddScoped<IComponents, Components>()
                .AddScoped<IEntities, Entities>();
            return services;
        }
    }   
}