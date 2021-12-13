using System;
using System.Reflection;
using Nova.CodeAnalysis;
using Nova.CodeAnalysis.Binding;
using Nova.CodeAnalysis.Syntax;
using Binder = Nova.CodeAnalysis.Binding.Binder;

namespace Nova
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