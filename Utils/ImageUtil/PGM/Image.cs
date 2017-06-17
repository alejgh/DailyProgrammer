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

		internal readonly static string[] plainPGMFormats = { "P2", "P3" };
		internal readonly static string[] rawPGMFormats = { "P6", "P5" };

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
		COLS,
        ROWS,
        MAX_COLOR_VAL,
        TOTAL_ELEMENTS
    }

    public static class PGMImageOperations {
        // we write the operations as an extesion so we can use
        // the dictionary of actions of the main program.
        // using pgm images will behave exactly the same as if
        // these functions where defined in the class


        static private float contrastAmount = 60.0f;
        static public float ContrastAmount
        {
            get {
                return contrastAmount;
            }
            set {
                if (value > 0.0f)
                    contrastAmount = value;
            }
        }

		static private int brightenAmount = 40;
		static public int BrightenAmount
		{
			get
			{
				return brightenAmount;
			}
			set
			{
				if (value > 0)
					brightenAmount = value;
			}
		}

        public static void RotateRight(this PGMImage img) 
        {
			PGMImage temp = img.Copy();
			img.numCols = temp.numRows;
			img.numRows = temp.numCols;

			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
						img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
												 temp.PixelValues[(temp.numRows - j - 1) * temp.numCols * 
                                                                  temp.numChannels + k + i * temp.numChannels];
			});
        }

        public static void RotateLeft(this PGMImage img)
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

        public static void FlipHorizontally(this PGMImage img)
		{
			PGMImage copy = img.Copy();

            /*
            Parallel.For(0, img.numRows, i =>
            {
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
						img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
							 copy.PixelValues[i * copy.numCols * copy.numChannels 
                                              + copy.numCols * copy.numChannels -
                                              (copy.numChannels - k - 1) - 
                                              j * copy.numChannels - 1];
            });
            */

            for (int i = 0; i < img.numRows; i++)
            {
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
                    {
                        int index1 = j * img.numChannels + k + img.numCols * i * img.numChannels;
                        int index2 = i * copy.numCols * copy.numChannels
                                                  + copy.numCols * copy.numChannels -
                                                  (copy.numChannels - k - 1) -
                                             j * copy.numChannels - 1;
                        img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
                             copy.PixelValues[i * copy.numCols * copy.numChannels
                                              + copy.numCols * copy.numChannels -
                                              (copy.numChannels - k - 1) -
                                              j * copy.numChannels - 1];
                    }
            }
        }

        public static void FlipVertically(this PGMImage img)
        {
			PGMImage copy = img.Copy();

            Parallel.For(0, img.numRows, i =>
            {
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
                        img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
                               copy.PixelValues[(copy.numRows - i - 1) * copy.numCols
                                                * copy.numChannels + k + j * copy.numChannels];

            });			
        }

		public static void Darken(this PGMImage img)
		{
			PGMImage copy = img.Copy();

			Parallel.For(0, img.numRows, i =>
			{
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
                    {
                        int index = j * img.numChannels + k + img.numCols * i * img.numChannels;
                        img.PixelValues[index] = Math.Max(img.PixelValues[index] - BrightenAmount, 0);
                    }
			});
		}

		public static void Brighten(this PGMImage img)
		{
			PGMImage copy = img.Copy();

			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
					{
						int index = j * img.numChannels + k + img.numCols * i * img.numChannels;
                        img.PixelValues[index] = Math.Min(img.PixelValues[index] + BrightenAmount,
                                                          img.maxColorVal);
					}
			});
		}

		public static void Negate(this PGMImage img)
		{
			PGMImage copy = img.Copy();

			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
					{
						int index = j * img.numChannels + k + img.numCols * i * img.numChannels;
                        img.PixelValues[index] = img.maxColorVal - img.PixelValues[index];
					}
			});
		}

		public static void Enlarge(this PGMImage img)
		{
			PGMImage copy = img.Copy();
            img.numRows *= 2;
            img.numCols *= 2;
            img.PixelValues = new int[img.numRows * img.numCols];
		

			Parallel.For(0, img.numRows, i =>
			{
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
                        img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
                               copy.PixelValues[(i / 2) * copy.numCols * copy.numChannels +
                                                (j / 2) * copy.numChannels + k];
			});
		}

		public static void Shrink(this PGMImage img)
		{
			PGMImage copy = img.Copy();
			img.numRows /= 2;
			img.numCols /= 2;
			img.PixelValues = new int[img.numRows * img.numCols];


			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
						img.PixelValues[j * img.numChannels + k + img.numCols * i * img.numChannels] =
							   copy.PixelValues[(i * 2) * copy.numCols * copy.numChannels +
												(j * 2) * copy.numChannels + k];
			});
		}

		public static void IncreaseContrast(this PGMImage img)
		{
			PGMImage copy = img.Copy();

            double contrastMagic = (259.0 * (ContrastAmount + 255.0)) 
                / (255.0 * (259.0 - ContrastAmount));

			Parallel.For(0, img.numRows, i =>
			{
                for (int j = 0; j < img.numCols; j++)
                    for (int k = 0; k < img.numChannels; k++)
					{
						int index = j * img.numChannels + k + img.numCols * i * img.numChannels;
                        img.PixelValues[index] = Truncate(contrastMagic * (img.PixelValues[index] 
                                                                           - 128.0) + 128.0, img);
                    }
            });
		}

		public static void DecreaseContrast(this PGMImage img)
		{
			PGMImage copy = img.Copy();

			double contrastMagic = (259.0 * (-ContrastAmount + 255.0))
				/ (255.0 * (259.0 + ContrastAmount));

			Parallel.For(0, img.numRows, i =>
			{
				for (int j = 0; j < img.numCols; j++)
					for (int k = 0; k < img.numChannels; k++)
					{
						int index = j * img.numChannels + k + img.numCols * i * img.numChannels;
						img.PixelValues[index] = Truncate(contrastMagic * (img.PixelValues[index] 
                                                                           - 128.0) + 128.0, img);
					}
			});
		}

        private static int Truncate(double val, PGMImage img) {
            if (val > img.maxColorVal) return 255;
            else if (val < 0) return 0;
            else return (int)val;
        }
    }
}
