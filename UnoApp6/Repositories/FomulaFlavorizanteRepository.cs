using System.Collections.Generic; // Permite usar coleções genéricas
using System.Linq; // Necessário para consultas LINQ
using System.Threading.Tasks; // Necessário para métodos assíncronos
using Microsoft.EntityFrameworkCore; // Usado para interagir com o banco de dados
using PeDJRMWinUI3UNO.Models; // Inclui o modelo de dados
using PeDJRMWinUI3UNO.Data; // Inclui o contexto do banco de dados

namespace PeDJRMWinUI3UNO.Repositories // Define o namespace para os repositórios
{
    /// <summary>
    /// Implementação do repositório para Fórmulas de Flavorizantes.
    /// </summary>
    public class FormulaFlavorizanteRepository : IFormulaFlavorizanteRepository
    {
        private readonly AppDbContext _context; // Contexto do banco de dados

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">O contexto do banco de dados.</param>
        public FormulaFlavorizanteRepository(AppDbContext context)
        {
            _context = context; // Define o contexto injetado
        }

        /// <summary>
        /// Obtém todas as fórmulas de flavorizantes.
        /// </summary>
        /// <returns>Uma coleção de todas as fórmulas.</returns>
        public async Task<IEnumerable<FormulaFlavorizanteModel>> ObterTodosAsync()
        {
            return await _context.FormulasFlavorizantes.ToListAsync(); // Retorna todas as fórmulas como uma lista
        }

        /// <summary>
        /// Obtém uma fórmula específica pelo ID.
        /// </summary>
        /// <param name="id">O ID da fórmula.</param>
        /// <returns>A fórmula correspondente ou nulo.</returns>
        public async Task<FormulaFlavorizanteModel?> ObterPorIdAsync(int id)
        {
            
            try
            {
                // Chama o repositório para buscar a receita pelo ID
                return await _context.FormulasFlavorizantes.FindAsync(id); // Busca uma fórmula pelo ID
            }
            catch (Exception ex)
            {
                // Log do erro
                Console.WriteLine($"Erro ao obter formula por ID no serviço: {ex.Message}");
                throw;
            }


        }

        /// <summary>
        /// Salva ou atualiza uma fórmula no banco de dados.
        /// </summary>
        /// <param name="formula">A fórmula a ser salva ou atualizada.</param>
        public async Task<int> SalvarAsync(FormulaFlavorizanteModel formula)
        {
            if (formula.Id == 0) // Verifica se é uma nova fórmula
            {
                await _context.FormulasFlavorizantes.AddAsync(formula); // Adiciona ao contexto
            }
            else
            {
                _context.FormulasFlavorizantes.Update(formula); // Atualiza o registro existente
            }

            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return formula.Id;
        }

        /// <summary>
        /// Exclui uma fórmula pelo ID.
        /// </summary>
        /// <param name="id">O ID da fórmula a ser excluída.</param>
        public async Task ExcluirAsync(int id)
        {
            var formula = await ObterPorIdAsync(id); // Busca a fórmula pelo ID
            if (formula != null) // Verifica se a fórmula foi encontrada
            {
                _context.FormulasFlavorizantes.Remove(formula); // Remove a fórmula do contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
            }
        }

        // Método para buscar fórmulas pelo nome
        public async Task<IEnumerable<FormulaFlavorizanteModel>> ObterPorNomeAsync(string nome)
        {
            // Verifica se o nome fornecido não é nulo ou vazio
            if (string.IsNullOrWhiteSpace(nome))
            {
                return new List<FormulaFlavorizanteModel>(); // Retorna uma lista vazia caso o nome seja inválido
            }

            // Realiza a busca no banco de dados considerando os campos relevantes
            return await _context.FormulasFlavorizantes
                .Where(f => f.Codigo_Flavorizante.Contains(nome) || // Filtra pelo código do flavorizante
                            f.Nome_Flavorizante.Contains(nome) || // Filtra pelo nome do flavorizante
                            (f.Descricao_Processo != null && f.Descricao_Processo.Contains(nome))) // Filtra pela descrição do processo
                .ToListAsync(); // Retorna a lista de resultados
        }

        // Método para obter o próximo ID disponível
        public async Task<int> ObterProximoIdFormulaAsync()
        {
            var ultimaFormula = await _context.FormulasFlavorizantes.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            return ultimaFormula?.Id + 1 ?? 1; // Se não houver registros, começa com 1            
        }


        public async Task<FormulaFlavorizanteModel> ObterPorCodigoAsync(string codigoFormula)
        {
            // Supondo que FormulaFlavorizante seja uma entidade do DbContext.
            return await _context.Set<FormulaFlavorizanteModel>()
                                 .FirstOrDefaultAsync(f => f.Codigo_Flavorizante == codigoFormula);
        }

    }
}
