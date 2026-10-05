namespace TripsTrapsTrull.Models;

public class Game
{
    public string[,] Board { get; private set; } = new string[3, 3];

    public string CurrentPlayer { get; private set; } = "X";

    public bool GameOver { get; private set; }

    public int XWins { get; private set; }
    public int OWins { get; private set; }
    public int Draws { get; private set; }

    public int GamesPlayed
    {
        get { return XWins + OWins + Draws; }
    }

    public Game()
    {
        LoadStatistics();
        Reset();
    }

    public void Reset()
    {
        Reset("X");
    }

    public void Reset(string startingPlayer)
    {
        Board = new string[3, 3];

        CurrentPlayer = startingPlayer;

        GameOver = false;
    }

    public bool MakeMove(int row, int column)
    {
        if (GameOver)
            return false;

        if (!string.IsNullOrEmpty(Board[row, column]))
            return false;

        Board[row, column] = CurrentPlayer;

        if (CheckWin(CurrentPlayer))
        {
            GameOver = true;

            if (CurrentPlayer == "X")
                XWins++;
            else
                OWins++;

            SaveStatistics();

            return true;
        }

        if (IsDraw())
        {
            GameOver = true;
            Draws++;

            SaveStatistics();

            return true;
        }

        CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";

        return true;
    }

    public bool CheckWin(string player)
    {
        for (int row = 0; row < 3; row++)
        {
            if (Board[row, 0] == player &&
                Board[row, 1] == player &&
                Board[row, 2] == player)
                return true;
        }

        for (int column = 0; column < 3; column++)
        {
            if (Board[0, column] == player &&
                Board[1, column] == player &&
                Board[2, column] == player)
                return true;
        }

        if (Board[0, 0] == player &&
            Board[1, 1] == player &&
            Board[2, 2] == player)
            return true;

        if (Board[0, 2] == player &&
            Board[1, 1] == player &&
            Board[2, 0] == player)
            return true;

        return false;
    }

    private bool IsDraw()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (string.IsNullOrEmpty(Board[row, column]))
                    return false;
            }
        }

        return true;
    }

    private void SaveStatistics()
    {
        Preferences.Default.Set("XWins", XWins);
        Preferences.Default.Set("OWins", OWins);
        Preferences.Default.Set("Draws", Draws);
    }

    private void LoadStatistics()
    {
        XWins = Preferences.Default.Get("XWins", 0);
        OWins = Preferences.Default.Get("OWins", 0);
        Draws = Preferences.Default.Get("Draws", 0);
    }

    public void ResetStatistics()
    {
        XWins = 0;
        OWins = 0;
        Draws = 0;

        Preferences.Default.Set("XWins", 0);
        Preferences.Default.Set("OWins", 0);
        Preferences.Default.Set("Draws", 0);
    }

}
