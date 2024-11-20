using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO
{
    public class ReceitasService
    {
        private readonly IReceitasRepository _receitasRepository;

        public ReceitasService(IReceitasRepository receitasRepository)
        {
            _receitasRepository = receitasRepository;
        }

        public async Task<int> AdicionarReceitaAsync(ReceitasModel receita)
        {
            if (string.IsNullOrEmpty(receita.Codigo_Receita) || string.IsNullOrEmpty(receita.Nome_Receita))
                throw new ArgumentException("Campos obrigatórios não preenchidos.");

            var receitaExistente = await _receitasRepository.ObterPorCodigoAsync(receita.Codigo_Receita);
            if (receitaExistente != null)
                throw new Exception($"Receita com código {receita.Codigo_Receita} já existe.");

            return await _receitasRepository.AddAsync(receita);
        }

        public async Task<IEnumerable<ReceitasModel>> ObterReceitasAsync()
        {
            return await _receitasRepository.GetAllAsync();
        }

        public async Task<int> ObterProximoIdReceitaAsync()
        {
            return await _receitasRepository.ObterProximoIdReceitaAsync();
        }
        // Outros métodos como Atualizar, Remover, etc.
    }
}
