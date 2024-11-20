using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models
{
    // Representa o modelo da tabela tbl_receitas_insumos
    public class ReceitasInsumosModel
    {
        // Chave primária da tabela
        public int Id { get; set; }

        // Chave estrangeira para tbl_versoes_receitas (campo id)
        public int Id_Versao_Receita { get; set; }

        // Chave estrangeira para tbl_insumos (campo id)
        public int Id_Insumo { get; set; }

        // Unidade de medida do insumo (exemplo: "kg", "g")
        public string Unidade_Medida { get; set; }

        // Quantidade do insumo na receita
        public decimal Quantidade { get; set; }

        // Chave estrangeira para tbl_flavorizantes (campo id), opcional
        public int? Id_Flavorizante { get; set; }

        // Propriedade de navegação para a versão da receita
        public VersoesReceitasModel VersaoReceita { get; set; }

        // Propriedade de navegação para o insumo
        public InsumosModel Insumo { get; set; }

        // Propriedade de navegação para o flavorizante
        public FlavorizantesModel Flavorizante { get; set; }
    }
}
