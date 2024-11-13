namespace PeDJRMWinUI3UNO.Models;

public class TipoIngredienteModel
{
    // Identificador único do tipo de ingrediente
    public int Id_Tipo_Ingrediente { get; set; }

    // Nome do tipo de ingrediente
    public string Tipo_Ingrediente { get; set; }

    // Descrição do tipo de ingrediente
    public string Descricao_Tipo_Ingrediente { get; set; }

    // Situação do tipo de ingrediente (ativo/inativo)
    public bool Situacao { get; set; } = false;

    // Sigla do tipo de ingrediente
    public string Sigla { get; set; }
}
