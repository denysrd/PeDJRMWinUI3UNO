// Namespace: PeDJRMWinUI3UNO.Models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models
{
    [Table("tbl_insumo")]
    public class InsumoModel
    {
        [Key]
        public int Id_Insumo { get; set; }

        public string Nome { get; set; }

        public decimal Custo { get; set; }

        public string Codigo_Interno { get; set; }

        public string Descricao_Insumo { get; set; }

        public string codigo_produto_fornecedor { get; set; }

        public int Id_Fornecedor { get; set; }

        [Required]
        public bool Situacao { get; set; } // 0 para inativo, 1 para ativo

        // Chave estrangeira para TipoIngrediente
        public int IdTipoIngrediente { get; set; }

        // Propriedade de navegação para TipoIngredienteModel
        [ForeignKey("IdTipoIngrediente")]
        public TipoIngredienteModel TipoIngrediente { get; set; }
    }
}
