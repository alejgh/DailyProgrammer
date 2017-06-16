using System;
using System.Threading.Tasks;
using System.Linq;

namespace ImageUtil.PGM
{
	public class PGMImage
	{
		public int numRows;
		public int numCols;
		public readonly int maxColorVal;
        internal int numChannels;

        public string Magic
        {
            get;
            internal set;
        }

		public int[] PixelValues
		{
			get;
			internal set;
		}

		internal readonly static string[] plainPGMFormats = { "P2", "P6" };
		internal readonly static string[] rawPGMFormats = { "P3", "P5" };

		internal readonly static string[] acceptedPGMFormats = { "P2", "P3", "P5", "P6" };

		public PGMImage(int rows, int cols, int maxColor, string magic, int[] pixValues)
		{
			numRows = rows;
			numCols = cols;
			maxColorVal = maxColor;
			this.Magic = magic;
			PixelValues = pixValues;

            if (this.IsColorImage()) numChannels = 3; 
            else numChannels = 1;
        }

		public void ConvertTo(string newFormat)
		{
			if (!PGMImage.acceptedPGMFormats.Contains(this.Magic))
				throw new FormatException("Invalid PGM format. Accepted formats " +
										  "are: " + Parser.GetFormats() + ".");
			this.Magic = newFormat;
		}

		public PGMImage Copy()
		{
            int[] pixValuesCopy = new int[this.PixelValues.Length];
            this.PixelValues.CopyTo(pixValuesCopy, 0);
			return new PGMImage(this.numRows, this.numCols, this.maxColorVal,
                                this.Magic, pixValuesCopy);
		}

        public bool IsColorImage()
        {
            return this.Magic == "P3" || this.Magic == "P6";
        }

	}

    internal enum PGMInfo {
        MAGIC = 0,
        ROWS,
        COLS,
        MAX_COLOR_VAL,
        TOTAL_ELEMENTS
    }

    public static class PGMImageOperations {
        // we write the operations as an extesion so we can use
        // the dictionary of actions of the main program.
        // using pgm images will behave exactly the same as if
        // these functions where defined in the class

        public static void RotateRight(this PGMImage image) 
        {
			PGMImage temp = image.Copy();
			image.numCols = temp.numRows;
			image.numRows = temp.numCols;

            Action<PGMImage, PGMImage, int, int, int> operationFunc;
            operationFunc = (img, copy, i, j, k) =>
                                     img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
                                         copy.PixelValues[(copy.numRows-j-1)*copy.numCols*copy.numChannels+k+i*copy.numChannels];


            ApplyOperationInParallel(image, temp, operationFunc);
        }

        public static void RotateLeft(this PGMImage image)
        {
            PGMImage temp = image.Copy();
            image.numCols = temp.numRows;
            image.numRows = temp.numCols;

			Action<PGMImage, PGMImage, int, int, int> operationFunc;
			operationFunc = (img, copy, i, j, k) =>
							img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
							    copy.PixelValues[copy.numCols * copy.numChannels - (copy.numChannels - 1 - k)
                                                    - i * copy.numChannels - 1 + j * copy.numCols * copy.numChannels];


			ApplyOperationInParallel(image, temp, operationFunc);
            
        }

        public static void RotateLeftCopy(this PGMImage img)
		{
			PGMImage copy = img.Copy();
			img.numCols = copy.numRows;
			img.numRows = copy.numCols;

			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
						img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
	                         copy.PixelValues[copy.numCols * copy.numChannels - (copy.numChannels - 1 - k)
						  - i * copy.numChannels - 1 + j * copy.numCols * copy.numChannels];
			});
            
        }


        private static void ApplyOperationInParallel(PGMImage img, PGMImage temp,
                                                     Action<PGMImage, PGMImage, int, int, int> operationFunc)
        {
			Parallel.For(0, img.numRows, i =>
			{
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
                        operationFunc(img, temp, i, j, k);
			});
        }
    }
}
