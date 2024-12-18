using PeDJRMWinUI3UNO.Models; // Importa o namespace dos modelos
using Microsoft.EntityFrameworkCore; // Permite o uso do Entity Framework Core
using System.Collections.Generic; // Permite o uso de coleções genéricas
using System.Threading.Tasks; // Permite a definição de métodos assíncronos
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories; // Define o namespace do repositório

/// <summary>
/// Implementação do repositório para insumos da fórmula flavorizante.
/// </summary>
public class FormulaFlavorizanteInsumosRepository : IFormulaFlavorizanteInsumosRepository
{
    private readonly AppDbContext _dbContext; // Instância do contexto do banco de dados

    // Construtor que recebe o contexto do banco de dados via injeção de dependência
    public FormulaFlavorizanteInsumosRepository(AppDbContext dbcontext)
    {
        _dbContext = dbcontext; // Inicializa o contexto
    }

    // Implementação do método para obter todos os registros
    public async Task<IEnumerable<FormulaFlavorizanteInsumosModel>> ObterTodosAsync()
    {
        return await _dbContext.Set<FormulaFlavorizanteInsumosModel>().ToListAsync(); // Retorna todos os registros
    }

    // Implementação do método para obter um registro pelo ID
    public async Task<FormulaFlavorizanteInsumosModel?> ObterPorIdAsync(int id)
    {
        return await _dbContext.Set<FormulaFlavorizanteInsumosModel>().FindAsync(id); // Busca um registro pelo ID
    }

    // Implementação do método para adicionar um novo registro
    public async Task<int> AdicionarAsync(FormulaFlavorizanteInsumosModel insumo)
    {
        await _dbContext.Set<FormulaFlavorizanteInsumosModel>().AddAsync(insumo); // Adiciona o novo registro
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco
        return insumo.Id; // Retorna o ID gerado
    }

    // Implementação do método para atualizar um registro existente
    public async Task AtualizarAsync(FormulaFlavorizanteInsumosModel insumo)
    {
        _dbContext.Set<FormulaFlavorizanteInsumosModel>().Update(insumo); // Marca o registro como modificado
        await _dbContext.SaveChangesAsync(); // Salva as alterações no banco
    }

    // Implementação do método para excluir um registro pelo ID
    public async Task ExcluirAsync(int id)
    {
        var insumo = await ObterPorIdAsync(id); // Busca o registro pelo ID
        if (insumo != null) // Verifica se o registro existe
        {
            _dbContext.Set<FormulaFlavorizanteInsumosModel>().Remove(insumo); // Remove o registro
            await _dbContext.SaveChangesAsync(); // Salva as alterações no banco
        }
    }

    // Obtém todos os registros relacionados a uma versão específica de receita
    public async Task<IEnumerable<FormulaFlavorizanteInsumosModel>> GetByVersaoFormulaIdAsync(int idVersaoFormulaFlavorizante)
    {
        return await _dbContext.FormulaFlavorizanteInsumos
            .Where(ri => ri.Id_Versao_Formula_Flavorizante == idVersaoFormulaFlavorizante) // Filtra pela versão
            .Include(ri => ri.Insumo) // Inclui informações do insumo
            .Include(ri => ri.Id_Car) // Inclui informações do flavorizante
            .ToListAsync(); // Retorna como uma lista
    }
}
