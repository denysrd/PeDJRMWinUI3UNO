using PeDJRMWinUI3UNO.Models; // Importa o namespace dos modelos
using System.Collections.Generic; // Permite o uso de coleções genéricas
using System.Threading.Tasks; // Permite a definição de métodos assíncronos

namespace PeDJRMWinUI3UNO.Repositories // Define o namespace do repositório
{
    /// <summary>
    /// Interface para o repositório de insumos da fórmula flavorizante.
    /// </summary>
    public interface IFormulaFlavorizanteInsumosRepository
    {
        // Método para obter todos os insumos
        Task<IEnumerable<FormulaFlavorizanteInsumosModel>> ObterTodosAsync(); // Retorna todos os registros da tabela

        // Método para obter um insumo pelo ID
        Task<FormulaFlavorizanteInsumosModel?> ObterPorIdAsync(int id); // Retorna um registro específico pelo ID

        // Método para adicionar um novo insumo
        Task<int> AdicionarAsync(FormulaFlavorizanteInsumosModel insumo); // Adiciona um novo registro

        // Método para atualizar um insumo existente
        Task AtualizarAsync(FormulaFlavorizanteInsumosModel insumo); // Atualiza um registro existente

        // Método para excluir um insumo pelo ID
        Task ExcluirAsync(int id); // Remove um registro pelo ID

        // Obtém todos os insumos de uma versão específica de formula
        Task<IEnumerable<FormulaFlavorizanteInsumosModel>> GetByVersaoFormulaIdAsync(int idVersaoFormulaFlavorizante); 
    }
}
