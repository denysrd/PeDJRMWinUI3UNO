using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Services;

namespace PeDJRMWinUI3UNO.ViewModels
{
    public class FlavorizantesViewModel
    {
        private readonly FlavorizantesService _flavorizantesService;

        public ObservableCollection<FlavorizantesModel> Flavorizantes { get; set; } = new ObservableCollection<FlavorizantesModel>();

        public FlavorizantesViewModel(FlavorizantesService flavorizantesService)
        {
            _flavorizantesService = flavorizantesService;
            CarregarFlavorizantesAsync();
        }

        private async void CarregarFlavorizantesAsync()
        {
            var flavorizantes = await _flavorizantesService.ObterTodosAsync();
            if (flavorizantes != null)
            {
                foreach (var flavorizante in flavorizantes)
                {
                    Flavorizantes.Add(flavorizante);
                }
            }
        }
    }
}
