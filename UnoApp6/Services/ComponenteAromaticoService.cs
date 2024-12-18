using System.Collections.Generic; // Importa suporte para coleções genéricas
using System.Threading.Tasks; // Importa funcionalidades assíncronas
using PeDJRMWinUI3UNO.Models; // Importa o modelo ComponenteAromaticoModel
using PeDJRMWinUI3UNO.Repositories; // Importa o repositório IComponenteAromaticoRepository

namespace PeDJRMWinUI3UNO.Services // Define o namespace do serviço
{
    // Serviço para gerenciar os componentes aromáticos
    public class ComponenteAromaticoService
    {
        // Repositório de componente aromático
        private readonly IComponenteAromaticoRepository _repository; // Repositório para acesso aos dados

        // Construtor que injeta o repositório
        public ComponenteAromaticoService(IComponenteAromaticoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); // Valida o repositório
        }

        // Método para buscar todos os componentes aromáticos
        public async Task<IEnumerable<ComponenteAromaticoModel>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync(); // Chama o método do repositório
        }

        // Método para buscar um componente aromático pelo ID
        public async Task<ComponenteAromaticoModel?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id); // Chama o método do repositório
        }

        // Método para salvar um novo componente aromático
        public async Task<bool> SalvarAsync(ComponenteAromaticoModel componente)
        {
            // Validação básica antes de salvar
            if (string.IsNullOrWhiteSpace(componente.Nome) || componente.Custo <= 0 || componente.IdTipoIngrediente <= 0)
            {
                return false;
            }
            await _repository.SalvarAsync(componente); // Chama o método do repositório
            return true; // Retorna true se a operação for bem-sucedida
        }

        // Método para atualizar um componente aromático existente
        public async Task<bool> AtualizarAsync(ComponenteAromaticoModel componente)
        {
            // Validação básica antes de atualizar
            if (componente.Id == 0 || string.IsNullOrWhiteSpace(componente.Nome) || componente.Custo <= 0 || componente.IdTipoIngrediente <= 0)
            {
                return false;
            }
            await _repository.AtualizarAsync(componente); // Chama o método do repositório
            return true; // Retorna true se a operação for bem-sucedida
        }

        // Método para excluir um componente aromático pelo ID
        public async Task ExcluirAsync(int id)
        {
            await _repository.ExcluirAsync(id); // Chama o método do repositório
        }

        // Método para obter o próximo ID disponível para insumo
        public Task<int> ObterProximoIdAsync()
        {
            return _repository.ObterProximoIdAsync();
        }
    }
}
