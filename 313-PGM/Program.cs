using System;
using ImageUtil;

namespace PGM
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ImageUtil.PGMParser.ParseImage("Files/P2/small.pgm");
        }
    }
}
