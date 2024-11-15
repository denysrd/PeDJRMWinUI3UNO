using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeDJRMWinUI3UNO.Models
{
    /// <summary>
    /// Modelo para a tabela tbl_receitas_insumos.
    /// </summary>
    [Table("tbl_receitas_insumos")]
    public class ReceitasInsumosModel
    {
        /// <summary>
        /// Identificador único do registro (id).
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Identificador da versão da receita associada (id_versao_receita).
        /// </summary>
        [ForeignKey("VersaoReceita")]
        [Column("id_versao_receita")]
        public int Id_Versao_Receita { get; set; }

        /// <summary>
        /// Propriedade de navegação para a versão da receita.
        /// </summary>
        public virtual VersoesReceitasModel Versao_Receita { get; set; }

        /// <summary>
        /// Identificador do insumo associado (id_insumo).
        /// </summary>
        [Column("id_insumo")]
        public int Id_Insumo { get; set; }

        /// <summary>
        /// Unidade de medida do insumo (unidade_medida).
        /// </summary>
        [Column("unidade_medida")]
        public string Unidade_Medida { get; set; }

        /// <summary>
        /// Quantidade do insumo (quantidade).
        /// </summary>
        [Column("quantidade")]
        public double Quantidade { get; set; }
    }
}
