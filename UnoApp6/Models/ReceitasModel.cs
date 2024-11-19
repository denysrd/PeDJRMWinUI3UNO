using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models
{
    /// Modelo para a tabela tbl_receitas.
    [Table("tbl_receitas")]
    public class ReceitasModel
    {        
        /// Identificador único da receita (id).
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// Código único da receita (codigo_receita).
        [Required]
        [Column("codigo_receita")]
        public string Codigo_Receita { get; set; }

        /// Nome da receita (nome_receita).
        [Required]
        [Column("nome_receita")]
        public string Nome_Receita { get; set; }

        /// Data de criação ou modificação da receita (data).
        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        /// Descrição do processo da receita (descricao_processo).
        [Column("descricao_processo")]
        public string Descricao_Processo { get; set; }
    }
}
