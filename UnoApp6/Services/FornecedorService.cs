using System.Text.RegularExpressions;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    public class FornecedorService
    {
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly AppDbContext _context;

        public FornecedorService(FornecedorRepository fornecedorRepository, AppDbContext context)
        {
            _fornecedorRepository = fornecedorRepository;
            _context = context;
        }

        public bool ValidarDocumento(string documento)
        {
            documento = Regex.Replace(documento, @"[^\d]", "");
            return documento.Length == 11 || documento.Length == 14;
        }

        public async Task<bool> InserirFornecedorAsync(FornecedorModel fornecedor)
        {
            if (string.IsNullOrWhiteSpace(fornecedor.Nome) || !ValidarDocumento(fornecedor.Documento))
            {
                return false;
            }

            // Inicia uma transação para a operação de inserção
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _fornecedorRepository.InserirFornecedorAsync(fornecedor);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Método para obter todos os fornecedores cadastrados
        public async Task<List<FornecedorModel>> ObterTodosFornecedoresAsync()
        {
            // Chama o repositório para buscar todos os fornecedores
            return await _fornecedorRepository.ObterTodosFornecedoresAsync();
        }

        public async Task ExcluirFornecedorAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var fornecedor = await _fornecedorRepository.FindAsync(id);
                if (fornecedor != null)
                {
                    await _fornecedorRepository.RemoverFornecedorAsync(id);

                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("Fornecedor não encontrado.");
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}