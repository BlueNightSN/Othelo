using System;
using System.Collections.Generic;

public class Player
{
    private string m_name;
    private char m_token;
    private bool m_isComputer;
    public Player(string name, char token, bool isComputer)
    {
        this.m_name = name;
        this.m_token = token;
        this.m_isComputer = isComputer;
    }
    public string GetName()
    {
        return m_name;
    }
    public char GetToken()
    {
        return m_token;
    }
    public string GetMove(Board board)
    {
        if(m_isComputer)
        {
            return GetComputerMove(board);
        }
        else
        {
            return GetHumanMove();
        }
    }
    private string GetHumanMove() 
    {
        Console.WriteLine($"{m_name}'s turn. Enter your move(number then letter without space):");
        string move = Console.ReadLine();
        if (move.ToUpper() == "Q")
        {
            Console.WriteLine($"{m_name} has quit the game");
            Environment.Exit(0);
        }
        return move;
    }
    private string GetComputerMove(Board board)
    {
        List<string> validMoves = GetAllValidMoves(board);


        if (validMoves.Count == 0)
        {
            Console.WriteLine("No valid moves for the computer. Skipping turn...");
            return null;
        }


        Random rand = new Random();

        string move = validMoves[rand.Next(validMoves.Count)];

        return move;

    }

    private List<string> GetAllValidMoves(Board board)
    {
        List<string> validMoves = new List<string>();

        for (int row = 0; row < board.GetSize(); row++)
        {
            for (int col = 0; col < board.GetSize(); col++)
            {
                if (board.IsValidMove(row, col, m_token))
                {
                    validMoves.Add($"{row + 1}{(char)(col + 'A')}");
                }
            }
        }
        return validMoves;
    }

}