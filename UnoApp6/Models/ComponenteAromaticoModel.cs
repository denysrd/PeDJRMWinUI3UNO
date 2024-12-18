using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace PeDJRMWinUI3UNO.Models
{
    [Table("tbl_componente_aromatico")]
    public class ComponenteAromaticoModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(45)]
        public string Nome { get; set; }

        [Column("custo", TypeName = "decimal(10,0)")]
        public decimal? Custo { get; set; }

        [Column("CAS")]
        [MaxLength(45)]
        public string? CAS { get; set; }

        [Column("fema")]
        [MaxLength(45)]
        public string? Fema { get; set; }

        [Column("descricao")]
        [MaxLength(1000)]
        public string? Descricao { get; set; }

        [Column("link_referencia")]
        [MaxLength(1000)]
        public string? LinkReferencia { get; set; }

        [Column("nomenclatura_en")]
        [MaxLength(1000)]
        public string? NomenclaturaEn { get; set; }

        [Column("data_recebimento")]
        public DateTime? DataRecebimento { get; set; }

        [Column("data_cadastro")]
        public DateTime? DataCadastro { get; set; }

        [Required]
        [Column("codigo_interno")]
        [MaxLength(45)]
        public string CodigoInterno { get; set; }

        [Required]
        [Column("id_fornecedor")]
        public int IdFornecedor { get; set; }

        [Column("codigo_produto_fornecedor")]
        [MaxLength(45)]
        public string? CodigoProdutoFornecedor { get; set; }

        [Column("idTipoIngrediente")]
        public int? IdTipoIngrediente { get; set; }

        [Required]
        [Column("situacao")]
        public bool Situacao { get; set; }

        // Propriedades de navegação
        [ForeignKey("IdFornecedor")]
        public FornecedorModel? Fornecedor { get; set; }

        [ForeignKey("IdTipoIngrediente")]
        public TipoIngredienteModel? TipoIngrediente { get; set; }
    }
}
