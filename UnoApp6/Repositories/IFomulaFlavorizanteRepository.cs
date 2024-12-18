using System.Collections.Generic; // Permite usar coleções genéricas como List
using System.Threading.Tasks; // Necessário para métodos assíncronos

namespace PeDJRMWinUI3UNO.Repositories // Define o namespace para os repositórios
{
    /// <summary>
    /// Interface para o repositório de Fórmulas de Flavorizantes.
    /// </summary>
    public interface IFormulaFlavorizanteRepository
    {
        // Método para obter todas as fórmulas de flavorizantes
        Task<IEnumerable<FormulaFlavorizanteModel>> ObterTodosAsync(); // Retorna uma coleção de todas as fórmulas

        // Método para obter uma fórmula específica pelo ID
        Task<FormulaFlavorizanteModel?> ObterPorIdAsync(int id); // Retorna uma fórmula específica pelo ID ou nulo se não encontrado

        // Método para salvar uma nova fórmula no banco de dados
        Task<int> SalvarAsync(FormulaFlavorizanteModel formula); // Salva ou atualiza uma fórmula no banco de dados

        // Método para excluir uma fórmula do banco de dados
        Task ExcluirAsync(int id); // Exclui uma fórmula pelo ID

        // Método para buscar fórmulas pelo nome
        Task<IEnumerable<FormulaFlavorizanteModel>> ObterPorNomeAsync(string nome);
        Task<int> ObterProximoIdFormulaAsync();
        Task<FormulaFlavorizanteModel> ObterPorCodigoAsync(string codigoFormula);

        

    }
}
