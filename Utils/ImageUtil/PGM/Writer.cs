using System.Linq;
using System.IO;

namespace ImageUtil.PGM
{
    public static class Writer
    {
        public static void WriteImage(PGMImage img, string filePath)
        {
            // write the header
            using (StreamWriter writer = new StreamWriter(filePath))
            {
				writer.Write(img.Magic + ' ');
				writer.Write(img.numCols.ToString() + ' ');
                writer.Write(img.numRows.ToString() + ' ');
                writer.Write(img.maxColorVal.ToString());
                writer.WriteLine();
                writer.Flush();
            }

            //write pixel data
            if (PGMImage.plainPGMFormats.Contains(img.Magic))
            {
                using (StreamWriter pixelWriter = new StreamWriter(filePath, true))
                {
                    foreach (int pixel in img.PixelValues)
                        pixelWriter.Write(pixel.ToString() + '\n');
                }
            }
            else
            {
                using(BinaryWriter pixelWriter = new BinaryWriter(File.Open(filePath, FileMode.Append)))
                {
                    foreach (int pixel in img.PixelValues)
                        pixelWriter.Write((byte)pixel);
                }
                
            }
        }
    }
}
