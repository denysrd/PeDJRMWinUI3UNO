using System.Collections.Generic; // Para trabalhar com listas e coleções
using System.Threading.Tasks; // Para trabalhar com métodos assíncronos
using PeDJRMWinUI3UNO.Models; // Referência ao modelo de versão da fórmula

namespace PeDJRMWinUI3UNO.Repositories // Namespace do repositório
{
    /// <summary>
    /// Interface para definir operações relacionadas às versões de fórmulas de flavorizantes.
    /// </summary>
    public interface IVersoesFormulaFlavorizanteRepository
    {
        // Método para obter todas as versões.
        Task<IEnumerable<VersoesFormulaFlavorizanteModel>> ObterTodasAsync();

        // Método para obter uma versão específica pelo ID.
        Task<VersoesFormulaFlavorizanteModel> ObterPorIdAsync(int id);

        // Método para adicionar uma nova versão.
        Task<int> AdicionarAsync(VersoesFormulaFlavorizanteModel versao);

        // Método para atualizar uma versão existente.
        Task AtualizarAsync(VersoesFormulaFlavorizanteModel versao);

        // Método para remover uma versão pelo ID.
        Task RemoverAsync(int id);

        // Método para obter todas as versões de uma formula.
        Task<IEnumerable<VersoesFormulaFlavorizanteModel>> GetByFormulaIdAsync(int id);

    }
}
