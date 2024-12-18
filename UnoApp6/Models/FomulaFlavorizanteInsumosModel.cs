using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace PeDJRMWinUI3UNO.Models
{
    // Define a tabela correspondente no banco de dados
    [Table("tbl_formula_flavorizante_insumos")]
    public class FormulaFlavorizanteInsumosModel
    {
        // Define a chave primária com geração automática
        [DataAnnotations.Key] // Usa o alias definido
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Chave estrangeira que referencia tbl_versoes_formula_flavorizante
        [Required]
        public int Id_Versao_Formula_Flavorizante { get; set; }

        // Chave estrangeira que referencia tbl_insumo
        [Required]
        public int Id_Insumo { get; set; }

        // Chave estrangeira que referencia tbl_componente_aromatico
        [Required]
        public int Id_Car { get; set; }

        // Define a unidade de medida como opcional, com limite de 45 caracteres
        [MaxLength(45)]
        public string? Unidade_Medida { get; set; }

        // Define a quantidade como obrigatória
        [Required]
        public decimal Quantidade { get; set; }

        // Relacionamento com VersoesFormulaFlavorizanteModel
        [ForeignKey("Id_Versao_Formula_Flavorizante")]
        public VersoesFormulaFlavorizanteModel VersaoFormulaFlavorizante { get; set; }

        // Relacionamento com InsumoModel
        [ForeignKey("Id_Insumo")]
        public InsumoModel Insumo { get; set; }

        // Relacionamento com ComponenteAromaticoModel
        [ForeignKey("Id_Car")]
        public ComponenteAromaticoModel ComponenteAromatico { get; set; }
    }
}
