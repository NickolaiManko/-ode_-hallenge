using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using WindPark.API.Entities;

namespace WindPark.API.Repositories
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration, DbContextOptions<DataContext> options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        public DbSet<WindParkEntity> WindParks { get; set; }

    }
}
