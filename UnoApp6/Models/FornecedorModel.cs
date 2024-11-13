// Namespace: PeDJRMWinUI3UNO.Models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models;

[Table("tbl_fornecedor")]
public class FornecedorModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id_Fornecedor { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    public string Documento { get; set; }

    public string Email { get; set; }

    public string Telefone { get; set; }

    [Required]
    public bool Situacao { get; set; } // 0 para inativo, 1 para ativo
}
