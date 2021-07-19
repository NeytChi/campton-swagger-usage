using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Campton.Models
{
    /// <summary>
    /// Контект БД для управления таблицами данных.
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Настройки для коннекта к БД
        /// </summary>
        public static DatabaseSettings DatabaseConfiguration;
        private bool useConfiguration = true;
        public virtual DbSet<User> Users { get; set; }
        
        public Context()
        {

        }
        
        public Context(DatabaseSettings databaseConfiguration)
        {
            if (databaseConfiguration != null)
                DatabaseConfiguration = databaseConfiguration;
        }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (useConfiguration)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseMySql(DatabaseConnection(), new MySqlServerVersion(new Version(8, 0, 11)));
                }
            }
        }
        /// <summary>
        /// Получения строки подключения к БД
        /// </summary>
        /// <returns></returns>
        public static string DatabaseConnection()
        {
            if (DatabaseConfiguration == null)
            {
                var configuration = ServerConfiguration.Get();
                DatabaseConfiguration = configuration
                    .GetSection("DatabaseSettings")
                    .Get<DatabaseSettings>();
            }
            return "Server=" + DatabaseConfiguration.Server +
               ";Port=" + DatabaseConfiguration.Port +
               ";Database=" + DatabaseConfiguration.Database +
               ";User=" + DatabaseConfiguration.User +
               ";Pwd=" + DatabaseConfiguration.Password +
               ";Charset=utf8;";

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}