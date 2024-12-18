using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;
using Microsoft.Extensions.Logging;

namespace PeDJRMWinUI3UNO.Data;

public class AppDbContext : DbContext
{
    // Construtor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Defina os DbSets para cada entidade
    public DbSet<FornecedorModel> FornecedorModel { get; set; }
    public DbSet<TipoIngredienteModel> TipoIngredientes { get; set; }
    public DbSet<TipoFormulacaoModel> TipoFormulacaoModel { get; set; }
    public DbSet<InsumoModel> InsumoModel { get; set; }
    public DbSet<FlavorizantesModel> FlavorizantesModel { get; set; }
    public DbSet<ReceitasModel> ReceitasModel { get; set; }
    public DbSet<VersoesReceitasModel> VersoesReceitas { get; set; }
    public DbSet<ReceitasInsumosModel> ReceitasInsumos { get; set; }       
    public DbSet<FormulaFlavorizanteModel> FormulasFlavorizantes { get; set; }
    public DbSet<ComponenteAromaticoModel> ComponentesAromaticosModel { get; set; }
    public DbSet<FormulaFlavorizanteInsumosModel> FormulaFlavorizanteInsumos { get; set; }
    public DbSet<VersoesFormulaFlavorizanteModel> VersoesFormulaFlavorizante { get; set; }


    // Configuração condicional do DbContext
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            const string connectionString = "Server=localhost;Database=db_jrmsistema;User=root;Password=Neoo@@141189;";
            optionsBuilder
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 02)))
                .UseLoggerFactory(LoggerFactoryInstance)
                .EnableSensitiveDataLogging() // Exibe os valores das consultas (use com cuidado)
                .EnableDetailedErrors()                    ; // Exibe detalhes adicionais sobre erros
        }
    }

    // Define uma propriedade LoggerFactory para exibir logs
    private static readonly ILoggerFactory LoggerFactoryInstance = LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole() // Adiciona logs ao console
            .SetMinimumLevel(LogLevel.Information); // Define o nível mínimo de log
    });

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

        modelBuilder.Entity<InsumoModel>(entity =>
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
        
       
        
        modelBuilder.Entity<ReceitasModel>(entity =>
        {
            entity.ToTable("Tbl_receitas");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Codigo_Receita).HasMaxLength(45).IsRequired();
            entity.Property(r => r.Nome_Receita).HasMaxLength(45).IsRequired();
            entity.Property(r => r.Data).IsRequired();
            entity.Property(r => r.Descricao_Processo).HasMaxLength(1000);
        });

        modelBuilder.Entity<VersoesReceitasModel>(entity =>
        {
            entity.ToTable("tbl_versoes_receitas"); // Nome da tabela
            entity.HasKey(v => v.Id); // Define a chave primária
            entity.Property(v => v.Versao).IsRequired(); // Campo obrigatório
            entity.Property(v => v.Data).IsRequired(); // Campo obrigatório
            entity.Property(v => v.Descricao_Processo).HasMaxLength(1000); // Limite de caracteres

            // Define o relacionamento com tbl_receitas
            entity.HasOne(v => v.Receita)
                  .WithMany(r => r.VersoesReceitas) // Uma receita pode ter muitas versões
                  .HasForeignKey(v => v.Id_Receita) // Chave estrangeira
                  .OnDelete(DeleteBehavior.Cascade); // Comportamento ao excluir a receita
        });

        modelBuilder.Entity<ReceitasInsumosModel>(entity =>
        {
            entity.ToTable("tbl_receitas_insumos"); // Define o nome da tabela
            entity.HasKey(ri => ri.Id); // Define a chave primária
            entity.Property(ri => ri.Unidade_Medida).HasMaxLength(45); // Limite de caracteres para a unidade de medida
            entity.Property(ri => ri.Quantidade).IsRequired(); // Campo obrigatório
            entity.Property(ri => ri.Id_Insumo).HasColumnName("Id_Insumo");
            entity.Property(ri => ri.Id_Flavorizante).HasColumnName("Id_Flavorizante");

            // Relacionamento com tbl_versoes_receitas
            entity.HasOne(ri => ri.VersaoReceita)
                  .WithMany() // Sem coleção configurada em VersoesReceitasModel
                  .HasForeignKey(ri => ri.Id_Versao_Receita)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com tbl_insumos
            entity.HasOne(ri => ri.Insumo)
                  .WithMany() // Sem coleção configurada em InsumosModel
                  .HasForeignKey(ri => ri.Id_Insumo)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com tbl_flavorizantes
            entity.HasOne(ri => ri.Flavorizante)
                  .WithMany() // Sem coleção configurada em FlavorizantesModel
                  .HasForeignKey(ri => ri.Id_Flavorizante)
                  .OnDelete(DeleteBehavior.Restrict);
        });
               
       

        modelBuilder.Entity<FlavorizantesModel>(entity =>
        {
            entity.ToTable("tbl_flavorizante");

            entity.HasKey(f => f.Id_Flavorizante);

            entity.Property(f => f.Id_Flavorizante).HasColumnName("id_flavorizante");
            entity.Property(f => f.Nome).HasColumnName("nome");
            entity.Property(f => f.Codigo_Interno).HasColumnName("codigo_interno");
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

        modelBuilder.Entity<ItemModel>(builder =>
        {
            builder.HasNoKey(); // Define ItemModel como sem chave primária
        });

        // Configuração para FormulaFlavorizanteModel
        modelBuilder.Entity<FormulaFlavorizanteModel>(entity =>
        {
            entity.ToTable("tbl_fomula_flavorizante"); // Define o nome da tabela
            entity.HasKey(e => e.Id); // Define a chave primária

            entity.Property(e => e.Codigo_Flavorizante)
                .IsRequired()
                .HasMaxLength(45); // Define como obrigatório e limita o tamanho

            entity.Property(e => e.Nome_Flavorizante)
                .IsRequired()
                .HasMaxLength(45);

            entity.Property(e => e.Data)
                .IsRequired(); // Define como obrigatório

            entity.Property(e => e.Descricao_Processo)
                .HasMaxLength(45); // Define como opcional, limite de 45 caracteres
        });

        // Configuração para VersoesFormulaFlavorizanteModel
        modelBuilder.Entity<VersoesFormulaFlavorizanteModel>(entity =>
        {
            entity.ToTable("tbl_versoes_formula_flavorizante"); // Nome da tabela
            entity.HasKey(e => e.Id); // Define a chave primária

            entity.Property(e => e.Versao)
                .IsRequired(); // Define como obrigatório

            entity.Property(e => e.Data)
                .IsRequired();

            entity.Property(e => e.Descricao_Processo)
                .HasMaxLength(45); // Define tamanho máximo

            // Configuração da chave estrangeira com FormulaFlavorizanteModel
            entity.HasOne(e => e.FormulaFlavorizante)
                  .WithMany(f => f.VersoesFormulas)
                  .HasForeignKey(e => e.Id_Formula_Flavorizante)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração para FormulaFlavorizanteInsumosModel
        modelBuilder.Entity<FormulaFlavorizanteInsumosModel>(entity =>
        {
            entity.ToTable("tbl_formula_flavorizante_insumos"); // Define o nome da tabela
            entity.HasKey(e => e.Id); // Define a chave primária

            entity.Property(e => e.Unidade_Medida)
                .HasMaxLength(45); // Define tamanho máximo

            entity.Property(e => e.Quantidade)
                .IsRequired(); // Campo obrigatório

            // Relacionamento com VersoesFormulaFlavorizanteModel
            entity.HasOne(e => e.VersaoFormulaFlavorizante)
                  .WithMany()
                  .HasForeignKey(e => e.Id_Versao_Formula_Flavorizante)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com InsumoModel
            entity.HasOne(e => e.Insumo)
                  .WithMany()
                  .HasForeignKey(e => e.Id_Insumo)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com ComponenteAromaticoModel
            entity.HasOne(e => e.ComponenteAromatico)
                  .WithMany()
                  .HasForeignKey(e => e.Id_Car)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração para ComponenteAromaticoModel
        modelBuilder.Entity<ComponenteAromaticoModel>(entity =>
        {
            entity.ToTable("tbl_componente_aromatico");

            // Definição da chave primária
            entity.HasKey(e => e.Id);

            // Propriedades da tabela
            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("nome");

            entity.Property(e => e.Custo)
                .HasColumnType("decimal(10,0)")
                .HasColumnName("custo");

            entity.Property(e => e.CAS)
                .HasMaxLength(45)
                .HasColumnName("CAS");

            entity.Property(e => e.Fema)
                .HasMaxLength(45)
                .HasColumnName("fema");

            entity.Property(e => e.Descricao)
                .HasMaxLength(1000)
                .HasColumnName("descricao");

            entity.Property(e => e.LinkReferencia)
                .HasMaxLength(1000)
                .HasColumnName("link_referencia");

            entity.Property(e => e.NomenclaturaEn)
                .HasMaxLength(1000)
                .HasColumnName("nomenclatura_en");

            entity.Property(e => e.DataRecebimento)
                .HasColumnName("data_recebimento");

            entity.Property(e => e.DataCadastro)
                .HasColumnName("data_cadastro");

            entity.Property(e => e.CodigoInterno)
                .IsRequired()
                .HasMaxLength(45)
                .HasColumnName("codigo_interno");

            entity.Property(e => e.IdFornecedor)
                .IsRequired()
                .HasColumnName("id_fornecedor");

            entity.Property(e => e.CodigoProdutoFornecedor)
                .HasMaxLength(45)
                .HasColumnName("codigo_produto_fornecedor");

            entity.Property(e => e.IdTipoIngrediente)
                .HasColumnName("idTipoIngrediente");

            entity.Property(e => e.Situacao)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("situacao");

            // Relacionamentos
            entity.HasOne(e => e.Fornecedor)
                .WithMany()
                .HasForeignKey(e => e.IdFornecedor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fornecedor_id");

            entity.HasOne(e => e.TipoIngrediente)
                .WithMany()
                .HasForeignKey(e => e.IdTipoIngrediente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("idTipoIngrediente");
        });

        // Configuração para InsumoModel       
        modelBuilder.Entity<InsumoModel>(entity =>
        {
            entity.ToTable("tbl_insumo");
            entity.HasKey(i => i.Id_Insumo);

            entity.Property(i => i.Id_Insumo).HasColumnName("id_insumo");
            entity.Property(i => i.Nome).HasColumnName("nome");
            entity.Property(i => i.Codigo_Interno).HasColumnName("codigo_interno");
        });








        // Configuração para FornecedorModel
        modelBuilder.Entity<FornecedorModel>(entity =>
        {
            entity.ToTable("tbl_fornecedor"); // Nome da tabela
            entity.HasKey(e => e.Id_Fornecedor); // Define a chave primária

            entity.Property(e => e.Nome)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Documento)
                  .IsRequired()
                  .HasMaxLength(50);
        });


    }
}
