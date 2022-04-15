using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data.EF
{
    public class UserAndTestDbContextFactory : IDesignTimeDbContextFactory<UserAndTestDbContext>
    {
        public UserAndTestDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("UserAndTestnDb");


            var optionsBuilder = new DbContextOptionsBuilder<UserAndTestDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new UserAndTestDbContext(optionsBuilder.Options);
        }
    }
}
