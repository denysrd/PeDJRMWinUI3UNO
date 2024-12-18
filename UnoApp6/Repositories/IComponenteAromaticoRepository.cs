using System.Collections.Generic; // Importa suporte para coleções genéricas
using System.Threading.Tasks; // Importa funcionalidades assíncronas

namespace PeDJRMWinUI3UNO.Repositories // Define o namespace do repositório
{
    // Interface para o repositório de ComponenteAromatico
    public interface IComponenteAromaticoRepository
    {
        // Método para buscar todos os componentes aromáticos
        Task<IEnumerable<ComponenteAromaticoModel>> ObterTodosAsync(); // Retorna uma lista de componentes aromáticos

        // Método para buscar um componente aromático pelo ID
        Task<ComponenteAromaticoModel?> ObterPorIdAsync(int id); // Retorna o componente correspondente ao ID fornecido

        // Método para salvar um novo componente aromático
        Task<bool> SalvarAsync(ComponenteAromaticoModel componente); // Salva um novo componente aromático no banco

        // Método para atualizar um componente aromático existente
        Task<bool> AtualizarAsync(ComponenteAromaticoModel componente); // Atualiza as informações de um componente existente
        
        // Método para excluir um componente aromático pelo ID
        Task ExcluirAsync(int id); // Exclui o componente correspondente ao ID fornecido

        // Método para buscar o próximo ID da tabela
        Task<int> ObterProximoIdAsync();        
    }
}
