using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories
{
    /// <summary>
    /// Repositório responsável pelo acesso à tabela tbl_receitas_flavorizantes.
    /// </summary>
    public class ReceitasFlavorizantesRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor do repositório ReceitasFlavorizantesRepository.
        /// </summary>
        public ReceitasFlavorizantesRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de flavorizantes de receitas.
        /// </summary>
        public async Task<List<ReceitasFlavorizantesModel>> GetAllAsync()
        {
            return await _context.ReceitasFlavorizantes
                .Include(rf => rf.VersaoReceita) // Inclui os dados da versão da receita.
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um registro específico pelo ID.
        /// </summary>
        public async Task<ReceitasFlavorizantesModel> GetByIdAsync(int id)
        {
            return await _context.ReceitasFlavorizantes
                .Include(rf => rf.VersaoReceita)
                .FirstOrDefaultAsync(rf => rf.Id == id);
        }

        /// <summary>
        /// Adiciona um novo registro de flavorizante para uma receita.
        /// </summary>
        public async Task AddAsync(ReceitasFlavorizantesModel receitaFlavorizante)
        {
            _context.ReceitasFlavorizantes.Add(receitaFlavorizante);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um registro de flavorizante existente.
        /// </summary>
        public async Task UpdateAsync(ReceitasFlavorizantesModel receitaFlavorizante)
        {
            _context.ReceitasFlavorizantes.Update(receitaFlavorizante);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um registro de flavorizante pelo ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var receitaFlavorizante = await _context.ReceitasFlavorizantes.FindAsync(id);
            if (receitaFlavorizante != null)
            {
                _context.ReceitasFlavorizantes.Remove(receitaFlavorizante);
                await _context.SaveChangesAsync();
            }
        }
    }
}
