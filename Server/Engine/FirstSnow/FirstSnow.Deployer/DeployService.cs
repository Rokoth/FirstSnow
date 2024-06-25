﻿using FirstSnow.Common;
using Deployer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.RegularExpressions;


namespace FirstSnow.FirstSnowDeployer
{
    /// <summary>
    /// Deploy lib wrapper
    /// </summary>
    public class DeployService : IDeployService
    {
        private readonly ILogger<DeployService>? _logger;       
        private readonly string _connectionString;

        public DeployService(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<DeployService>>();         
            var _options = serviceProvider.GetRequiredService<IOptions<CommonOptions>>();
            _connectionString = _options.Value.ConnectionStrings.FirstOrDefault(s=>s.Key == "MainConnection").Value;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public DeployService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Deploy db
        /// </summary>
        /// <param name="num">number of last update</param>
        /// <returns></returns>
        public async Task Deploy(int? num = null)
        {
            var deployLog = string.Empty;
            try
            {
                CheckDbForExists(_connectionString);
              
                DeploySettings deploySettings = new()
                {
                    BeginNum = num,
                    CheckSqlPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "Check"),
                    ConnectionString = _connectionString,
                    DeploySqlPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "Deploy"),
                    UpdateSqlPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "Update")
                };
                var deployer = new Deployer.Deployer(deploySettings);
                deployer.OnError += (sender, message) =>
                {
                    deployLog += message + "\r\n";
                    _logger?.LogError("Error in deployer: {message}", message);
                };
                deployer.OnDebug += (sender, message) =>
                {
                    _logger?.LogDebug("Debug info from deployer: {message}", message);
                };
                deployer.OnMessage += (sender, message) =>
                {
                    _logger?.LogInformation("Message info from deployer: {message}", message);
                };
                deployer.OnWarning += (sender, message) =>
                {
                    deployLog += message + "\r\n";
                    _logger?.LogWarning("Warning in deployer: {message}", message);
                };

                if (!await deployer.Deploy())
                {                    
                    throw new DeployException($"DB was not deploy, log: {deployLog}");
                }
            }
            catch (DeployException)
            {               
                throw;
            }
            catch (Exception ex)
            {                
                throw new DeployException($"" +
                    $"Error while Deploy DB.\r\n" +
                    $"Message: {ex.Message}\r\n" +
                    $"StackTrace: {ex.StackTrace}\r\n" +
                    $"DeployLog: {deployLog}");
            }
        }

        /// <summary>
        /// TODO: move to deploy lib
        /// </summary>
        /// <param name="connectionString"></param>
        private static void CheckDbForExists(string connectionString)
        {
            try
            {
                var dbName = Regex.Match(connectionString, "Database=(.*?);").Groups[1].Value;
                var rootConnectionString = Regex.Replace(connectionString, "Database=.*?;", $"Database=postgres;");
                using NpgsqlConnection _connPg = new(rootConnectionString);
                _connPg.Open();
                string script1 = $"select exists(SELECT 1 FROM pg_database WHERE datname = '{dbName}');";
                var cmd1 = new NpgsqlCommand(script1, _connPg);
                var result = cmd1.ExecuteScalar();
                if (result != null && !(bool)result)
                {
                    string script2 = $"create database {dbName};";
                    var cmd2 = new NpgsqlCommand(script2, _connPg);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new DeployException($"Не удалось развернуть базу данных: " +
                    $"ошибка при проверке или создании базы: {ex.Message} {ex.StackTrace}");
            }
        }
    }
}