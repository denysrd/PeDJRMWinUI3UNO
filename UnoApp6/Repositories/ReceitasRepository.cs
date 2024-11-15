using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories
{
    /// <summary>
    /// Repositório responsável pelo acesso à tabela tbl_receitas.
    /// </summary>
    public class ReceitasRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do repositório ReceitasRepository.
        /// </summary>
        public ReceitasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as receitas.
        /// </summary>
        public async Task<List<ReceitasModel>> GetAllAsync()
        {
            return await _context.Receitas.ToListAsync();
        }

        /// <summary>
        /// Obtém uma receita pelo ID.
        /// </summary>
        public async Task<ReceitasModel> GetByIdAsync(int id)
        {
            return await _context.Receitas.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Adiciona uma nova receita.
        /// </summary>
        public async Task AddAsync(ReceitasModel receita)
        {
            _context.Receitas.Add(receita);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza uma receita existente.
        /// </summary>
        public async Task UpdateAsync(ReceitasModel receita)
        {
            _context.Receitas.Update(receita);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove uma receita pelo ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var receita = await _context.Receitas.FindAsync(id);
            if (receita != null)
            {
                _context.Receitas.Remove(receita);
                await _context.SaveChangesAsync();
            }
        }
    }
}
