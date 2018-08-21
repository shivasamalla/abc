using Microsoft.Extensions.DependencyInjection;
using RBTCreditControl.Entity;
using RBTCreditControl.Repository;

namespace RBTCreditControl.WebApp.Models
{
    public static class ServiceConfigurationcs
    {
        public static void ConfigureRBTCControlServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ILocationMaster_Repo), typeof(LocationMaster_Repo));
            services.AddScoped(typeof(ICacheManager<User>), typeof(CacheManager<User>));

            services.AddScoped(typeof(IErrLogService), typeof(ErrLogService));
          
        }
    }
}
