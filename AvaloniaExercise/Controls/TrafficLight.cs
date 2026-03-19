using Avalonia;
using Avalonia.Controls.Primitives;

namespace AvaloniaExercise.Controls;

public class TrafficLight : TemplatedControl
{
    public static readonly StyledProperty<bool> RedEnabledProperty = AvaloniaProperty.Register<TrafficLight, bool>(nameof(RedEnabled), true);
    public static readonly StyledProperty<bool> YellowEnabledProperty = AvaloniaProperty.Register<TrafficLight, bool>(nameof(YellowEnabled), false);
    public static readonly StyledProperty<bool> GreenEnabledProperty = AvaloniaProperty.Register<TrafficLight, bool>(nameof(GreenEnabled), false);
    public static readonly StyledProperty<double> LightSizeProperty = AvaloniaProperty.Register<TrafficLight, double>(nameof(LightSize), 16d);
    public static readonly StyledProperty<Thickness> LightMarginProperty = AvaloniaProperty.Register<TrafficLight, Thickness>(nameof(LightMargin));

    public bool RedEnabled
    {
        get => GetValue(RedEnabledProperty);
        set => SetValue(RedEnabledProperty, value);
    }

    public bool YellowEnabled
    {
        get => GetValue(YellowEnabledProperty);
        set => SetValue(YellowEnabledProperty, value);
    }

    public bool GreenEnabled
    {
        get => GetValue(GreenEnabledProperty);
        set => SetValue(GreenEnabledProperty, value);
    }

    public double LightSize
    {
        get => GetValue(LightSizeProperty);
        set => SetValue(LightSizeProperty, value);
    }

    public Thickness LightMargin
    {
        get => GetValue(LightMarginProperty);
        set => SetValue(LightMarginProperty, value);
    }
}