using TripsTrapsTrull.Models;

namespace TripsTrapsTrull.Pages;

public partial class StatisticsPage : ContentPage
{
    private readonly Game game;

    public StatisticsPage(Game game)
    {
        InitializeComponent();

        this.game = game;

        UpdateStatistics();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        GamesLabel.Text =
            $"Mänge kokku: {game.XWins + game.OWins + game.Draws}";

        XWinsLabel.Text =
            $"X võite: {game.XWins}";

        OWinsLabel.Text =
            $"O võite: {game.OWins}";

        DrawsLabel.Text =
            $"Viike: {game.Draws}";
    }

    private async void ResetStatistics_Clicked(
        object sender,
        EventArgs e)
    {
        bool answer = await DisplayAlertAsync(
            "Kustuta statistika",
            "Kas oled kindel, et soovid kogu statistika kustutada?",
            "Jah",
            "Ei");

        if (!answer)
            return;

        // Muudab Game objekti statistika nulliks
        game.ResetStatistics();

        // Näitab kohe statistika lehel nulle
        UpdateStatistics();

        // Läheme tagasi MainPage'ile
        await Navigation.PopAsync();
    }

    private async void Back_Clicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
