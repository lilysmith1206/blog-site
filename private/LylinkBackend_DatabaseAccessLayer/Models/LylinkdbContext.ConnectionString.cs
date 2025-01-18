using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;

namespace LylinkBackend_DatabaseAccessLayer.Models
{
    public partial class LylinkdbContext
    {
        private readonly string? _connectionString;

        public LylinkdbContext()
        {
        }

        public LylinkdbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LylinkdbContext(DbContextOptions<LylinkdbContext> options)
            : base(options)
        {
            _connectionString = options.Extensions
                .OfType<MySqlOptionsExtension>()
                .FirstOrDefault()?.ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                if (string.IsNullOrEmpty(_connectionString) == false)
                {
                    optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse("11.5.2-mariadb"));
                }
            }
        }
    }
}
