using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// Serviço responsável pela lógica de negócios para receitas.
    public class ReceitasService
    {
        private readonly ReceitasRepository _repository;
                
        /// Construtor do serviço ReceitasService.
        public ReceitasService(ReceitasRepository repository)
        {
            _repository = repository;
        }

        /// Obtém todas as receitas cadastradas.
        public async Task<List<ReceitasModel>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }

        /// Obtém uma receita específica pelo ID.
        public async Task<ReceitasModel> GetReceitaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> ObterProximoIdReceitaAsync()
        {
            return await _repository.ObterProximoIdReceitaAsync();
        }

    }
}
