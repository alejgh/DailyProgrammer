using System;
using System.Diagnostics;
using System.Collections.Generic;
using ImageUtil.PGM;

namespace DailyProgrammer
{
    class PGM_313
    {

        private static readonly IDictionary<char, Action<PGMImage>> operations =
            new Dictionary<char, Action<PGMImage>>()
        {
            { 'R', PGMImageOperations.RotateRight },
            { 'L', PGMImageOperations.RotateLeft }
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("Reading file...");
            var alibabaImg = ImageUtil.PGM.Parser.ParseImage("Files/P5/medium.pgm");
            Console.WriteLine("File read!");
            MeasureTime(alibabaImg.RotateLeft);
			MeasureTime(alibabaImg.RotateLeftCopy);
            Console.WriteLine("Writing file...");
			ImageUtil.PGM.Writer.WriteImage(alibabaImg, "Files/P5/mediumRR.pgm");
			Console.WriteLine("Finished!");
        }

        private static void MeasureTime(Action act)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
            act();
			sw.Stop();
            Console.WriteLine("Time elapsed: {0} milliseconds.", sw.Elapsed.Milliseconds);
        }
    }
}
