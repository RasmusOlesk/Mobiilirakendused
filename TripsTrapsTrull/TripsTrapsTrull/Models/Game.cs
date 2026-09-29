namespace TripsTrapsTrull.Models;

public class Game
{
    public string[,] Board { get; private set; } = new string[3, 3];

    public string CurrentPlayer { get; private set; } = "X";

    public bool GameOver { get; private set; }

    public int XWins { get; private set; }
    public int OWins { get; private set; }
    public int Draws { get; private set; }

    public Game()
    {
        Reset();
    }

    public void Reset()
    {
        Board = new string[3, 3];
        CurrentPlayer = "X";
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

            return true;
        }

        if (IsDraw())
        {
            GameOver = true;
            Draws++;
            return true;
        }

        CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";

        return true;
    }

    public bool CheckWin(string player)
    {
        // Read
        for (int row = 0; row < 3; row++)
        {
            if (Board[row, 0] == player &&
                Board[row, 1] == player &&
                Board[row, 2] == player)
                return true;
        }

        // Veerud
        for (int column = 0; column < 3; column++)
        {
            if (Board[0, column] == player &&
                Board[1, column] == player &&
                Board[2, column] == player)
                return true;
        }

        // Diagonaalid
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
}
