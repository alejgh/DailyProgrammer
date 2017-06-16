using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Extensions;

namespace ImageUtil.PGM
{
	public static class Parser
	{
		public static PGMImage ParseImage(string filePath)
		{
            String fileText = File.ReadAllText(filePath);
            IEnumerable<string> info = fileText.SplitWordsLazy().
										   Take((int)PGMInfo.TOTAL_ELEMENTS);

			string magic = info.ElementAt((int)PGMInfo.MAGIC);
            int nRows, nCols, maxCol;
			try
			{
				nRows = info.ElementAt((int)PGMInfo.ROWS).ToIntWrapper();
				nCols = info.ElementAt((int)PGMInfo.COLS).ToIntWrapper();
				maxCol = info.ElementAt((int)PGMInfo.MAX_COLOR_VAL).ToIntWrapper();
			}
			catch (Exception)
			{
				throw new FormatException("Invalid PGM file format.");
			}


            int[] pixelValues;

            if (PGMImage.plainPGMFormats.Contains(magic)) 
            {
                pixelValues = fileText.SplitWordsLazy().
                                      Skip((int)PGMInfo.TOTAL_ELEMENTS).
                                      Select(word => word.ToIntWrapper()).
                                      ToArray();
            } 
            else if (PGMImage.rawPGMFormats.Contains(magic))
            {
                int bytesToSkip = Encoding.ASCII.GetBytes(String.Join("", info)).Length 
                                          + (int)PGMInfo.TOTAL_ELEMENTS;
                byte[] fileBytes = File.ReadAllBytes(filePath);
                pixelValues = fileBytes.Skip(bytesToSkip).
                                       Select(b => Convert.ToInt32(b)).
                                       ToArray();
            } 
            else
                throw new FormatException("Invalid PGM format. Accepted formats " +
                                          "are: " + GetFormats() + ".");

            return new PGMImage(nRows, nCols, maxCol, magic, pixelValues);
		}

		internal static string GetFormats()
		{
			string ret = "";
            foreach (var format in PGMImage.acceptedPGMFormats)
			{
				ret += format + " - ";
			}
			ret.TrimEnd(new char[] { '-', ' ' });
			return ret;
		}
	}
}
