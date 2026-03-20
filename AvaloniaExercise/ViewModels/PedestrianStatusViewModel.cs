using ActiproSoftware.Extensions;
using Avalonia.Threading;
using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaExercise.ViewModels
{
    public class PedestrianStatusViewModel : ObservableObject
    {
        private readonly IPedestrianSensorService _pedestrianSensorService;
        private readonly DispatcherTimer _intervalTimer = new(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, IntervalTimer_Tick);

        public ObservableCollection<PedestrianViewModel> Pedestrians { get; } = [];

        public PedestrianStatusViewModel(IPedestrianSensorService pedestrianSensorService)
        {
            _pedestrianSensorService = pedestrianSensorService;
            _pedestrianSensorService.PedestriansChanged += PedestrianSensorService_PedestriansChanged;

            Pedestrians.AddRange(_pedestrianSensorService.Pedestrians.Select(pedestrian => new PedestrianViewModel(pedestrian)));

            _intervalTimer.Start();
        }

        private void PedestrianSensorService_PedestriansChanged(object? sender, PedestriansChangedEventArgs e)
        {
            if (e.Operation == PedestrianOperation.Arrived)
            {
                Pedestrians.Add(new(e.Pedestrian));
            }
            else if (e.Operation == PedestrianOperation.Left)
            {
                Pedestrians.RemoveAll(viewModel => viewModel.AssociatedPedestrian == e.Pedestrian);
            }
        }

        private static void IntervalTimer_Tick(object? sender, EventArgs e) => WeakReferenceMessenger.Default.Send(new TimerTickMessage());
    }
}
