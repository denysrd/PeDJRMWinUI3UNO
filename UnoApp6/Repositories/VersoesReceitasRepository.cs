using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories
{
    /// <summary>
    /// Repositório responsável pelo acesso à tabela tbl_versoes_receitas.
    /// </summary>
    public class VersoesReceitasRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do repositório.
        /// </summary>
        public VersoesReceitasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as versões de receitas.
        /// </summary>
        public async Task<List<VersoesReceitasModel>> GetAllAsync()
        {
            return await _context.VersoesReceitas
                .Include(v => v.Receita) // Inclui os dados da receita associada.
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma versão específica pelo ID.
        /// </summary>
        public async Task<VersoesReceitasModel> GetByIdAsync(int id)
        {
            return await _context.VersoesReceitas
                .Include(v => v.Receita)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        /// <summary>
        /// Adiciona uma nova versão de receita.
        /// </summary>
        public async Task AddAsync(VersoesReceitasModel versaoReceita)
        {
            _context.VersoesReceitas.Add(versaoReceita);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza uma versão de receita existente.
        /// </summary>
        public async Task UpdateAsync(VersoesReceitasModel versaoReceita)
        {
            _context.VersoesReceitas.Update(versaoReceita);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove uma versão de receita pelo ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var versaoReceita = await _context.VersoesReceitas.FindAsync(id);
            if (versaoReceita != null)
            {
                _context.VersoesReceitas.Remove(versaoReceita);
                await _context.SaveChangesAsync();
            }
        }
    }
}
