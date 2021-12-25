using System;
using ConsoleTables;

namespace RockScissorPaper
{
    static class Help
    {
        public static void ShowTable()
        {
            var consoleTable = new ConsoleTable(Rules.Table[0]);
            foreach (var row in Rules.Table[1..])
            {
                consoleTable.AddRow(row);
            }

            consoleTable.Write(Format.Alternative);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
