// Namespace: PeDJRMWinUI3UNO.Models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
namespace PeDJRMWinUI3UNO.Models
{
    [Table("tbl_flavorizante")]
    public class FlavorizantesModel
    {
        [Key]
        public int Id_Flavorizante { get; set; }

        public string Nome { get; set; }

        public decimal Custo { get; set; }

        public string Codigo_Interno { get; set; }

        public string Codigo_Fornecedor { get; set; }

        [Required]
        public bool Situacao { get; set; } // 0 para inativo, 1 para ativo

        // Chave estrangeira para TipoIngrediente
        public int Id_Fornecedor { get; set; }

        // Propriedade de navegação para TipoIngredienteModel
        [ForeignKey("Id_Fornecedor")]
        public FornecedorModel Fornecedor { get; set; }

    }
}
