using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories;

public class VersoesReceitasRepository : IVersoesReceitasRepository
{
    private readonly AppDbContext _dbContext; // Contexto do banco de dados

    public VersoesReceitasRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); // Injeta o contexto no repositório
    }

    public async Task<int> AddAsync(VersoesReceitasModel versaoReceita)
    {
        _dbContext.VersoesReceitas.Add(versaoReceita); // Adiciona a versão ao DbSet
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
        return versaoReceita.Id; // Retorna o ID gerado
    }    

    public async Task<VersoesReceitasModel> GetByIdAsync(int id)
    {
        var versao = await _dbContext.VersoesReceitas
            .Include(v => v.Receita)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (versao == null)
            throw new InvalidOperationException($"Nenhuma versão encontrada com o ID {id}.");

        return versao;
    }

    public async Task<IEnumerable<VersoesReceitasModel>> GetByReceitaIdAsync(int idReceita)
    {
        // Obtém todas as versões associadas a uma receita específica
        return await _dbContext.VersoesReceitas
            .Where(v => v.Id_Receita == idReceita)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(VersoesReceitasModel versaoReceita)
    {
        var versaoExistente = await GetByIdAsync(versaoReceita.Id); // Obtém a versão existente pelo ID
        if (versaoExistente == null)
        {
            return false; // Retorna falso se a versão não for encontrada
        }

        // Atualiza os campos
        versaoExistente.Versao = versaoReceita.Versao;
        versaoExistente.Data = versaoReceita.Data;
        versaoExistente.Descricao_Processo = versaoReceita.Descricao_Processo;

        _dbContext.VersoesReceitas.Update(versaoExistente); // Marca a entidade como atualizada
        return await _dbContext.SaveChangesAsync() > 0; // Retorna true se a atualização for bem-sucedida
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var versao = await GetByIdAsync(id); // Obtém a versão pelo ID
        if (versao == null)
        {
            return false; // Retorna falso se a versão não for encontrada
        }

        _dbContext.VersoesReceitas.Remove(versao); // Remove a versão do DbSet
        return await _dbContext.SaveChangesAsync() > 0; // Retorna true se a remoção for bem-sucedida
    }

    public async Task<bool> DeleteByReceitaIdAsync(int receitaId)
    {
        // Busca as versões relacionadas à receita
        var versoes = await _dbContext.VersoesReceitas
            .Where(v => v.Id_Receita == receitaId)
            .ToListAsync();

        if (!versoes.Any())
            return false;

        // Remove todas as versões encontradas
        _dbContext.VersoesReceitas.RemoveRange(versoes);

        // Salva as alterações
        return await _dbContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Remove todos os insumos associados a uma versão específica de receita.
    /// </summary>
    /// <param name="versaoReceitaId">ID da versão da receita.</param>
    /// <returns>Verdadeiro se a remoção foi bem-sucedida; falso caso contrário.</returns>
    public async Task<bool> DeleteByVersaoReceitaIdAsync(int versaoReceitaId)
    {
        // Obtém os insumos relacionados à versão da receita
        var insumos = await _dbContext.ReceitasInsumos
            .Where(ri => ri.Id_Versao_Receita == versaoReceitaId)
            .ToListAsync();

        if (!insumos.Any())
            return false;

        // Remove os insumos encontrados
        _dbContext.ReceitasInsumos.RemoveRange(insumos);

        // Salva as alterações no banco de dados
        return await _dbContext.SaveChangesAsync() > 0;
    }

    /// Implementação do método para buscar todas as versões associadas a uma receita
    /// <param name="idReceita">O ID da receita.</param>
    /// <returns>Uma lista de objetos `VersoesReceitasModel` representando as versões da receita.</returns>
    public async Task<IEnumerable<VersoesReceitasModel>> ObterVersoesReceitaAsync(int idReceita)
    {
        // Valida se o ID da receita é válido
        if (idReceita <= 0)
        {
            throw new ArgumentException("O ID da receita é inválido.");
        }

        // Busca todas as versões da receita no banco de dados
        // Utiliza LINQ para filtrar as versões pelo ID da receita
        return await _dbContext.VersoesReceitas
            .Where(v => v.Id_Receita == idReceita) // Filtra pelo ID da receita
            .ToListAsync(); // Converte para uma lista de forma assíncrona
    }
}
