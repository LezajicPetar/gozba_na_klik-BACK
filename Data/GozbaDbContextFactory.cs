using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace gozba_na_klik.Data
{
    public class GozbaDbContextFactory : IDesignTimeDbContextFactory<GozbaDbContext>
    {
        public GozbaDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var conn = config.GetConnectionString("DefaultConnection")
                       ?? "Host=localhost;Port=5432;Database=gozba;Username=postgres;Password=postgres";

            var options = new DbContextOptionsBuilder<GozbaDbContext>()
                .UseNpgsql(conn)
                .Options;

            return new GozbaDbContext(options);
        }
    }
}
