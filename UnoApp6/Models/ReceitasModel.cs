using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeDJRMWinUI3UNO.Models;


[Table("tbl_receitas")] // Mapeia a classe para a tabela correta
public class ReceitasModel
{
    public int Id { get; set; } // PK
    [Column("codigo_receita")] // Mapeia para o campo correto no banco de dados
    public string Codigo_Receita { get; set; } // UNIQUE, NOT NULL
    public string Nome_Receita { get; set; } // NOT NULL
    public DateTime Data { get; set; } // DATE
    public string Descricao_Processo { get; set; } // VARCHAR(1000)

    public ObservableCollection<VersoesReceitasModel> VersoesReceitas { get; set; } = new ObservableCollection<VersoesReceitasModel>();

}
