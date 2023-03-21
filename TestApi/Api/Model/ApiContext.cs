using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration.Internal;

namespace TestApi.Models
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Empresa> Empresa { get; set; } = null!;
        public DbSet<Fornecedor> Fornecedor { get; set; } = null!;
    }
}
