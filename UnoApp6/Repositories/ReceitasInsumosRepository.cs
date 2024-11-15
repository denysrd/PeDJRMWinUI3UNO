using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories
{
    /// <summary>
    /// Repositório responsável pelo acesso à tabela tbl_receitas_insumos.
    /// </summary>
    public class ReceitasInsumosRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do repositório ReceitasInsumosRepository.
        /// </summary>
        public ReceitasInsumosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de insumos de receitas.
        /// </summary>
        public async Task<List<ReceitasInsumosModel>> GetAllAsync()
        {
            return await _context.ReceitasInsumos
                .Include(ri => ri.VersaoReceita) // Inclui os dados da versão da receita.
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um registro específico pelo ID.
        /// </summary>
        public async Task<ReceitasInsumosModel> GetByIdAsync(int id)
        {
            return await _context.ReceitasInsumos
                .Include(ri => ri.VersaoReceita)
                .FirstOrDefaultAsync(ri => ri.Id == id);
        }

        /// <summary>
        /// Adiciona um novo registro de insumo para uma receita.
        /// </summary>
        public async Task AddAsync(ReceitasInsumosModel receitaInsumo)
        {
            _context.ReceitasInsumos.Add(receitaInsumo);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um registro de insumo existente.
        /// </summary>
        public async Task UpdateAsync(ReceitasInsumosModel receitaInsumo)
        {
            _context.ReceitasInsumos.Update(receitaInsumo);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um registro de insumo pelo ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var receitaInsumo = await _context.ReceitasInsumos.FindAsync(id);
            if (receitaInsumo != null)
            {
                _context.ReceitasInsumos.Remove(receitaInsumo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
