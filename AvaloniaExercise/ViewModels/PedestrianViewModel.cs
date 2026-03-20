using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;

namespace AvaloniaExercise.ViewModels
{
    public record TimerTickMessage;

    public partial class PedestrianViewModel : ObservableObject, IRecipient<TimerTickMessage>
    {
        public Pedestrian AssociatedPedestrian { get; }

        [NotifyPropertyChangedFor(nameof(IsWaiting))]
        [NotifyPropertyChangedFor(nameof(IsCrossing))]
        [NotifyPropertyChangedFor(nameof(HasCrossed))]
        [ObservableProperty]
        private PedestrianStatus _pedestrianStatus;

        public bool IsWaiting => PedestrianStatus == PedestrianStatus.WaitingToCross;
        public bool IsCrossing => PedestrianStatus == PedestrianStatus.Crossing;
        public bool HasCrossed => PedestrianStatus == PedestrianStatus.Crossed;

        public string Name => AssociatedPedestrian.Name;
        public string SpeciesIcon => AssociatedPedestrian.Species.ToIconName();
        public string SpeciesName => AssociatedPedestrian.Species.ToDisplayName();

        public TimeSpan WaitTime => DateTime.UtcNow - AssociatedPedestrian.ArrivedAtUtc;

        public PedestrianViewModel(Pedestrian pedestrian)
        {
            AssociatedPedestrian = pedestrian;
            AssociatedPedestrian.StatusChanged += Pedestrian_StatusChanged;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void Pedestrian_StatusChanged(object? sender, PedestrianStatus e)
        {
            PedestrianStatus = e;
        }

        public override bool Equals(object? obj)
        {
            return obj is PedestrianViewModel model &&
                   EqualityComparer<Pedestrian>.Default.Equals(AssociatedPedestrian, model.AssociatedPedestrian);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AssociatedPedestrian);
        }

        public void Receive(TimerTickMessage message) => OnPropertyChanged(nameof(WaitTime));
    }
}
