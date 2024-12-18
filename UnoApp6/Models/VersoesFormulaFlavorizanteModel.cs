using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace PeDJRMWinUI3UNO.Models
{
    // Define a tabela correspondente no banco de dados
    [Table("tbl_versoes_formula_flavorizante")]
    public class VersoesFormulaFlavorizanteModel
    {
        // Define a chave primária com geração automática de valores
        [DataAnnotations.Key] // Usa o alias definido
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Chave estrangeira que referencia a tabela tbl_fomula_flavorizante
        [Required]
        public int Id_Formula_Flavorizante { get; set; }

        // Define a versão da fórmula como obrigatória
        [Required]
        public decimal Versao { get; set; }

        // Define a data como obrigatória
        [Required]
        public DateTime Data { get; set; }

        // Define a descrição do processo como opcional, com limite de 45 caracteres
        [MaxLength(45)]
        public string? Descricao_Processo { get; set; }

        // Relacionamento com FormulaFlavorizanteModel (Chave estrangeira)
        [ForeignKey("Id_Formula_Flavorizante")]
        public FormulaFlavorizanteModel FormulaFlavorizante { get; set; }

        // Relacionamento 1:N com os insumos
        public ICollection<FormulaFlavorizanteInsumosModel> Insumos { get; set; }

        // Propriedade para armazenar os itens da versão
        [System.ComponentModel.DataAnnotations.Schema.NotMapped] // Ignorado pelo EF
        public ObservableCollection<ItemModel> Itens { get; set; } = new ObservableCollection<ItemModel>();
    }
}
