namespace FitnessSurvey;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            Title = "Анкета фитнес-клуба",
            Width = 520,
            Height = 900
        };
    }
}
