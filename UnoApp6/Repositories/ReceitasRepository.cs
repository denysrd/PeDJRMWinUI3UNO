using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace PeDJRMWinUI3UNO;

public class ReceitasRepository : IReceitasRepository
{
    private readonly AppDbContext _dbContext;

    public ReceitasRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
            
    public async Task<int> AddAsync(ReceitasModel receita)
    {
        _dbContext.ReceitasModel.Add(receita);
        await _dbContext.SaveChangesAsync();
        return receita.Id; // Retorna o ID gerado
    }

    /// Exclui uma receita pelo ID
    public async Task DeleteByIdAsync(int id)
    {
        var receita = await _dbContext.ReceitasModel.FirstOrDefaultAsync(r => r.Id == id);
        if (receita != null)
        {
            _dbContext.ReceitasModel.Remove(receita);
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<ReceitasModel?> ObterReceitaPorIdAsync(int id)
    {
        try
        {
            // Busca a receita no banco de dados pelo ID, incluindo as versões se necessário
            return await _dbContext.ReceitasModel
                .Include(r => r.VersoesReceitas) // Inclui as versões associadas
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        catch (Exception ex)
        {
            // Log do erro
            Console.WriteLine($"Erro ao obter receita por ID no repositório: {ex.Message}");
            throw;
        }
    }


    /// Obtém todas as versões associadas a um ID de receita.
    /// <param name="receitaId">ID da receita.</param>
    /// <returns>Uma lista de versões da receita.</returns>
    public async Task<IEnumerable<VersoesReceitasModel>> GetByReceitaIdAsync(int receitaId)
    {
        // Consulta no banco de dados as versões associadas ao ID da receita
        return await _dbContext.VersoesReceitas
            .Where(v => v.Id_Receita == receitaId) // Filtra pelo ID da receita
            .OrderBy(v => v.Versao) // Ordena pela versão
            .ToListAsync(); // Retorna como uma lista
    }

    public async Task<ReceitasModel> GetByIdAsync(int id)
    {
        var receita = await _dbContext.ReceitasModel.FirstOrDefaultAsync(r => r.Id == id);
        if (receita == null)
            throw new InvalidOperationException($"Nenhuma receita encontrada com o ID {id}.");
        return receita;
    }

    public async Task<ReceitasModel?> ObterPorCodigoAsync(string codigoReceita)
    {
        try
        {
            // Certifique-se de usar o contexto de banco de dados para buscar pelo código.
            Console.WriteLine($"Buscando receita com código: {codigoReceita}");
            var receita = await _dbContext.ReceitasModel
                .FirstOrDefaultAsync(r => r.Codigo_Receita == codigoReceita);
            Console.WriteLine(receita != null ? $"Receita encontrada: {receita.Nome_Receita}" : "Receita não encontrada.");
            return receita;
        }
        catch (Exception ex)
        {
            // Log de erro para análise
            Console.WriteLine($"Erro ao buscar receita pelo código: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ReceitasModel>> GetAllAsync()
    {
        return await _dbContext.ReceitasModel.ToListAsync();
    }

    public async Task<bool> UpdateAsync(ReceitasModel receita)
    {
        var receitaExistente = await GetByIdAsync(receita.Id);
        if (receitaExistente == null)
            return false;

        receitaExistente.Codigo_Receita = receita.Codigo_Receita;
        receitaExistente.Nome_Receita = receita.Nome_Receita;
        receitaExistente.Data = receita.Data;
        receitaExistente.Descricao_Processo = receita.Descricao_Processo;

        _dbContext.ReceitasModel.Update(receitaExistente);
        return await _dbContext.SaveChangesAsync() > 0;
    }      

    // Método para obter o próximo ID disponível
    public async Task<int> ObterProximoIdReceitaAsync()
    {            
            var ultimaReceita = await _dbContext.ReceitasModel.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
            return ultimaReceita?.Id + 1 ?? 1; // Se não houver registros, começa com 1            
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Busca a receita pelo ID
        var receita = await GetByIdAsync(id);

        if (receita == null)
            return false;

        // Remove a receita do banco de dados
        _dbContext.ReceitasModel.Remove(receita);

        // Salva as alterações
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteByVersaoReceitaIdAsync(int versaoReceitaId)
    {
        // Obtém todos os insumos associados à versão da receita
        var insumos = await _dbContext.ReceitasInsumos
            .Where(ri => ri.Id_Versao_Receita == versaoReceitaId)
            .ToListAsync();

        if (!insumos.Any())
            return false;

        // Remove os insumos encontrados
        _dbContext.ReceitasInsumos.RemoveRange(insumos);

        // Salva as alterações no banco
        return await _dbContext.SaveChangesAsync() > 0;
    }

}
