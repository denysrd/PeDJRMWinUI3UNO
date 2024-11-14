using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Defina os DbSets para cada entidade
        public DbSet<FornecedorModel> FornecedorModel { get; set; }
        public DbSet<TipoIngredienteModel> TipoIngredientes { get; set; }
        public DbSet<TipoFormulacaoModel> TipoFormulacaoModel { get; set; }
        public DbSet<InsumosModel> InsumosModel { get; set; }
        public DbSet<FlavorizantesModel> FlavorizantesModel { get; set; }

        // Configuração condicional do DbContext
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Usa uma configuração padrão se nenhuma configuração for passada
                const string connectionString = "Server=localhost;Database=db_jrmsistema;User=root;Password=Neoo@@141189;";
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração das entidades
            modelBuilder.Entity<FornecedorModel>(entity =>
            {
                entity.HasKey(e => e.Id_Fornecedor); // Define a chave primária
                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.Documento).IsRequired();
            });

            modelBuilder.Entity<TipoIngredienteModel>(entity =>
            {
                entity.ToTable("tbl_tipo_ingrediente");
                entity.HasKey(e => e.Id_Tipo_Ingrediente);
                entity.Property(e => e.Tipo_Ingrediente).IsRequired();
                entity.Property(e => e.Descricao_Tipo_Ingrediente).HasMaxLength(255);
                entity.Property(e => e.Sigla).HasMaxLength(10);
                entity.Property(e => e.Situacao).IsRequired().HasDefaultValue(false);
            });

            modelBuilder.Entity<TipoFormulacaoModel>(entity =>
            {
                entity.ToTable("tbl_tipo_formulacao");
                entity.HasKey(e => e.Id_Tipo_Formulacao);
                entity.Property(e => e.Tipo_Formula).IsRequired();
                entity.Property(e => e.Descricao_Formula).HasMaxLength(255);
            });

            modelBuilder.Entity<InsumosModel>(entity =>
            {
                entity.HasKey(e => e.Id_Insumo);
                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.Custo).HasColumnType("decimal(18,2)");

                // Configuração da chave estrangeira para TipoIngrediente
                entity.HasOne(e => e.TipoIngrediente)
                      .WithMany()
                      .HasForeignKey(e => e.IdTipoIngrediente)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<InsumosModel>(entity =>
            {
                entity.HasKey(e => e.Id_Insumo);
                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.Custo).HasColumnType("decimal(18,2)");

                // Configuração da chave estrangeira para TipoIngrediente
                entity.HasOne(e => e.TipoIngrediente)
                      .WithMany()
                      .HasForeignKey(e => e.IdTipoIngrediente)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FlavorizantesModel>(entity =>
            {
                entity.HasKey(e => e.Id_Flavorizante);
                entity.Property(e => e.Nome).IsRequired();
                entity.Property(e => e.Custo).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Codigo_Interno).IsRequired();
                entity.Property(e => e.Codigo_Fornecedor).IsRequired();
                entity.Property(e => e.Situacao).IsRequired().HasDefaultValue(false);

                // Configuração da chave estrangeira para TipoIngrediente
                entity.HasOne(e => e.Fornecedor)
                      .WithMany()
                      .HasForeignKey(e => e.Id_Fornecedor)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
