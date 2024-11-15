using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeDJRMWinUI3UNO.Models
{
    /// <summary>
    /// Modelo para a tabela tbl_receitas.
    /// </summary>
    [Table("tbl_receitas")]
    public class ReceitasModel
    {
        /// <summary>
        /// Identificador único da receita (id).
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Código único da receita (codigo_receita).
        /// </summary>
        [Required]
        [Column("codigo_receita")]
        public string Codigo_Receita { get; set; }

        /// <summary>
        /// Nome da receita (nome_receita).
        /// </summary>
        [Required]
        [Column("nome_receita")]
        public string Nome_Receita { get; set; }

        /// <summary>
        /// Data de criação ou modificação da receita (data).
        /// </summary>
        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Descrição do processo da receita (descricao_processo).
        /// </summary>
        [Column("descricao_processo")]
        public string Descricao_Processo { get; set; }
    }
}
