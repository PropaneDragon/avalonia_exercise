using AvaloniaExercise.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AvaloniaExercise.ViewModels
{
    public partial class TrafficLightStatusViewModel : ObservableObject
    {
        private readonly ITrafficLightService _trafficlightService;

        [ObservableProperty]
        private bool _isCrossing = false;

        [NotifyPropertyChangedFor(nameof(RedLightIsActive))]
        [NotifyPropertyChangedFor(nameof(YellowLightIsActive))]
        [NotifyPropertyChangedFor(nameof(GreenLightIsActive))]
        [ObservableProperty]
        private TrafficLightStatus _lastTrafficLightStatus;

        [NotifyPropertyChangedFor(nameof(TotalCrossingTime))]
        [ObservableProperty]
        private DateTime? _crossingTimeExpiryUtc;
        private DateTime _crossingTimeStartedUtc;

        public bool RedLightIsActive => LastTrafficLightStatus == TrafficLightStatus.Red;
        public bool YellowLightIsActive => LastTrafficLightStatus == TrafficLightStatus.Amber;
        public bool GreenLightIsActive => LastTrafficLightStatus == TrafficLightStatus.Green;

        public TimeSpan? TotalCrossingTime => CrossingTimeExpiryUtc - _crossingTimeStartedUtc;

        public TrafficLightStatusViewModel(ITrafficLightService trafficlightService)
        {
            _trafficlightService = trafficlightService;
            _trafficlightService.StatusChanged += TrafficlightService_StatusChanged;
            _trafficlightService.CrossingTimeExpiryUtcChanged += TrafficlightService_CrossingTimeExpiryUtcChanged;

            LastTrafficLightStatus = _trafficlightService.Status;
        }

        [RelayCommand]
        private async Task RequestCrossing()
        {
            await _trafficlightService.RequestCrossingAsync();
            IsCrossing = false;
        }

        private void TrafficlightService_StatusChanged(object? sender, TrafficLightStatus e) => LastTrafficLightStatus = e;
        private void TrafficlightService_CrossingTimeExpiryUtcChanged(object? sender, DateTime? e)
        {
            _crossingTimeStartedUtc = DateTime.UtcNow;
            CrossingTimeExpiryUtc = e;

            if (e.HasValue)
            {
                IsCrossing = true;
            }
        }
    }
}
