using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nova.CodeAnalysis.Syntax;

namespace nv
{
    public static class Utilities
    {
        public static void WriteLineAsColor(ConsoleColor color, object msg) 
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void WriteAsColor(ConsoleColor color, object msg)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }

        public static void PrettyPrint(this Node node, string indent = "", bool isLast = true)
        {
            const string treeVerticalString = "│";
            const string treeNotLastString = "├──";
            const string treeLastString = "└──";
            
            string marker = isLast ? treeLastString : treeNotLastString;

            Utilities.WriteAsColor(ConsoleColor.Green, indent);
            Utilities.WriteAsColor(ConsoleColor.Green, marker);
            Utilities.WriteAsColor(ConsoleColor.Green, node.Kind);

            if (node is Token t && t.Value != null)
            {
                Console.Write(" ");
                Utilities.WriteAsColor(ConsoleColor.Green, t.Value);
            }

            Console.WriteLine();
            indent += isLast ? "    " : $"{treeVerticalString}    ";

            Node last = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == last);
        }
    }
}
