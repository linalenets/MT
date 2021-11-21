using System;
using System.IO;
namespace Lexer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length < 1 || File.Exists(args[0]) is not true)
                return;
            var lexer = new Lexer();
            var tokens = lexer.Tokenize(File.ReadAllText(args[0]));
            foreach (var item in tokens)
            {
                Console.WriteLine(item.ToString());
            }
            foreach (var error in lexer.Errors)
                Console.WriteLine(error.ToString());
        }
    }
}
