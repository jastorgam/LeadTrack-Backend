using LeadTrack.API.Bootstrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace LeadTrack.API.Test
{
    public class TestConfiguration
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public TestConfiguration()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();

            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.RegisterApplicationExtension();
            serviceCollection.RegisterRepositoryExtension(configuration);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

    }
}
