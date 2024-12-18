using System.Collections.Generic; // Para trabalhar com listas e coleções
using System.Linq; // Para manipulações de coleções
using System.Threading.Tasks; // Para métodos assíncronos
using Microsoft.EntityFrameworkCore; // Para interação com o banco de dados
using PeDJRMWinUI3UNO.Models; // Modelo de dados
using PeDJRMWinUI3UNO.Data; // Contexto do banco de dados

namespace PeDJRMWinUI3UNO.Repositories // Namespace do repositório
{
    /// <summary>
    /// Implementação do repositório para gerenciar versões de fórmulas de flavorizantes.
    /// </summary>
    public class VersoesFormulaFlavorizanteRepository : IVersoesFormulaFlavorizanteRepository
    {
        // Contexto do banco de dados
        private readonly AppDbContext _dbContext; // Representa o contexto do Entity Framework

        /// <summary>
        /// Construtor para inicializar o repositório com o contexto do banco de dados.
        /// </summary>
        public VersoesFormulaFlavorizanteRepository(AppDbContext context)
        {
            _dbContext = context; // Injeta o contexto no repositório
        }

        /// <summary>
        /// Obtém todas as versões de fórmulas de flavorizantes.
        /// </summary>
        public async Task<IEnumerable<VersoesFormulaFlavorizanteModel>> ObterTodasAsync()
        {
            return await _dbContext.VersoesFormulaFlavorizante.ToListAsync(); // Retorna todas as versões
        }

        /// <summary>
        /// Obtém uma versão específica pelo ID.
        /// </summary>
        public async Task<VersoesFormulaFlavorizanteModel> ObterPorIdAsync(int id)
        {
            return await _dbContext.VersoesFormulaFlavorizante.FindAsync(id); // Retorna a versão pelo ID
        }

        /// <summary>
        /// Adiciona uma nova versão.
        /// </summary>
        public async Task<int> AdicionarAsync(VersoesFormulaFlavorizanteModel versao)
        {
            await _dbContext.VersoesFormulaFlavorizante.AddAsync(versao); // Adiciona a nova versão
            await _dbContext.SaveChangesAsync(); // Salva as alterações
            return versao.Id;
        }

        /// <summary>
        /// Atualiza uma versão existente.
        /// </summary>
        public async Task AtualizarAsync(VersoesFormulaFlavorizanteModel versao)
        {
            _dbContext.VersoesFormulaFlavorizante.Update(versao); // Atualiza a versão
            await _dbContext.SaveChangesAsync(); // Salva as alterações
        }

        /// <summary>
        /// Remove uma versão pelo ID.
        /// </summary>
        public async Task RemoverAsync(int id)
        {
            var versao = await ObterPorIdAsync(id); // Busca a versão pelo ID
            if (versao != null)
            {
                _dbContext.VersoesFormulaFlavorizante.Remove(versao); // Remove a versão
                await _dbContext.SaveChangesAsync(); // Salva as alterações
            }
        }

        public async Task<IEnumerable<VersoesFormulaFlavorizanteModel>> GetByFormulaIdAsync(int id)
        {
            // Obtém todas as versões associadas a uma receita específica
            return await _dbContext.VersoesFormulaFlavorizante
                .Where(v => v.Id == id)
                .ToListAsync();
        }


    }
}
