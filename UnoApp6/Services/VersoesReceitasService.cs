using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services;

public class VersoesReceitasService
{
    private readonly IVersoesReceitasRepository _versoesReceitasRepository; // Repositório de versões
    private readonly IReceitasInsumosRepository _receitasInsumosRepository; // Repositório de insumos


    public VersoesReceitasService(IVersoesReceitasRepository versoesReceitasRepository)
    {
        _versoesReceitasRepository = versoesReceitasRepository; // Injeta o repositório no serviço
    }

    public async Task<int> AdicionarVersaoReceitaAsync(VersoesReceitasModel versaoReceita)
    {
        if (versaoReceita.Id_Receita <= 0)
        {
            throw new ArgumentException("ID da receita é obrigatório."); // Valida o ID da receita
        }

        if (versaoReceita.Versao <= 0)
        {
            throw new ArgumentException("A versão deve ser maior que zero."); // Valida o número da versão
        }

        return await _versoesReceitasRepository.AddAsync(versaoReceita); // Adiciona a versão ao banco
    }

    public async Task<VersoesReceitasModel> ObterPorIdAsync(int id)
    {
        return await _versoesReceitasRepository.GetByIdAsync(id); // Obtém uma versão pelo ID
    }

    public async Task<IEnumerable<VersoesReceitasModel>> ObterPorReceitaIdAsync(int idReceita)
    {
        return await _versoesReceitasRepository.GetByReceitaIdAsync(idReceita); // Obtém todas as versões de uma receita
    }

    public async Task<bool> AtualizarVersaoReceitaAsync(VersoesReceitasModel versaoReceita)
    {
        if (versaoReceita.Id <= 0)
        {
            throw new ArgumentException("ID da versão é obrigatório."); // Valida o ID da versão
        }

        return await _versoesReceitasRepository.UpdateAsync(versaoReceita); // Atualiza a versão no banco
    }

    public async Task<bool> RemoverVersaoReceitaAsync(int id)
    {
        return await _versoesReceitasRepository.DeleteAsync(id); // Remove a versão pelo ID
    }

    public async Task<bool> RemoverPorReceitaIdAsync(int receitaId)
    {
        if (receitaId <= 0)
            throw new ArgumentException("ID da receita inválido.");

        // Chama o repositório para excluir as versões
        return await _versoesReceitasRepository.DeleteByReceitaIdAsync(receitaId);
    }

    

}
