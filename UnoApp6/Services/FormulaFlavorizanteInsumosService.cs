using PeDJRMWinUI3UNO.Models; // Importa o namespace dos modelos
using PeDJRMWinUI3UNO.Repositories; // Importa o namespace dos repositórios
using System.Collections.Generic; // Permite o uso de coleções genéricas
using System.Threading.Tasks; // Permite a definição de métodos assíncronos

namespace PeDJRMWinUI3UNO.Services // Define o namespace do serviço
{
    /// <summary>
    /// Serviço para gerenciamento de insumos da fórmula flavorizante.
    /// </summary>
    public class FormulaFlavorizanteInsumosService
    {
        private readonly IFormulaFlavorizanteInsumosRepository _repository; // Instância do repositório

        // Construtor que recebe o repositório via injeção de dependência
        public FormulaFlavorizanteInsumosService(IFormulaFlavorizanteInsumosRepository repository)
        {
            _repository = repository; // Inicializa o repositório
        }

        // Método para obter todos os registros
        public async Task<IEnumerable<FormulaFlavorizanteInsumosModel>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync(); // Retorna todos os registros
        }

        // Método para obter um registro pelo ID
        public async Task<FormulaFlavorizanteInsumosModel?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id); // Retorna o registro pelo ID
        }

        // Método para adicionar um novo registro        
        public async Task<int> AdicionarAsync(FormulaFlavorizanteInsumosModel insumo)
        {
            // Valida o ID da versão da receita
            if (insumo.Id_Versao_Formula_Flavorizante <= 0)
            {
                throw new ArgumentException("ID da versão da Formula é obrigatório.");
            }

            // Adiciona o registro no banco de dados
            return await _repository.AdicionarAsync(insumo);
        }

        // Método para atualizar um registro existente
        public async Task AtualizarAsync(FormulaFlavorizanteInsumosModel insumo)
        {
            await _repository.AtualizarAsync(insumo); // Atualiza o registro existente
        }

        // Método para excluir um registro pelo ID
        public async Task ExcluirAsync(int id)
        {
            await _repository.ExcluirAsync(id); // Remove o registro pelo ID
        }

        // Obtém todos os insumos de uma versão específica de receita
        public async Task<IEnumerable<FormulaFlavorizanteInsumosModel>> ObterPorVersaoFormulaIdAsync(int idVersaoFormulaFlavorizante)
        {
            return await _repository.GetByVersaoFormulaIdAsync(idVersaoFormulaFlavorizante);
        }

    }
}
