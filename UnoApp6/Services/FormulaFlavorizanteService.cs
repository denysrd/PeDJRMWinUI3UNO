using System.Collections.Generic; // Permite usar coleções genéricas
using System.Diagnostics;
using System.Threading.Tasks; // Necessário para métodos assíncronos
using PeDJRMWinUI3UNO.Models; // Inclui o modelo de dados
using PeDJRMWinUI3UNO.Repositories; // Inclui o repositório

namespace PeDJRMWinUI3UNO.Services // Define o namespace para os serviços
{
    /// <summary>
    /// Serviço para gerenciamento de Fórmulas de Flavorizantes.
    /// </summary>
    public class FormulaFlavorizanteService
    {
        private readonly IFormulaFlavorizanteRepository _repository; // Repositório injetado
        private readonly IVersoesFormulaFlavorizanteRepository _versoesFormulasRepository;
        /// <summary>
        /// Construtor que injeta o repositório.
        /// </summary>
        /// <param name="repository">O repositório de Fórmulas de Flavorizantes.</param>
        public FormulaFlavorizanteService(IFormulaFlavorizanteRepository repository)
        {
            _repository = repository; // Define o repositório injetado
        }

        /// <summary>
        /// Obtém todas as fórmulas de flavorizantes.
        /// </summary>
        /// <returns>Uma coleção de todas as fórmulas.</returns>
        public async Task<IEnumerable<FormulaFlavorizanteModel>> ObterTodasFormulasAsync()
        {
            return await _repository.ObterTodosAsync(); // Retorna todas as fórmulas
        }

        /// <summary>
        /// Obtém uma fórmula específica pelo ID.
        /// </summary>
        /// <param name="id">O ID da fórmula.</param>
        /// <returns>A fórmula correspondente ou nulo.</returns>
        public async Task<FormulaFlavorizanteModel?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id); // Busca uma fórmula pelo ID
        }

        /// <summary>
        /// Salva ou atualiza uma fórmula.
        /// </summary>
        /// <param name="formula">A fórmula a ser salva ou atualizada.</param>
        public async Task SalvarAsync(FormulaFlavorizanteModel formula)
        {
            await _repository.SalvarAsync(formula); // Salva ou atualiza a fórmula
        }

        /// <summary>
        /// Exclui uma fórmula pelo ID.
        /// </summary>
        /// <param name="id">O ID da fórmula a ser excluída.</param>
        public async Task ExcluirAsync(int id)
        {
            await _repository.ExcluirAsync(id); // Exclui a fórmula pelo ID
        }

        public async Task<IEnumerable<VersoesFormulaFlavorizanteModel>> ObterVersoesAsync(int id)
        {
            // Valida o ID da receita.
            if (id <= 0)
            {
                throw new ArgumentException("O ID da formula fornecido é inválido.");
            }

            // Verifica se o repositório foi corretamente injetado.
            if (_versoesFormulasRepository == null)
            {
                throw new InvalidOperationException("O repositório de versões de formulas não está configurado.");
            }

            try
            {
                // Busca as versões no repositório.
                var versoes = await _versoesFormulasRepository.GetByFormulaIdAsync(id);

                // Verifica se as versões retornadas não são nulas.
                if (versoes == null)
                {
                    throw new InvalidOperationException($"Nenhuma versão encontrada para a formula com ID {id}.");
                }

                return versoes;
            }
            catch (Exception ex)
            {
                // Captura e registra exceções, se necessário.
                Debug.WriteLine($"Erro ao obter versões da receita: {ex.Message}");
                throw;
            }
        }

        // Método para buscar fórmulas pelo nome
        public async Task<IEnumerable<FormulaFlavorizanteModel>> ObterPorNomeAsync(string nome)
        {
            // Chama o repositório para buscar os dados
            return await _repository.ObterPorNomeAsync(nome);
        }


        public async Task<int> ObterProximoIdFormulaAsync()
        {
            return await _repository.ObterProximoIdFormulaAsync();
        }

        /// Obtém uma Formula pelo código.
        /// <param name="codigoFormula">Código da Formula.</param>
        /// <returns>Formula encontrada ou null.</returns>
        public async Task<FormulaFlavorizanteModel?> ObterFormulaPorCodigoAsync(string CodigoFormula)
        {
            return await _repository.ObterPorCodigoAsync(CodigoFormula);
        }

        /// Adiciona uma nova formula ao banco de dados.
        /// Verifica se os campos obrigatórios estão preenchidos e se a formula já existe antes de adicionar.
        /// <param name="formula">Objeto da formula que será adicionada.</param>
        /// <returns>O ID da formula adicionada.</returns>
        /// <exception cref="ArgumentException">Lançado se os campos obrigatórios não forem preenchidos.</exception>
        /// <exception cref="InvalidOperationException">Lançado se uma receita com o mesmo código já existir.</exception>
        public async Task<int> AdicionarFormulaAsync(FormulaFlavorizanteModel formula)
        {
            // Valida se o código da receita ou o nome da receita estão vazios ou nulos.
            // Caso estejam, lança uma exceção de argumento inválido.
            if (string.IsNullOrEmpty(formula.Codigo_Flavorizante) || string.IsNullOrEmpty(formula.Nome_Flavorizante))
                throw new ArgumentException("Campos obrigatórios não preenchidos.");

            // Tenta obter uma receita existente com o mesmo código da nova receita.
            var formulaExistente = await _repository.ObterPorCodigoAsync(formula.Codigo_Flavorizante);

            // Adiciona a nova receita ao banco de dados usando o repositório.
            // Retorna o ID da receita recém-adicionada.
            return await _repository.SalvarAsync(formula);
        }






    }
}
