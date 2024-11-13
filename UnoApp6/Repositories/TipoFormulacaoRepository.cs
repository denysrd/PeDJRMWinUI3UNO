using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class TipoFormulacaoRepository
    {
        private readonly AppDbContext _context;

        public TipoFormulacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método para encontrar um Tipo de Formulação pelo ID
        public async Task<TipoFormulacaoModel> FindAsync(int Id_Tipo_Formulacao)
        {
            return await _context.TipoFormulacaoModel.FindAsync(Id_Tipo_Formulacao);
        }

        // Método para salvar um novo Tipo de Formulação
        public async Task<bool> SalvarAsync(TipoFormulacaoModel tipoFormulacao)
        {
            _context.TipoFormulacaoModel.Add(tipoFormulacao);
            return await _context.SaveChangesAsync() > 0;
        }

        // Método para atualizar um Tipo de Formulação existente
        public async Task<bool> AtualizarAsync(TipoFormulacaoModel tipoFormulacao)
        {
            _context.TipoFormulacaoModel.Update(tipoFormulacao);
            return await _context.SaveChangesAsync() > 0;
        }

        // Método para remover um Tipo de Formulação pelo ID
        public async Task<bool> RemoverAsync(int Id_Tipo_Formulacao)
        {
            var tipoFormulacao = await FindAsync(Id_Tipo_Formulacao);
            if (tipoFormulacao != null)
            {
                _context.TipoFormulacaoModel.Remove(tipoFormulacao);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        // Método para obter todos os tipos de formulações
        public async Task<List<TipoFormulacaoModel>> ObterTodosAsync()
        {
            return await _context.TipoFormulacaoModel.ToListAsync();
        }
    }
}
