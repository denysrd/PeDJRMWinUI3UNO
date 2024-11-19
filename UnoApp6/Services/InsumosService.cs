using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    public class InsumosService
    {
        private readonly InsumosRepository _insumosRepository;

        public InsumosService(InsumosRepository insumosRepository)
        {
            _insumosRepository = insumosRepository;
        }

        // Método para obter o próximo ID disponível para insumo
        public Task<int> ObterProximoIdAsync()
        {
            return _insumosRepository.ObterProximoIdAsync();
        }

        // Método para obter todos os insumos
        public Task<List<InsumosModel>> ObterTodosAsync()
        {
            return _insumosRepository.ObterTodosAsync();
        }

        // Método para encontrar um insumo pelo ID
        public Task<InsumosModel> FindAsync(int id)
        {
            return _insumosRepository.FindAsync(id);
        }

        // Método para salvar um novo insumo com validação
        public async Task<bool> SalvarAsync(InsumosModel insumo)
        {
            // Validação básica antes de salvar
            if (string.IsNullOrWhiteSpace(insumo.Nome) || insumo.Custo <= 0 || insumo.IdTipoIngrediente <= 0)
            {
                return false;
            }

            return await _insumosRepository.SalvarAsync(insumo);
        }

        // Método para atualizar um insumo existente com validação
        public async Task<bool> AtualizarAsync(InsumosModel insumo)
        {
            // Validação básica antes de atualizar
            if (insumo.Id_Insumo == 0 || string.IsNullOrWhiteSpace(insumo.Nome) || insumo.Custo <= 0 || insumo.IdTipoIngrediente <= 0)
            {
                return false;
            }

            return await _insumosRepository.AtualizarAsync(insumo);
        }

        // Método para remover um insumo pelo ID
        public Task<bool> RemoverAsync(int id)
        {
            return _insumosRepository.RemoverAsync(id);
        }
    }
}
