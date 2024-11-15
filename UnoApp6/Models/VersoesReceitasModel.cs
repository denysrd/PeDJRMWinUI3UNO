using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeDJRMWinUI3UNO.Models
{
    /// <summary>
    /// Modelo para a tabela tbl_versoes_receitas.
    /// </summary>
    [Table("tbl_versoes_receitas")]
    public class VersoesReceitasModel
    {
        /// <summary>
        /// Identificador único da versão da receita.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Identificador da receita associada.
        /// </summary>
        [ForeignKey("Receita")]
        [Column("id_receita")]
        public int IdReceita { get; set; }

        /// <summary>
        /// Propriedade de navegação para a tabela tbl_receitas.
        /// </summary>
        public virtual ReceitasModel Receita { get; set; }

        /// <summary>
        /// Número da versão da receita.
        /// </summary>
        [Column("versao")]
        public string Versao { get; set; }

        /// <summary>
        /// Data associada à versão da receita.
        /// </summary>
        [Column("data")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Descrição do processo para a versão da receita.
        /// </summary>
        [Column("descricao_processo")]
        public string DescricaoProcesso { get; set; }
    }
}
