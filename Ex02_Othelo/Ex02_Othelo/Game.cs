using System;

public class Game
{
    private Board m_board;
    private Player m_player1;
    private Player m_player2;
    private Player m_currentPlayer;
    public void Start()
    {
        InitializeGame();
        PlayGame();
    }
    private void InitializeGame()
    {
        Console.WriteLine("Welcome to Othello!");
        int size;
        do
        {
            Console.WriteLine("Choose board size (6 or 8): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out size) || (size != 6 && size != 8))
            {
                Console.WriteLine("Invalid input. Please enter 6 or 8.");
            }

        } while (size != 6 && size != 8);

        m_board = new Board(size);
        Console.WriteLine("Enter Player 1 name: ");
        string player1Name = Console.ReadLine();
        m_player1 = new Player(player1Name, 'X', false);
        string playAgainstComputer;

        do
        {
            Console.WriteLine("Do you want to play against the computer? yes/no(if you say no its going to be 2 player mode): ");
            playAgainstComputer = Console.ReadLine().ToLower();

            if (playAgainstComputer != "yes" && playAgainstComputer != "no")
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }

        } while (playAgainstComputer != "yes" && playAgainstComputer != "no");
        if (playAgainstComputer == "yes")
        {
            m_player2 = new Player("Computer", 'O', true);
        }
        else
        {
            Console.WriteLine("Enter Player 2 name: ");
            string player2Name = Console.ReadLine();
            m_player2 = new Player(player2Name, 'O', false);
        }
        m_currentPlayer = m_player1;
    }
    private void PlayGame()
    {
        bool gameOver = false;

        while (!gameOver)
        {
            m_board.PrintBoard();
            DisplayCurrentScore();

            string move;
            int row = 0, col = 0;
            bool validMove = false;
            do
            {
                move = m_currentPlayer.GetMove(m_board);
                if (move == null)
                {
                    Console.WriteLine($"{m_currentPlayer.GetName()} has no valid moves. Skipping turn...");
                    SwitchPlayer();
                    continue;
                }
                if (move.Length < 2)
                {
                    Console.WriteLine("Invalid move. Please try again.");
                    continue;
                }
                if (!int.TryParse(move[0].ToString(), out row) || row < 1 || row > m_board.GetSize())
                {
                    Console.WriteLine("Invalid out of board move. Please try again.");
                    continue;
                }
                char column = char.ToUpper(move[1]);
                col = column - 'A';
                if (col < 0 || col >= m_board.GetSize())
                {
                    Console.WriteLine("Invalid out of board move. Please try again.");
                    continue;
                }
                row -= 1;

                if (m_board.IsValidMove(row, col, m_currentPlayer.GetToken()))
                {
                    validMove = true;
                }
                else
                {
                    Console.WriteLine($"The move {move} is not valid. Please select a legal move that flips at least one opponent's piece.");
                }

            } while (!validMove);
            m_board.PlaceMove(row, col, m_currentPlayer.GetToken());
            SwitchPlayer();
        }

        EndGame();
    }
    private void DisplayCurrentScore()
    {
        (int xCount, int oCount) = m_board.GetScore();
        Console.WriteLine();
        Console.WriteLine("Current Score:");
        Console.WriteLine($"Player 1 (X) score: {xCount}");
        Console.WriteLine($"Player 2 (O) score: {oCount}");
        Console.WriteLine();
    }
    private void SwitchPlayer()
    {
        m_currentPlayer = (m_currentPlayer == m_player1) ? m_player2 : m_player1;
    }
    private void EndGame()
    {
        m_board.PrintBoard();
        (int xCount, int oCount) = m_board.GetScore();
        Console.WriteLine("Game over!");
        Console.WriteLine($"Player 1 (X) score: {xCount}");
        Console.WriteLine($"Player 2 (O) score: {oCount}");
        if (xCount > oCount)
        {
            Console.WriteLine("Player 1 (X) wins!");
        }
        else if (oCount > xCount)
        {
            Console.WriteLine("Player 2 (O) wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}
