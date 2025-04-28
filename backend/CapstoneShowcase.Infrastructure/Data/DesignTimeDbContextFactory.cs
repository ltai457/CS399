using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CapstoneShowcase.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1434;Database=CapstoneShowcase;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
} 