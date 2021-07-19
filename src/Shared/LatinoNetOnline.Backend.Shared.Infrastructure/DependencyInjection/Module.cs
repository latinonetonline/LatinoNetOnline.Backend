using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.DependencyInjection
{

    public abstract class Module
    {
        public virtual void Load(IServiceCollection services, IConfiguration configuration)
        {
        }

        public virtual void Configure(IApplicationBuilder app)
        {
        }

        public virtual void InitialConfiguration(IConfiguration configuration)
        {
        }

        //internal IServiceCollection Loader<T>(IServiceCollection services)
        //    where T : Module
        //{
        //    Load(services);
        //    return services;
        //}
    }
}
