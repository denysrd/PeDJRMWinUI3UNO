using System.Collections.Generic; // Para trabalhar com listas e coleções
using System.Threading.Tasks; // Para métodos assíncronos
using PeDJRMWinUI3UNO.Models; // Modelo de dados
using PeDJRMWinUI3UNO.Repositories; // Repositório

namespace PeDJRMWinUI3UNO.Services // Namespace do serviço
{
    /// <summary>
    /// Serviço para gerenciar operações relacionadas às versões de fórmulas de flavorizantes.
    /// </summary>
    public class VersoesFormulaFlavorizanteService
    {
        private readonly IVersoesFormulaFlavorizanteRepository _repository; // Referência ao repositório

        /// <summary>
        /// Construtor para inicializar o serviço com o repositório.
        /// </summary>
        public VersoesFormulaFlavorizanteService(IVersoesFormulaFlavorizanteRepository repository)
        {
            _repository = repository; // Injeta o repositório no serviço
        }

        /// <summary>
        /// Obtém todas as versões de fórmulas de flavorizantes.
        /// </summary>
        public async Task<IEnumerable<VersoesFormulaFlavorizanteModel>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync(); // Chama o método do repositório
        }

        /// <summary>
        /// Obtém uma versão específica pelo ID.
        /// </summary>
        public async Task<VersoesFormulaFlavorizanteModel> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id); // Chama o método do repositório
        }

        /// <summary>
        /// Adiciona uma nova versão.
        /// </summary>
        /// 

        public async Task<int> AdicionarVersaoAsync(VersoesFormulaFlavorizanteModel versaoFormula)
        {
            if (versaoFormula.Id_Formula_Flavorizante <= 0)
            {
                throw new ArgumentException("ID do flavorizante é obrigatório."); // Valida o ID do flavorizante
            }

            if (versaoFormula.Versao <= 0)
            {
                throw new ArgumentException("A versão deve ser maior que zero."); // Valida o número da versão
            }

            return await _repository.AdicionarAsync(versaoFormula); // Adiciona a versão ao banco
        }




        public async Task AdicionarAsync(VersoesFormulaFlavorizanteModel versao)
        {
            await _repository.AdicionarAsync(versao); // Chama o método do repositório
        }


        /// <summary>
        /// Atualiza uma versão existente.
        /// </summary>
        public async Task AtualizarAsync(VersoesFormulaFlavorizanteModel versao)
        {
            await _repository.AtualizarAsync(versao); // Chama o método do repositório
        }

        /// <summary>
        /// Remove uma versão pelo ID.
        /// </summary>
        public async Task RemoverAsync(int id)
        {
            await _repository.RemoverAsync(id); // Chama o método do repositório
        }

        public async Task<IEnumerable<VersoesFormulaFlavorizanteModel>> ObterPorFormulaIdAsync(int idFormula)
        {
            return await _repository.GetByFormulaIdAsync(idFormula); // Obtém todas as versões de uma Formula
        }
               
    }
}
