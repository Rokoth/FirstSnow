using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirstSnow.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Topshelf;
using FirstSnow.FirstSnowDeployer;
using DataBaseEngine.Abstract;

namespace FirstSnow.Host
{
    public class Program
    {
        private const string _logDirectory = "Logs";
        private const string _logFileName = "log-startup.txt";
        private const string _appSettingsFileName = "appsettings.json";
        private const string _startUpInfoMessage = "App starts with arguments: {0}";
        private const string _errorNotifyOptionsSection = "ErrorNotifyOptions";

        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            string _startUpLogPath = Path.Combine(_logDirectory, _logFileName);
            var loggerConfig = new LoggerConfiguration()
               .WriteTo.Console()
               .WriteTo.File(_startUpLogPath)
               .MinimumLevel.Verbose();

            using var logger = loggerConfig.CreateLogger();
            logger.Information(string.Format(_startUpInfoMessage, string.Join(", ", args)));

            GetWebHostBuilder(args).Build().Run();
        }

        private static WebApplicationBuilder GetWebHostBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(GetConfiguration())
                .ConfigureAppConfiguration((hostingContext, config) => ConfigureApp(args, config))
                .ConfigureLogging((hostingContext, logging) => CreateLogger(hostingContext, logging))
                .UseKestrel()
                .UseStartup<Startup>();

            return builder;
        }

        private static void CreateLogger(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(hostingContext.Configuration)
                .CreateLogger();
            logging.AddSerilog(Log.Logger);
            logging.AddErrorNotifyLogger(config =>
            {
                config.Options = hostingContext.Configuration
                    .GetSection(_errorNotifyOptionsSection)
                    .Get<ErrorNotifyOptions>();
            });
        }

        private static void ConfigureApp(string[] args, IConfigurationBuilder config)
        {
            if (args != null) config.AddCommandLine(args);

        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(_appSettingsFileName, optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .AddDbConfiguration()
                                .Build();
        }
    }

    public class ConfigDbProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _options;
        private readonly IDeployService _deployService;

        public ConfigDbProvider(Action<DbContextOptionsBuilder> options,
            IDeployService deployService)
        {
            _options = options;
            _deployService = deployService;
        }

        public override void Load()
        {
            try
            {
                LoadInternal();
            }
            catch
            {
                try
                {
                    _deployService.Deploy().Wait();
                    LoadInternal();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void LoadInternal()
        {
            var builder = new DbContextOptionsBuilder<DbPgContext>();
            _options(builder);

            using (var context = new DbPgContext(builder.Options))
            {
                var items = context.Settings
                    .AsNoTracking()
                    .ToList();

                foreach (var item in items)
                {
                    Data.Add(item.ParamName, item.ParamValue);
                }
            }
        }
    }

    public class ConfigDbSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;
        private string _connectionString;

        public ConfigDbSource(Action<DbContextOptionsBuilder> optionsAction, string connectionString)
        {
            _optionsAction = optionsAction;
            _connectionString = connectionString;
        }

        public Microsoft.Extensions.Configuration.IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            IDeployService deployService = new DeployService(_connectionString);
            return new ConfigDbProvider(_optionsAction, deployService);
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Bearer ")
                }
            });
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Db.Model.User, Contract.Model.User>();
            //CreateMap<Contract.Model.UserCreator, Db.Model.User>()
            //    .ForMember(s => s.Password, s => s.Ignore());
            //CreateMap<Db.Model.UserHistory, Contract.Model.UserHistory>();
            //CreateMap<Contract.Model.UserUpdater, Db.Model.User>()
            //    .ForMember(s => s.Password, s => s.Ignore());

            //CreateMap<Db.Model.Formula, Contract.Model.Formula>();
            //CreateMap<Contract.Model.FormulaCreator, Db.Model.Formula>();
            //CreateMap<Db.Model.FormulaHistory, Contract.Model.FormulaHistory>();
            //CreateMap<Contract.Model.FormulaUpdater, Db.Model.Formula>();

            //CreateMap<Db.Model.Product, Contract.Model.Product>();
            //CreateMap<Contract.Model.ProductCreator, Db.Model.Product>();
            //CreateMap<Db.Model.ProductHistory, Contract.Model.ProductHistory>();
            //CreateMap<Contract.Model.ProductUpdater, Db.Model.Product>();

            //CreateMap<Db.Model.Incoming, Contract.Model.Incoming>();
            //CreateMap<Contract.Model.IncomingCreator, Db.Model.Incoming>();
            //CreateMap<Db.Model.IncomingHistory, Contract.Model.IncomingHistory>();
            //CreateMap<Contract.Model.IncomingUpdater, Db.Model.Incoming>();

            //CreateMap<Db.Model.Outgoing, Contract.Model.Outgoing>();
            //CreateMap<Contract.Model.OutgoingCreator, Db.Model.Outgoing>();
            //CreateMap<Db.Model.OutgoingHistory, Contract.Model.OutgoingHistory>();
            //CreateMap<Contract.Model.OutgoingUpdater, Db.Model.Outgoing>();

            //CreateMap<Db.Model.Reserve, Contract.Model.Reserve>();
            //CreateMap<Contract.Model.ReserveCreator, Db.Model.Reserve>();
            //CreateMap<Db.Model.ReserveHistory, Contract.Model.ReserveHistory>();
            //CreateMap<Contract.Model.ReserveUpdater, Db.Model.Reserve>();

            //CreateMap<Db.Model.Correction, Contract.Model.Correction>();
            //CreateMap<Contract.Model.CorrectionCreator, Db.Model.Correction>();
            //CreateMap<Db.Model.CorrectionHistory, Contract.Model.CorrectionHistory>();
            //CreateMap<Contract.Model.CorrectionUpdater, Db.Model.Correction>();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("MainConnection");
            builder.AddConfigDbProvider(options => options.UseNpgsql(connectionString), connectionString);
            return builder;
        }

        public static IConfigurationBuilder AddConfigDbProvider(
            this IConfigurationBuilder configuration, Action<DbContextOptionsBuilder> setup, string connectionString)
        {
            configuration.Add(new ConfigDbSource(setup, connectionString));
            return configuration;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new AutoMapper.MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static ILoggingBuilder AddErrorNotifyLogger(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ErrorNotifyLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <ErrorNotifyLoggerConfiguration, ErrorNotifyLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddErrorNotifyLogger(
            this ILoggingBuilder builder,
            Action<ErrorNotifyLoggerConfiguration> configure)
        {
            builder.Services.Configure(configure);
            builder.AddErrorNotifyLogger();

            return builder;
        }
    }
}
