using System;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Persistence.EF.SQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.MisCentral.Services
{
    public interface IDynamicDbContextFactory
    {
        DashboardDbContext CreateDbContext(string connectionString);
    }

    public class DynamicDbContextFactory : IDynamicDbContextFactory
    {
        public DashboardDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DashboardDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });

            return new DashboardDbContext(optionsBuilder.Options, new MigrationsTenantProvider());
        }
    }
}
