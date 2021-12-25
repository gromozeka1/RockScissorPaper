using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace RockScissorPaper
{
    public static class Program
    {
        private static readonly Dictionary<int, string> availableMoves = new ();
        private static int totalMoves;
        private static bool isRunning = true;
        private static readonly string helpSelection = "?";
        private static readonly int exitSelection = 0;


        static void Main(string[] args)
        {
            if (!ValidateParameters(args))
            {
                Console.WriteLine("Example of correct input of parameters:");
                Console.WriteLine("RockScissorPaper.exe rock paper scissors");
                return;
            }

            InitDictionary(args);
            Rules.FillTable(availableMoves);

            do
            {
                int computerMove = GenerateComputerMove();
                var hmacGenerator = new HmacGenerator();
                Console.WriteLine($"HMAC: {hmacGenerator.GenerateHmac(computerMove)}");

                ShowMenu();

                Console.Write("Enter your move: ");
                string userChoice = Console.ReadLine();

                if (userChoice.Equals(helpSelection, StringComparison.InvariantCulture))
                {
                    HelpMe();
                    continue;
                }

                if (!int.TryParse(userChoice, out int userMove) || userMove < 0 || userMove > totalMoves)
                {
                    Console.WriteLine("Incorrect move. Try again.");
                    continue;
                }

                if (userMove == exitSelection)
                {
                    Exit();
                    continue;
                }

                ShowResult(userMove, computerMove);
                ShowFinalMessage(hmacGenerator);
            }
            while (isRunning);
        }

        private static bool ValidateParameters(string[] parameters)
        {
            if (parameters.Length < 3 || parameters.Length % 2 == 0)
            {
                Console.WriteLine("Incorrect number of input parameters. Number of parameters should be even and not less 3.");
                return false;
            }

            var duplicateValues = parameters
                .GroupBy(e => e)
                .Where(e => e.Count() > 1).ToArray();
            
            if (duplicateValues.Length > 0)
            {
                Console.WriteLine("There are duplicate values of parameters:");
                foreach (var values in duplicateValues)
                {
                    Console.WriteLine(values.Key);
                }
                return false;
            }

            return true;
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Available moves:");
            foreach (var item in availableMoves)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }
            Console.WriteLine($"{exitSelection} - exit");
            Console.WriteLine($"{helpSelection} - help");
        }

        private static int GenerateComputerMove() => RandomNumberGenerator.GetInt32(availableMoves.Count) + 1;

        private static void ShowResult(int move1, int move2)
        {
            Console.WriteLine($"Your move: {availableMoves[move1]}");
            Console.WriteLine($"Computer move: {availableMoves[move2]}");

            string message = Rules.GetWinner(move1, move2) switch
            {
                0 => "Draw. Try again.",
                1 => "You win!",
                2 => "You are looooser.",
                _ => throw new ArgumentException("Incorrect value {winner} for winner."),
            };

            Console.WriteLine(message);


        }

        private static void InitDictionary(string[] moves)
        {
            int counter = 1;

            foreach (var move in moves)
            {
                availableMoves.Add(counter, move);
                counter++;
            }

            totalMoves = availableMoves.Count;
        }

        private static void HelpMe()
        {
            Console.WriteLine("Available pairs of moves and their result:");
            Help.ShowTable();
        }

        private static void Exit()
        {
            Console.WriteLine("Goodbye. See you later.");
            isRunning = false;
        }

        private static void ShowFinalMessage(HmacGenerator hmacGenerator)
        {
            Console.WriteLine($"HMAC key: {hmacGenerator.Key}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
