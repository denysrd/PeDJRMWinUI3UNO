using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class FornecedorRepository
    {
        private readonly AppDbContext _context;

        public FornecedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FornecedorModel> FindAsync(int Id_Fornecedor)
        {
            return await _context.FornecedorModel.FindAsync(Id_Fornecedor);
        }

        public async Task<bool> InserirFornecedorAsync(FornecedorModel fornecedor)
        {
            _context.FornecedorModel.Add(fornecedor);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<FornecedorModel>> ObterTodosFornecedoresAsync()
        {
            return await _context.FornecedorModel.ToListAsync();
        }

        public async Task<FornecedorModel> ObterFornecedorPorIdAsync(int Id_Fornecedor)
        {
            return await _context.FornecedorModel.FindAsync(Id_Fornecedor);
        }

        public IQueryable<FornecedorModel> ObterTodosFornecedor()
        {
            return _context.FornecedorModel.AsQueryable();
        }

        public async Task<bool> AtualizarFornecedorAsync(FornecedorModel fornecedor)
        {
            _context.FornecedorModel.Update(fornecedor);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoverFornecedorAsync(int Id_Fornecedor)
        {
            var fornecedor = await _context.FornecedorModel.FindAsync(Id_Fornecedor);
            if (fornecedor != null)
            {
                _context.FornecedorModel.Remove(fornecedor);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
