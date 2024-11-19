using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SkiaSharp;

namespace PeDJRMWinUI3UNO.Repositories
{
    /// Repositório responsável pelo acesso à tabela tbl_receitas.
    public class ReceitasRepository
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        /// Construtor do repositório ReceitasRepository.
        public ReceitasRepository(DbContextOptions<AppDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        // Método para obter o próximo ID disponível
        public async Task<int> ObterProximoIdReceitaAsync()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var ultimaReceita = await context.ReceitasModel.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
                return ultimaReceita?.Id + 1 ?? 1; // Se não houver registros, começa com 1
            }
        }
        // Método para obter todos os insumos
        public async Task<List<ReceitasModel>> ObterTodosAsync()
        {
            try
            {
                using (var context = new AppDbContext(_dbContextOptions))
                {
                    return await context.ReceitasModel.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao obter Receitas: {ex.Message}");
                return new List<ReceitasModel>();
            }
        }

        // Método para encontrar um insumo pelo ID
        public async Task<ReceitasModel> GetByIdAsync(int id)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                return await context.ReceitasModel.FindAsync(id);
            }
        }
       
    }
}
