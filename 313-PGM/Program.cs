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
            { 'L', PGMImageOperations.RotateLeft },
            { 'H', PGMImageOperations.FlipHorizontally },
            { 'V', PGMImageOperations.FlipVertically },
            { 'B', PGMImageOperations.Brighten },
            { 'D', PGMImageOperations.Darken },
            { 'E', PGMImageOperations.Enlarge },
            { 'S', PGMImageOperations.Shrink },
            { 'N', PGMImageOperations.Negate },
            { 'C', PGMImageOperations.IncreaseContrast },
            { 'W', PGMImageOperations.DecreaseContrast }
        };

        public static void Main(string[] args)
        {
            if(args.Length != 2){
                Console.WriteLine("Invalid number of arguments.");
                Console.WriteLine("Usage: <pathtofile> <operations>.");
                Console.WriteLine("Supported operations:\n" +
                                  "- R: rotate the image to the right.\n" +
                                  "- L: rotate the image to the left.\n" +
                                  "- H: flip the image horizontally.\n" +
                                  "- V: flip the image vertically.\n" +
                                  "- B: brighten the image.\n" +
                                  "- D: darken the image.\n" +
                                  "- E: double the size of the image.\n" +
                                  "- S: halve the size of the image.\n" +
                                  "- N: invert the color values of the image.\n" +
                                  "- C: increase the contrast of the image\n" +
                                  "- W: decrease the contrast of the image.");
            }
            string filePath = args[0];
            string inputOperations = args[1].ToUpper();
            var img = ImageUtil.PGM.Parser.ParseImage(filePath);
            inputOperations = SimplifyOperations(inputOperations);
            foreach (char op in inputOperations)
            {
                if (operations.ContainsKey(op)) operations[op](img);
                else Console.WriteLine("Skipping invalid operation: '" + op + "'.");
            }

            string outputPath = filePath.Split('.')[0] + inputOperations + '.' +
                                        filePath.Split('.')[1];
            ImageUtil.PGM.Writer.WriteImage(img, outputPath);
        }

        private static string SimplifyOperations(string inputOperations)
        {
            return inputOperations.Replace("LR", "").Replace("RL", "").
                                  Replace("ES", "").Replace("SE", "").
                                  Replace("VV", "").Replace("HH", "").
                                  Replace("BD", "").Replace("DB", "").
                                  Replace("NN", "").Replace("CW", "").
                                  Replace("WC", "");
        }
    }
}
