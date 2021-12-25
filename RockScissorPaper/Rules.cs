using System;
using System.Collections.Generic;

namespace RockScissorPaper
{
    static class Rules
    {
        private const string DrawText = "Draw";
        private const string WinText = "Win";
        private const string LoseText = "Lose";

        public static string[][] Table { get; set; }

        public static void FillTable(Dictionary<int, string> moves)
        {
            InitTable(moves);

            for (int i = 1; i < moves.Count + 1; i++)
            {
                for (int j = 1; j < moves.Count + 1; j++)
                {
                    Table[i][j] = GetWinner(i, j) switch
                    {
                        0 => DrawText,
                        1 => WinText,
                        2 => LoseText,
                        _ => throw new ArgumentException("Incorrect result."),
                    };
                }
            }
        }

        public static int GetWinner(int player1Move, int player2Move)
        {
            if (player1Move == player2Move)
            {
                return 0;
            }

            return (Math.Abs(player1Move - player2Move) % 2) switch
            {

                0 => player1Move > player2Move ? 1 : 2,
                1 => player1Move > player2Move ? 2 : 1,
                _ => throw new ArgumentException("Something goes wrong."),
            };
        }

        private static void InitTable(Dictionary<int, string> moves)
        {
            Table = new string[moves.Count + 1][];
            for (int i = 0; i < Table.Length; i++)
            {
                Table[i] = new string[moves.Count + 1];
            }

            foreach (var move in moves)
            {
                Table[0][move.Key] = move.Value;
                Table[move.Key][0] = move.Value;
            }
        }
    }
}
