using System;
using System.Reflection;
using Nova.CodeAnalysis;
using Nova.CodeAnalysis.Binding;
using Nova.CodeAnalysis.Syntax;
using Binder = Nova.CodeAnalysis.Binding.Binder;

namespace nv
{
    internal static class Program
    {
        private static bool showTree = true;

        public static void Main()
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
                else if (line.ToLower() == "#help")
                {
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "Nova Interpreter - Version 1.0.0");
                    Console.WriteLine();
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "-------- COMMANDS --------");
                    Console.WriteLine("     ");
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "#showtree - Shows the parse tree of expressions.");
                    Console.WriteLine("     ");
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "#exit - Exits the REPL.");
                    Console.WriteLine("     ");
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "#clear - Clears the REPL.");
                    Console.WriteLine("     ");
                    Utilities.WriteLineAsColor(ConsoleColor.Yellow, "#help - Displays this message.");

                    continue;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);
                Binder binder = new Binder();
                BoundExpr boundExpr = binder.BindExpr(syntaxTree.Root);

                string[] diagnostics = binder.Diagnostics.Concat(syntaxTree.Diagnostics).ToArray();

                if (showTree)
                    syntaxTree.Root.PrettyPrint();

                if (binder.Diagnostics.Any())
                {
                    foreach (string diagnostic in diagnostics)
                        Utilities.WriteLineAsColor(ConsoleColor.Red, diagnostic);
                }
                else
                {
                    Evaluator e = new Evaluator(boundExpr);

                    Utilities.WriteLineAsColor(ConsoleColor.DarkGray, e.Evaluate());
                }
            }
        }
    }
}