using System;

public class ReceitasModel
{
    public int Id { get; set; } // PK
    public string Codigo_Receita { get; set; } // UNIQUE, NOT NULL
    public string Nome_Receita { get; set; } // NOT NULL
    public DateTime Data { get; set; } // DATE
    public string Descricao_Processo { get; set; } // VARCHAR(1000)
}
