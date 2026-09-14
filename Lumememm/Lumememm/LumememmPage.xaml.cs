namespace Lumememm;

public partial class MainPage : ContentPage
{
    private readonly Random random = new Random();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnActionClicked(object sender, EventArgs e)
    {
        string action = ActionPicker.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(action))
        {
            await DisplayAlert("Teade", "Palun vali tegevus!", "OK");
            return;
        }

        ActionLabel.Text = $"Valitud tegevus: {action}";

        switch (action)
        {
            case "Peida lumememm":
                HideSnowman();
                break;

            case "Näita lumememm":
                ShowSnowman();
                break;

            case "Muuda värvi":
                await ChangeColor();
                break;

            case "Sulata":
                await MeltSnowman();
                break;

            case "Tantsi":
                await DanceSnowman();
                break;
        }
    }

    private void HideSnowman()
    {
        SnowmanLayout.IsVisible = false;
    }

    private void ShowSnowman()
    {
        SnowmanLayout.IsVisible = true;
        SnowmanLayout.Opacity = 1;
        SnowmanLayout.Scale = 1;
    }

    private async Task ChangeColor()
    {
        bool answer = await DisplayAlert(
            "Muuda värvi",
            "Kas soovid lumememme värvi muuta?",
            "Jah",
            "Ei");

        if (!answer)
            return;

        Color[] colors =
        {
            Colors.LightBlue,
            Colors.LightPink,
            Colors.LightGreen,
            Colors.LightYellow,
            Colors.LightGray,
            Colors.Purple
        };

        Color newColor = colors[random.Next(colors.Length)];

        Body.BackgroundColor = newColor;
        Head.BackgroundColor = newColor;
    }

    private async Task MeltSnowman()
    {
        SnowmanLayout.IsVisible = true;

        // Stepperi väärtus määrab animatsiooni kiiruse.
        // Suurem väärtus = kiirem sulamine.
        double speed = SpeedStepper.Value;

        uint duration = (uint)(2000 / speed);

        await Task.WhenAll(
            SnowmanLayout.FadeTo(0, duration),
            SnowmanLayout.ScaleTo(0.2, duration)
        );

        SnowmanLayout.IsVisible = false;

        // Taastame suuruse, et "Näita" toimiks korrektselt.
        SnowmanLayout.Opacity = 1;
        SnowmanLayout.Scale = 1;
    }

    private async Task DanceSnowman()
    {
        SnowmanLayout.IsVisible = true;

        double speed = SpeedStepper.Value;

        // Mida suurem Stepperi väärtus,
        // seda kiirem on tants.
        uint duration = (uint)(500 / speed);

        for (int i = 0; i < 3; i++)
        {
            await SnowmanLayout.TranslateTo(-50, 0, duration);
            await SnowmanLayout.TranslateTo(50, 0, duration);
        }

        await SnowmanLayout.TranslateTo(0, 0, duration);
    }

    private void OnOpacityChanged(object sender, ValueChangedEventArgs e)
    {
        SnowmanLayout.Opacity = e.NewValue;
    }
}
