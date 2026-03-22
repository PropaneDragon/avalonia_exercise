using ActiproSoftware.Extensions;
using Avalonia.Threading;
using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace AvaloniaExercise.ViewModels
{
    public partial class PedestrianStatusViewModel : ObservableObject
    {
        private readonly IPedestrianSensorService _pedestrianSensorService;
        private readonly DispatcherTimer _intervalTimer = new(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, IntervalTimer_Tick);
        private readonly Lock _pedestrianLock = new();

        [ObservableProperty]
        private bool _filterWaiting = false;

        [ObservableProperty]
        private bool _filterCrossing = false;

        public int WaitingPedestrians
        {
            get
            {
                lock (_pedestrianLock) { return Pedestrians.Count(pedestrian => pedestrian.PedestrianStatus == PedestrianStatus.WaitingToCross); }
            }
        }

        public int CrossingPedestrians
        {
            get
            {
                lock (_pedestrianLock) { return Pedestrians.Count(pedestrian => pedestrian.PedestrianStatus == PedestrianStatus.Crossing); }
            }
        }

        public ObservableCollection<PedestrianViewModel> Pedestrians { get; } = [];
        public ObservableCollection<PedestrianViewModel> FilteredPedestrians { get; } = [];

        public PedestrianStatusViewModel(IPedestrianSensorService pedestrianSensorService)
        {
            _pedestrianSensorService = pedestrianSensorService;
            _pedestrianSensorService.PedestriansChanged += PedestrianSensorService_PedestriansChanged;

            lock (_pedestrianLock)
            {
                Pedestrians.AddRange(_pedestrianSensorService.Pedestrians.Select(pedestrian => new PedestrianViewModel(pedestrian)));
            }

            ReapplyPedestrianFilter();

            _intervalTimer.Start();
        }

        private void PedestrianSensorService_PedestriansChanged(object? sender, PedestriansChangedEventArgs e)
        {
            if (e.Operation == PedestrianOperation.Arrived)
            {
                lock (_pedestrianLock)
                {
                    Pedestrians.Add(new(e.Pedestrian));
                }

                e.Pedestrian.StatusChanged += Pedestrian_StatusChanged;
            }
            else if (e.Operation == PedestrianOperation.Left)
            {
                lock (_pedestrianLock)
                {
                    Pedestrians.RemoveAll(viewModel => viewModel.AssociatedPedestrian == e.Pedestrian);
                }

                e.Pedestrian.StatusChanged -= Pedestrian_StatusChanged;
            }

            ReapplyPedestrianFilter();

            OnPropertyChanged(nameof(WaitingPedestrians));
            OnPropertyChanged(nameof(CrossingPedestrians));
        }

        private void ReapplyPedestrianFilter()
        {
            lock (_pedestrianLock)
            {
                FilteredPedestrians.RemoveAll(pedestrian => !Pedestrians.Contains(pedestrian) || !MatchesFilter(pedestrian));
                FilteredPedestrians.AddRange(Pedestrians.Where(pedestrian => MatchesFilter(pedestrian) && !FilteredPedestrians.Contains(pedestrian)));
            }
        }

        private void Pedestrian_StatusChanged(object? sender, PedestrianStatus e)
        {
            ReapplyPedestrianFilter();

            OnPropertyChanged(nameof(WaitingPedestrians));
            OnPropertyChanged(nameof(CrossingPedestrians));
        }

        private bool MatchesFilter(PedestrianViewModel pedestrian)
        {
            return  (!FilterWaiting && !FilterCrossing) ||
                    (FilterWaiting && pedestrian.IsWaiting) ||
                    (FilterCrossing && pedestrian.IsCrossing);
        }

        partial void OnFilterWaitingChanged(bool value) => ReapplyPedestrianFilter();
        partial void OnFilterCrossingChanged(bool value) => ReapplyPedestrianFilter();

        private static void IntervalTimer_Tick(object? sender, EventArgs e) => WeakReferenceMessenger.Default.Send(new TimerTickMessage());
    }
}
