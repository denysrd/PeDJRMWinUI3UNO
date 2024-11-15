using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models
{
    /// <summary>
    /// Modelo para a tabela tbl_receitas_flavorizantes.
    /// </summary>
    [Table("tbl_receitas_flavorizantes")]
    public class ReceitasFlavorizantesModel
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
        public virtual VersoesReceitasModel VersaoReceita { get; set; }

        /// <summary>
        /// Identificador do flavorizante associado (id_flavorizante).
        /// </summary>
        [Column("id_flavorizante")]
        public int Id_Flavorizante { get; set; }

        /// <summary>
        /// Unidade de medida do flavorizante (unidade_medida).
        /// </summary>
        [Column("unidade_medida")]
        public string Unidade_Medida { get; set; }

        /// <summary>
        /// Quantidade do flavorizante (quantidade).
        /// </summary>
        [Column("quantidade")]
        public double Quantidade { get; set; }
    }
}
