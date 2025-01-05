
using LeadTrackApi.Application.Business;
using LeadTrackApi.Application.Interfaces;
using LeadTrackApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LeadTrack.API.Bootstrapper
{
    public static class ApplicationExtension
    {
        public static IServiceCollection RegisterApplicationExtension(this IServiceCollection services)
        {
            services.AddSingleton<JwtService>();
            services.AddTransient<ILeadBusiness, LeadBusiness>();
            services.AddTransient<IAuthBusiness, AuthBusiness>();
            return services;
        }
    }
}
