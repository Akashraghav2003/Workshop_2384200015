using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class AddressContextFactory : IDesignTimeDbContextFactory<AddressContext>
    {
        public AddressContext CreateDbContext(string[] args)
        {
            // Setting the path to find 'appsettings.json'
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\AddressBook");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("SqlConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Database connection string is missing.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AddressContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AddressContext(optionsBuilder.Options);
        }
    }
}
