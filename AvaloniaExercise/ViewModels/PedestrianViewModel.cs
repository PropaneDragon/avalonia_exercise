using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaExercise.ViewModels
{
    public partial class PedestrianViewModel : ObservableObject
    {
        public Pedestrian Pedestrian { get; }

        [ObservableProperty]
        private PedestrianStatus _pedestrianStatus;

        public PedestrianViewModel(Pedestrian pedestrian)
        {
            Pedestrian = pedestrian;
            Pedestrian.StatusChanged += Pedestrian_StatusChanged;
        }

        private void Pedestrian_StatusChanged(object? sender, PedestrianStatus e)
        {
            PedestrianStatus = e;
        }
    }
}
