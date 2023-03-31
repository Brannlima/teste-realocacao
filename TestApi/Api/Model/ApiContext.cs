using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration.Internal;
using TestApi.Models;

namespace TestApi.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Empresa> Empresas { get; set; } = null!;
        public DbSet<Fornecedor> Fornecedores { get; set; } = null!;
        public DbSet<PessoaFisica> PessoasFisicas { get; set; } = null!;
        public DbSet<Empresarial> Empresariais { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiContext).Assembly);

            modelBuilder.Entity<Empresa>(ConfigureEmpresa);
            modelBuilder.Entity<Fornecedor>(ConfigureFornecedor);
            modelBuilder.Entity<PessoaFisica>(ConfigurePessoaFisica);
            modelBuilder.Entity<Empresarial>(ConfigureEmpresarial);
        }

        private static void ConfigureEmpresa(EntityTypeBuilder<Empresa> builder)
        {
            builder
                .HasMany(e => e.Fornecedores)
                .WithMany(f => f.Empresas)
                .UsingEntity(j => j.ToTable("EmpresaFornecedor"));

            builder.HasIndex(e => e.CNPJ).IsUnique();
            builder.Property(e => e.CNPJ).HasMaxLength(18).IsRequired();
        }

        private static void ConfigureFornecedor(EntityTypeBuilder<Fornecedor> builder)
        {
            builder
                .HasDiscriminator<string>("TipoFornecedor")
                .HasValue<Fornecedor>("Fornecedor")
                .HasValue<PessoaFisica>("PessoaFisica")
                .HasValue<Empresarial>("Empresarial");

            builder.Property(f => f.Nome).HasMaxLength(200).IsRequired();
        }

        private static void ConfigurePessoaFisica(EntityTypeBuilder<PessoaFisica> builder)
        {
            builder.HasIndex(pf => pf.CPF).IsUnique();
            builder.Property(pf => pf.CPF).HasMaxLength(14).IsRequired();

            builder.HasIndex(pf => pf.RG).IsUnique();

            builder.Property(pf => pf.DataNascimento).IsRequired();
        }

        private static void ConfigureEmpresarial(EntityTypeBuilder<Empresarial> builder)
        {
            builder.HasIndex(emp => emp.CNPJ).IsUnique();
            builder.Property(emp => emp.CNPJ).HasMaxLength(18).IsRequired();
        }
    }
}
