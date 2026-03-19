using ActiproSoftware.Extensions;
using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaExercise.ViewModels
{
    public class PedestrianStatusViewModel : ObservableObject
    {
        private readonly IPedestrianSensorService _pedestrianSensorService;

        public ObservableCollection<PedestrianViewModel> Pedestrians { get; } = [];

        public PedestrianStatusViewModel(IPedestrianSensorService pedestrianSensorService)
        {
            _pedestrianSensorService = pedestrianSensorService;
            _pedestrianSensorService.PedestriansChanged += PedestrianSensorService_PedestriansChanged;

            Pedestrians.AddRange(_pedestrianSensorService.Pedestrians.Select(pedestrian => new PedestrianViewModel(pedestrian)));
        }

        private void PedestrianSensorService_PedestriansChanged(object? sender, PedestriansChangedEventArgs e)
        {
            if (e.Operation == PedestrianOperation.Arrived)
            {
                Pedestrians.Add(new(e.Pedestrian));
            }
            else if (e.Operation == PedestrianOperation.Left)
            {
                Pedestrians.RemoveAll(viewModel => viewModel.Pedestrian == e.Pedestrian);
            }
        }
    }
}
