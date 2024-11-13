using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class TipoIngredienteRepository
    {
        private readonly AppDbContext _context;

        public TipoIngredienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoIngredienteModel>> ObterTodosAsync()
        {
            return await _context.TipoIngredientes.ToListAsync();
        }

        public async Task SalvarAsync(TipoIngredienteModel tipoIngrediente)
        {
            _context.TipoIngredientes.Add(tipoIngrediente);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar um TipoIngrediente existente no banco de dados
        public async Task AtualizarAsync(TipoIngredienteModel tipoIngrediente)
        {
            _context.TipoIngredientes.Update(tipoIngrediente);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(TipoIngredienteModel tipoIngrediente)
        {
            _context.TipoIngredientes.Remove(tipoIngrediente);
            await _context.SaveChangesAsync();
        }
    }
}
