using Ex02.ConsoleUtils;
using System;
using System.Diagnostics.Contracts;

public class Board
{
    private int m_SizeOfBoard;
    private char[,] m_BoardGrid;
    public Board(int size)
    {
        this.m_SizeOfBoard = size;
        m_BoardGrid = new char[size, size];
        InitializeBoard();
    }
    private void InitializeBoard()
    {
        for (int i = 0; i < m_SizeOfBoard; i++)
        {
            for (int j = 0; j < m_SizeOfBoard; j++)
            {
                m_BoardGrid[i, j] = ' ';
            }
        }

        int mid = m_SizeOfBoard / 2;
        m_BoardGrid[mid - 1, mid - 1] = 'O'; 
        m_BoardGrid[mid, mid] = 'O';
        m_BoardGrid[mid - 1, mid] = 'X';
        m_BoardGrid[mid, mid - 1] = 'X';
    }
    public void PrintBoard()
    {
        Screen.Clear();
        Console.Write("    ");
        for (int i = 0; i < m_SizeOfBoard; i++)
        {
            Console.Write("" + (char)('A' + i) + "   ");
        }
        Console.WriteLine();

        for (int i = 0; i < m_SizeOfBoard; i++)
        {
            Console.Write((i + 1) + " |");
            for (int j = 0; j < m_SizeOfBoard; j++)
            {
                Console.Write(" " + m_BoardGrid[i, j] + " |");
            }
            Console.WriteLine();
            Console.Write("  ");
            for (int j = 0; j < m_SizeOfBoard; j++)
            {
                Console.Write("====");
            }
            Console.WriteLine("===");
        }
    }
    public bool IsValidMove(int row, int col, char playerToken)
    {
        //Console.WriteLine($"Checking if move at ({row}, {col}) is valid for player {playerToken}");

        // Check if the cell is empty and within the board bounds
        if (row < 0 || row >= m_SizeOfBoard || col < 0 || col >= m_SizeOfBoard || m_BoardGrid[row, col] != ' ')
        {
            Console.WriteLine($"Move at ({row}, {col}) is out of bounds or the cell is already occupied.");
            return false;
        }

        // Check if the move will flip any of the opponent's pieces
        bool isValid = CheckAndFlip(row, col, playerToken, false);
        //Console.WriteLine(isValid ? $"Move at ({row}, {col}) is valid." : $"Move at ({row}, {col}) is not valid.");
        return isValid;
    }




    private bool CheckAndFlip(int row, int col, char playerToken, bool flip)
    {
       // Console.WriteLine($"Checking move for player {playerToken} at ({row}, {col})");

        bool isValid = false;
        char opponentToken = (playerToken == 'X') ? 'O' : 'X';

        // Check in all eight directions (horizontal, vertical, diagonal)
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, 1, 0, flip);  // Right
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, -1, 0, flip); // Left
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, 0, 1, flip);  // Down
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, 0, -1, flip); // Up
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, 1, 1, flip);  // Diagonal down-right
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, 1, -1, flip); // Diagonal down-left
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, -1, 1, flip); // Diagonal up-right
        isValid |= CheckDirectionAndFlip(row, col, playerToken, opponentToken, -1, -1, flip); // Diagonal up-left

       // Console.WriteLine(isValid ? "Move will flip opponent pieces." : "Move will not flip any pieces.");
        return isValid;
    }



    private bool CheckDirectionAndFlip(int row, int col, char playerToken, char opponentToken, int rowDir, int colDir, bool flip)
    {
        int r = row + rowDir;
        int c = col + colDir;
        bool foundOpponent = false;

       // Console.WriteLine($"Checking direction ({rowDir}, {colDir}) from ({row}, {col}) for player {playerToken}");

        // Traverse in the direction specified by rowDir and colDir
        while (r >= 0 && r < m_SizeOfBoard && c >= 0 && c < m_SizeOfBoard && m_BoardGrid[r, c] == opponentToken)
        {
            foundOpponent = true;
            r += rowDir;
            c += colDir;
        }

        // Check if we find a piece belonging to the current player that closes off the opponent's pieces
        if (foundOpponent && r >= 0 && r < m_SizeOfBoard && c >= 0 && c < m_SizeOfBoard && m_BoardGrid[r, c] == playerToken)
        {
            if (flip)
            {
               // Console.WriteLine($"Flipping opponent pieces in direction ({rowDir}, {colDir})");
                r = row + rowDir;
                c = col + colDir;
                while (m_BoardGrid[r, c] == opponentToken)
                {
                    m_BoardGrid[r, c] = playerToken;
                    r += rowDir;
                    c += colDir;
                }
            }
            return true;  // Valid move in this direction
        }

        //Console.WriteLine($"No valid move in direction ({rowDir}, {colDir})");
        return false;  // No valid move in this direction
    }



    public void PlaceMove(int row, int col, char playerToken)
    {
        //Console.WriteLine($"Placing move for player {playerToken} at ({row}, {col})");
        CheckAndFlip(row, col, playerToken, true);  // Pass 'true' to flip pieces
        m_BoardGrid[row, col] = playerToken;  // Place the player's piece
    }

    public int GetSize()
    {
        return m_SizeOfBoard;
    }
    public (int xCount, int oCount) GetScore()
    {
        int xCount = 0;
        int oCount = 0;

        // Loop through the board to count the number of 'X' and 'O' pieces
        for (int i = 0; i < m_SizeOfBoard; i++)
        {
            for (int j = 0; j < m_SizeOfBoard; j++)
            {
                if (m_BoardGrid[i, j] == 'X') xCount++;
                if (m_BoardGrid[i, j] == 'O') oCount++;
            }
        }

        return (xCount, oCount);
    }


}