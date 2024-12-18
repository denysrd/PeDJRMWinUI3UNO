using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace PeDJRMWinUI3UNO.Models
{
    // Define a tabela correspondente no banco de dados
    [Table("tbl_fomula_flavorizante")]
    public class FormulaFlavorizanteModel
    {
        // Define a chave primária da tabela
        [DataAnnotations.Key] // Usa o alias definido
        // ID não é gerado automaticamente
        public int Id { get; set; }

        // Define o código do flavorizante como obrigatório e limita o tamanho a 45 caracteres
        [Required, MaxLength(45)]
        public string Codigo_Flavorizante { get; set; }

        // Define o nome do flavorizante como obrigatório e limita o tamanho a 45 caracteres
        [Required, MaxLength(45)]
        public string Nome_Flavorizante { get; set; }

        // Define a data de criação ou atualização como obrigatória
        [Required]
        public DateTime Data { get; set; }

        // Campo opcional para a descrição do processo, limitado a 45 caracteres
        [MaxLength(45)]
        public string? Descricao_Processo { get; set; }

        // Relacionamento 1:N com as versões da fórmula
        public ICollection<VersoesFormulaFlavorizanteModel> VersoesFormulas { get; set; }
    }
}
