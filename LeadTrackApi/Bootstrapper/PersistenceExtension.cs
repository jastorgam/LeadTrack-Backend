
using LeadTrackApi.Persistence.Models;
using LeadTrackApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTrack.API.Bootstrapper
{
    public static class PersistenceExtension
    {
        public static IServiceCollection RegisterRepositoryExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
            services.AddSingleton<MongoDBService>();
            return services;
        }
    }
}
