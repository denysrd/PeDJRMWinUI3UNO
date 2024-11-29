using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories;

// Implementação do repositório para manipulação da tabela tbl_receitas_insumos
public class ReceitasInsumosRepository : IReceitasInsumosRepository
{
    // Contexto do banco de dados
    private readonly AppDbContext _dbContext;

    // Injeta o contexto no repositório
    public ReceitasInsumosRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Adiciona um novo registro na tabela tbl_receitas_insumos
    public async Task<int> AddAsync(ReceitasInsumosModel receitaInsumo)
    {
        _dbContext.ReceitasInsumos.Add(receitaInsumo); // Adiciona o registro ao DbSet
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco
        return receitaInsumo.Id; // Retorna o ID gerado
    }

    // Obtém um registro da tabela tbl_receitas_insumos pelo ID
    public async Task<ReceitasInsumosModel> GetByIdAsync(int id)
    {
        var insumo = await _dbContext.ReceitasInsumos
            .Include(ri => ri.VersaoReceita)
            .Include(ri => ri.Insumo)
            .Include(ri => ri.Flavorizante)
            .FirstOrDefaultAsync(ri => ri.Id == id);

        if (insumo == null)
            throw new InvalidOperationException($"Nenhum insumo encontrado com o ID {id}.");

        return insumo;
    }
            
    // Obtém todos os registros relacionados a uma versão específica de receita
    public async Task<IEnumerable<ReceitasInsumosModel>> GetByVersaoReceitaIdAsync(int idVersaoReceita)
    {
        return await _dbContext.ReceitasInsumos
            .Where(ri => ri.Id_Versao_Receita == idVersaoReceita) // Filtra pela versão
            .Include(ri => ri.Insumo) // Inclui informações do insumo
            .Include(ri => ri.Flavorizante) // Inclui informações do flavorizante
            .ToListAsync(); // Retorna como uma lista
    }

    // Atualiza um registro existente na tabela tbl_receitas_insumos
    public async Task<bool> UpdateAsync(ReceitasInsumosModel receitaInsumo)
    {
        var receitaInsumoExistente = await GetByIdAsync(receitaInsumo.Id); // Busca o registro pelo ID
        if (receitaInsumoExistente == null)
        {
            return false; // Retorna falso se o registro não for encontrado
        }

        // Atualiza os campos necessários
        receitaInsumoExistente.Id_Versao_Receita = receitaInsumo.Id_Versao_Receita;
        receitaInsumoExistente.Id_Insumo = receitaInsumo.Id_Insumo;
        receitaInsumoExistente.Unidade_Medida = receitaInsumo.Unidade_Medida;
        receitaInsumoExistente.Quantidade = receitaInsumo.Quantidade;
        receitaInsumoExistente.Id_Flavorizante = receitaInsumo.Id_Flavorizante;

        _dbContext.ReceitasInsumos.Update(receitaInsumoExistente); // Marca o registro como atualizado
        return await _dbContext.SaveChangesAsync() > 0; // Salva as alterações e retorna true se bem-sucedido
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // Obter o registro do banco de dados
        var receitaInsumo = await _dbContext.ReceitasInsumos.FirstOrDefaultAsync(ri => ri.Id == id);
        if (receitaInsumo == null)
            return false;

        // Remover o registro
        _dbContext.ReceitasInsumos.Remove(receitaInsumo);

        // Salvar alterações no banco de dados
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Remove um registro da tabela tbl_receitas_insumos pelo ID
    public async Task<bool> DeleteByVersaoReceitaIdAsync(int versaoReceitaId)
    {
        // Obter os insumos relacionados à versão da receita
        var insumos = await _dbContext.ReceitasInsumos
            .Where(ri => ri.Id_Versao_Receita == versaoReceitaId)
            .ToListAsync();

        if (!insumos.Any())
            return false;

        // Remover os insumos relacionados
        _dbContext.ReceitasInsumos.RemoveRange(insumos);

        // Salvar alterações no banco de dados
        await _dbContext.SaveChangesAsync();
        return true;
    }

    // Método existente de DeleteByReceitaIdAsync para comparação
    public async Task<bool> DeleteByReceitaIdAsync(int receitaId)
    {
        var versoes = await _dbContext.VersoesReceitas
            .Where(v => v.Id_Receita == receitaId)
            .Select(v => v.Id)
            .ToListAsync();

        if (!versoes.Any())
            return false;

        var insumos = await _dbContext.ReceitasInsumos
            .Where(ri => versoes.Contains(ri.Id_Versao_Receita))
            .ToListAsync();

        if (!insumos.Any())
            return false;

        _dbContext.ReceitasInsumos.RemoveRange(insumos);
        await _dbContext.SaveChangesAsync();
        return true;
    }



}
