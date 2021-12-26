using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace RockScissorPaper
{
    /// <summary>
    /// Program class.
    /// </summary>
    public static class Program
    {
        private const string HelpSelection = "?";
        private const int ExitSelection = 0;

        private static readonly Dictionary<int, string> AvailableMoves = new ();
        private static int totalMoves;
        private static bool isRunning = true;

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            if (!ValidateParameters(args))
            {
                ShowExampleInput();
                return;
            }

            InitDictionary(args);
            Rules.FillTable(AvailableMoves);

            do
            {
                int computerMove = GenerateComputerMove();
                var hmacGenerator = new HmacGenerator();
                Console.WriteLine($"HMAC: {hmacGenerator.GenerateHmac(computerMove)}");

                ShowMenu();

                Console.Write("Enter your move: ");
                string userChoice = Console.ReadLine();

                if (userChoice.Equals(HelpSelection, StringComparison.Ordinal))
                {
                    HelpMe();
                    continue;
                }

                if (!int.TryParse(userChoice, out int userMove) || userMove < 0 || userMove > totalMoves)
                {
                    Console.WriteLine("Incorrect move. Try again.");
                    continue;
                }

                if (userMove == ExitSelection)
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
            foreach (var item in AvailableMoves)
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            }

            Console.WriteLine($"{ExitSelection} - exit");
            Console.WriteLine($"{HelpSelection} - help");
        }

        private static void ShowExampleInput()
        {
            Console.WriteLine("Example of correct input of parameters:");
            Console.WriteLine("RockScissorPaper.exe rock paper scissors");
        }

        private static int GenerateComputerMove() => RandomNumberGenerator.GetInt32(AvailableMoves.Count) + 1;

        private static void ShowResult(int move1, int move2)
        {
            Console.WriteLine($"Your move: {AvailableMoves[move1]}");
            Console.WriteLine($"Computer move: {AvailableMoves[move2]}");

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
                AvailableMoves.Add(counter, move);
                counter++;
            }

            totalMoves = AvailableMoves.Count;
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
