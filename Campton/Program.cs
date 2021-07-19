using Campton.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Campton
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var contextOptions = new DbContextOptionsBuilder<Context>()
                .UseMySql(Context.DatabaseConnection(), new MySqlServerVersion(new Version(8, 0, 11)))
                .Options;
            var context = new Context(contextOptions);
            context.Database.EnsureCreated();
            // �������� ������������ ������� ��� ��������� ������������
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            // ������� �������� ������������ ������ ����� ������������� � ������������
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.MySQL(Context.DatabaseConnection())
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // ��� ��� ������������� ��
            Log.Information("Starting up the API...");

            // �������� ����� ��� ����� ��������
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
