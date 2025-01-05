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

            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.RegisterApplicationExtension();
            serviceCollection.RegisterRepositoryExtension(configuration);
            //serviceCollection.RegisterInfraestructureExtension(configuration);


            // TODO Agregar Handlers correspondientes
            //serviceCollection.AddMediatR(typeof(SignMandateHandler));
            //serviceCollection.AddMediatR(typeof(GetMandatePdfHandler));
            //serviceCollection.AddMediatR(typeof(HasMandateHandler));

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

    }
}
