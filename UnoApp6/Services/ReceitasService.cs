using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Repositories;
using PeDJRMWinUI3UNO.Services;

namespace PeDJRMWinUI3UNO;

public class ReceitasService
{
    private readonly IReceitasRepository _receitasRepository;
    private readonly IReceitasInsumosRepository _receitasInsumosRepository;
    private readonly IVersoesReceitasRepository _versoesReceitasRepository;
    public ReceitasService(IReceitasRepository receitasRepository, IReceitasInsumosRepository receitasInsumosRepository, IVersoesReceitasRepository versoesReceitasRepository)
    {
        _receitasRepository = receitasRepository;
        _receitasInsumosRepository = receitasInsumosRepository;
        _versoesReceitasRepository = versoesReceitasRepository;
    }

    /// Exclui uma receita e todas as suas versões
    public async Task RemoverReceitaComVersoesAsync(int receitaId)
    {
        // Remove as versões da receita
        await _versoesReceitasRepository.DeleteByReceitaIdAsync(receitaId);

        // Remove a receita
        await _receitasRepository.DeleteByIdAsync(receitaId);
    }


    public async Task<IEnumerable<VersoesReceitasModel>> ObterVersoesReceitaAsync(int idReceita)
    {
        // Valida o ID da receita.
        if (idReceita <= 0)
        {
            throw new ArgumentException("O ID da receita fornecido é inválido.");
        }

        // Verifica se o repositório foi corretamente injetado.
        if (_versoesReceitasRepository == null)
        {
            throw new InvalidOperationException("O repositório de versões de receitas não está configurado.");
        }

        try
        {
            // Busca as versões no repositório.
            var versoes = await _versoesReceitasRepository.GetByReceitaIdAsync(idReceita);

            // Verifica se as versões retornadas não são nulas.
            if (versoes == null)
            {
                throw new InvalidOperationException($"Nenhuma versão encontrada para a receita com ID {idReceita}.");
            }

            return versoes;
        }
        catch (Exception ex)
        {
            // Captura e registra exceções, se necessário.
            Debug.WriteLine($"Erro ao obter versões da receita: {ex.Message}");
            throw;
        }
    }



    /// Adiciona uma nova receita ao banco de dados.
    /// Verifica se os campos obrigatórios estão preenchidos e se a receita já existe antes de adicionar.
    /// <param name="receita">Objeto da receita que será adicionada.</param>
    /// <returns>O ID da receita adicionada.</returns>
    /// <exception cref="ArgumentException">Lançado se os campos obrigatórios não forem preenchidos.</exception>
    /// <exception cref="InvalidOperationException">Lançado se uma receita com o mesmo código já existir.</exception>
    public async Task<int> AdicionarReceitaAsync(ReceitasModel receita)
    {
        // Valida se o código da receita ou o nome da receita estão vazios ou nulos.
        // Caso estejam, lança uma exceção de argumento inválido.
        if (string.IsNullOrEmpty(receita.Codigo_Receita) || string.IsNullOrEmpty(receita.Nome_Receita))
            throw new ArgumentException("Campos obrigatórios não preenchidos.");

        // Tenta obter uma receita existente com o mesmo código da nova receita.
        var receitaExistente = await _receitasRepository.ObterPorCodigoAsync(receita.Codigo_Receita);
                
        // Adiciona a nova receita ao banco de dados usando o repositório.
        // Retorna o ID da receita recém-adicionada.
        return await _receitasRepository.AddAsync(receita);
    }

    /// Obtém uma receita pelo código.
    /// <param name="codigoReceita">Código da receita.</param>
    /// <returns>Receita encontrada ou null.</returns>
    public async Task<ReceitasModel?> ObterReceitaPorCodigoAsync(string CodigoReceita)
    {
        return await _receitasRepository.ObterPorCodigoAsync(CodigoReceita);
    }

    public async Task<IEnumerable<ReceitasModel>> ObterTodasReceitasAsync()
    {
        return await _receitasRepository.GetAllAsync();
    }

    public async Task<int> ObterProximoIdReceitaAsync()
    {
        return await _receitasRepository.ObterProximoIdReceitaAsync();
    }

    public async Task<bool> RemoverReceitaAsync(int receitaId)
    {
        if (receitaId <= 0)
            throw new ArgumentException("ID da receita inválido.");

        // Remover insumos relacionados à receita
        var insumosRemovidos = await _receitasInsumosRepository.DeleteByReceitaIdAsync(receitaId);

        if (!insumosRemovidos)
            return false;

        // Remover a própria receita
        return await _receitasRepository.DeleteAsync(receitaId);
    }

    /// Remove uma versão de receita e todos os insumos associados a ela.
    /// <param name="versaoReceitaId">ID da versão da receita a ser removida.</param>
    /// <returns>Verdadeiro se a remoção foi bem-sucedida; falso caso contrário.</returns>
    public async Task<bool> RemoverVersaodeReceitaAsync(int versaoReceitaId)
    {
        // Valida o ID da versão da receita
        if (versaoReceitaId <= 0)
            throw new ArgumentException("ID da versão da receita inválido.", nameof(versaoReceitaId));

        try
        {
            // Remove os insumos associados à versão da receita
            var insumosRemovidos = await _receitasInsumosRepository.DeleteByVersaoReceitaIdAsync(versaoReceitaId);

            if (!insumosRemovidos)
            {
                // Caso não haja insumos a serem removidos, continua o processo
                Console.WriteLine($"Nenhum insumo encontrado para a versão de receita {versaoReceitaId}.");
            }

            // Remove a versão da receita
            var versaoRemovida = await _versoesReceitasRepository.DeleteAsync(versaoReceitaId);

            // Retorna verdadeiro se ambas as operações forem bem-sucedidas
            return versaoRemovida;
        }
        catch (Exception ex)
        {
            // Captura e registra exceções
            Console.WriteLine($"Erro ao remover a versão da receita: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Obtém uma receita pelo ID, utilizando o repositório.
    /// </summary>
    /// <param name="id">O ID da receita.</param>
    /// <returns>Retorna a receita correspondente ao ID ou null se não for encontrada.</returns>
    public async Task<ReceitasModel?> ObterReceitaPorIdAsync(int id)
    {
        try
        {
            // Chama o repositório para buscar a receita pelo ID
            return await _receitasRepository.ObterReceitaPorIdAsync(id);
        }
        catch (Exception ex)
        {
            // Log do erro
            Console.WriteLine($"Erro ao obter receita por ID no serviço: {ex.Message}");
            throw;
        }
    }
}
