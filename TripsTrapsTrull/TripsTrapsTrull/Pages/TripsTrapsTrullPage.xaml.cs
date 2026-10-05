using TripsTrapsTrull.Models;
using TripsTrapsTrull.Pages;

namespace TripsTrapsTrull;

public partial class MainPage : ContentPage
{
    private readonly Game game = new Game();

    private readonly Button[,] buttons = new Button[3, 3];

    public MainPage()
    {
        InitializeComponent();

        CreateGameBoard();
        UpdateUI();
    }

    private void CreateGameBoard()
    {
        GameGrid.RowDefinitions.Clear();
        GameGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < 3; i++)
        {
            GameGrid.RowDefinitions.Add(
                new RowDefinition { Height = GridLength.Star });

            GameGrid.ColumnDefinitions.Add(
                new ColumnDefinition { Width = GridLength.Star });
        }

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                int currentRow = row;
                int currentColumn = column;

                Button button = new Button
                {
                    Text = "",
                    FontSize = 70,
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black
                };

                button.Clicked += (sender, e) =>
                {
                    Cell_Clicked(currentRow, currentColumn);
                };

                buttons[row, column] = button;

                GameGrid.Add(button, column, row);
            }
        }
    }

    private async void Cell_Clicked(int row, int column)
    {
        if (!game.MakeMove(row, column))
            return;

        UpdateUI();

        string player = game.Board[row, column];

        if (game.GameOver)
        {
            bool won = game.CheckWin(player);

            if (won)
            {
                bool playAgain = await DisplayAlertAsync(
                    "Mäng läbi",
                    $"{player} võitis! Kas soovid veel mängida?",
                    "Jah",
                    "Ei");

                if (playAgain)
                    StartNewGame();
            }
            else
            {
                bool playAgain = await DisplayAlertAsync(
                    "Viik",
                    "Mäng jäi viiki! Kas soovid veel mängida?",
                    "Jah",
                    "Ei");

                if (playAgain)
                    StartNewGame();
            }
        }
    }

    private void StartNewGame()
    {
        game.Reset();
        UpdateUI();
    }

    private void NewGame_Clicked(object sender, EventArgs e)
    {
        StartNewGame();
    }

    private async void WhoStarts_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Kes alustab?",
            $"{game.CurrentPlayer} alustab!",
            "OK");
    }


    private async void Rules_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RulesPage());

    }

    private async void Statistics_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatisticsPage(game));
    }



    private void UpdateUI()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                buttons[row, column].Text =
                    game.Board[row, column];

                if (game.Board[row, column] == "X")
                    buttons[row, column].TextColor = Colors.Red;
                else if (game.Board[row, column] == "O")
                    buttons[row, column].TextColor = Colors.Blue;
                else
                    buttons[row, column].TextColor = Colors.Black;
            }
        }

        if (!game.GameOver)
            TurnLabel.Text = $"{game.CurrentPlayer} kord";
        else
            TurnLabel.Text = "Mäng läbi";

        ScoreLabel.Text =
            $"X: {game.XWins}   O: {game.OWins}   Viigid: {game.Draws}";
    }

    public void UpdateStatisticsUI()
    {
        UpdateUI();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        UpdateUI();
    }




}

internal class RulesPage : Page
{
}