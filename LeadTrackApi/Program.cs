
using LeadTrack.API.Bootstrapper;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace LeadTrackApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        // Obtener el logger correctamente configurado
        var logger = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        }).CreateLogger<Program>();

        // Add services to the container.
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("es-CL");
            options.SupportedCultures = new List<CultureInfo> { new CultureInfo("es-CL") };
            options.RequestCultureProviders = new List<IRequestCultureProvider>();
        });

        var security = builder.Configuration.GetSection("Security");
        var key = Encoding.UTF8.GetBytes(security["SecretKey"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = security["Issuer"],
                ValidAudience = security["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    logger.LogWarning($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    logger.LogWarning("Token validated successfully");
                    return Task.CompletedTask;
                }
            };
        });
        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.ListenAnyIP(5000); // Escucha en todos los interfaces en el puerto 5000
        //    options.ListenAnyIP(5001, listenOptions =>
        //    {
        //        listenOptions.UseHttps("/etc/nginx/ssl/certificate.crt", "passwordcita");
        //    });
        //});


        var cultureInfo = new CultureInfo("es-CL");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        var configuration = builder.Configuration;

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        builder.Services.RegisterApplicationExtension();
        builder.Services.RegisterRepositoryExtension(configuration);

        var app = builder.Build();
        var allowedOrigins = new[] { "http://localhost:4200", "https://leadtrack-12e68.web.app" };

        app.UseCors(options => options
            .SetIsOriginAllowed(origin =>
            {
                logger.LogInformation($"Origin: {origin}");

                if (allowedOrigins.Contains(origin)) return true;
                if (origin.StartsWith("https://leadtrack-12e68.web.app")) return true;

                logger.LogWarning($"Origin not allowed: {origin}");
                return false;
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition"));


        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
