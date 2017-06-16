using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Extensions;

namespace ImageUtil
{
	public class PGMImage
	{
        public readonly int numRows;
        public readonly int numCols;
        public readonly int maxColorVal;
        public readonly string magic;

        public int[] Lines
        {
            get;
            internal set;
        }

        public PGMImage(int rows, int cols, int maxColor, string magic, int[] lines) {
            numRows = rows;
            numCols = cols;
            maxColorVal = maxColor;
            this.magic = magic;
            Lines = lines;
        }
	}

	internal enum PGMInfo
	{
		MAGIC = 0,
		ROWS,
		COLS,
		MAX_COLOR_VAL,
		TOTAL_ELEMENTS
	}

    public static class PGMParser
    {

        private readonly static string[] rawPGMFormats = { "P2", "P5" };
        private readonly static string[] plainPGMFormats = { "P3", "P6" };

        public static PGMImage ParseImage(string filePath)
        {
            String file;
            using (StreamReader reader = File.OpenText(filePath))
			{
				file = reader.ReadToEnd();
            }

            IEnumerable<string> info = file.SplitWordsLazy().
                                           Take((int)PGMInfo.TOTAL_ELEMENTS);
            string magic = info.ElementAt((int)PGMInfo.MAGIC);
            int nRows, nCols, maxCol;
            try{
				nRows = info.ElementAt((int)PGMInfo.ROWS).ToIntWrapper();
                nCols = info.ElementAt((int)PGMInfo.COLS).ToIntWrapper();
                maxCol = info.ElementAt((int)PGMInfo.MAX_COLOR_VAL).ToIntWrapper();
            } catch (Exception) {
                throw new FormatException("Invalid PGM file format.");
            }

            string pixels = file.Substring(GetHeaderLength(file));
            int[] lines;
			/*
            if (rawPGMFormats.Contains(magic))
                lines = ParseRawImagePixels(pixels).Where(i => i >= 127).ToArray();
            else if (plainPGMFormats.Contains(magic))
                lines = ParsePlainImagePixels(pixels).ToArray();
            else
                throw new FormatException("Invalid PGM format. Formats accepted" +
                                          "are: " + GetFormats() + ".");
            */
			lines = ParsePlainImagePixels(pixels).ToArray();
            return new PGMImage(nRows, nCols, maxCol, magic, lines);
        }

        private static IEnumerable<int> ParsePlainImagePixels(string file)
        {
            return file.SplitWordsLazy().Select(w => w.ToIntWrapper());
        }

        private static IEnumerable<int> ParseRawImagePixels(string file)
        {
            return file.GetBytesLazy().Select(b => Convert.ToInt32(b));
		}

		private static int GetHeaderLength(string file)
		{
			int length = 0;
            int currentWord = 0;
			int totalWords = (int)PGMInfo.TOTAL_ELEMENTS;

            foreach (char ch in file)
			{
				length++;

                if (char.IsWhiteSpace(ch)){
                    currentWord++;
                    if (currentWord == totalWords) break;
                }
            }

            return length;
        }

        private static string GetFormats() {
            string ret = "";
            foreach (var format in rawPGMFormats)
            {
                ret += format + " - ";
            }
            foreach (var format in plainPGMFormats)
			{
				ret += format + " - ";
            }
            ret.TrimEnd(new char[] {'-', ' '});
            return ret;
        }
    }
}
