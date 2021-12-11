using System;
using Nova.CodeAnalysis;

namespace Nova
{
    internal class Program
    { 
        private const string TreeVerticalString = "│";
        private const string TreeNotLastString = "├──";
        private const string TreeLastString = "└──";

        private static bool showTree = true;

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("nova> ");
                string line = Console.ReadLine();

                if (line.ToLower() == "#showtree")
                {
                    showTree = !showTree;

                    Utilities.WriteLineAsColor(ConsoleColor.Cyan, showTree ? "[INF] Showing parse trees." : "[INF] Not showing parse trees.");

                    continue;
                } 
                else if (line.ToLower() == "#exit")
                {
                    Environment.Exit(0);
                } 
                else if (line.ToLower() == "#clear")
                {
                    Console.Clear();
                    continue;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);

                if (showTree)
                    PrettyPrint(syntaxTree.Root);

                if (syntaxTree.Diagnostics.Any())
                {
                    foreach (string diagnostic in syntaxTree.Diagnostics)
                        Utilities.WriteLineAsColor(ConsoleColor.Red, diagnostic);
                }
                else
                {
                    Evaluator e = new Evaluator(syntaxTree.Root);

                    Utilities.WriteLineAsColor(ConsoleColor.DarkGray, e.Evaluate());
                }
            }
        }

        static void PrettyPrint(Node node, string indent = "", bool isLast = true)
        {
            string marker = isLast ? TreeLastString : TreeNotLastString;

            Utilities.WriteAsColor(ConsoleColor.Green, indent);
            Utilities.WriteAsColor(ConsoleColor.Green, marker);
            Utilities.WriteAsColor(ConsoleColor.Green, node.Type);

            if (node is Token t && t.Value != null)
            {
                Console.Write(" ");
                Utilities.WriteAsColor(ConsoleColor.Green, t.Value);
            }

            Console.WriteLine();
            indent += isLast ? "    " : $"{TreeVerticalString}    ";

            Node last = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == last);
        }
    }
}