using System.IO;
using SkiaSharp;
using BarcodeStandard;

namespace Matsen
{
    public class BarcodeGenerator
    {
        public void GenerateBarcode(string barcodeText, string filePath)
        {
            Barcode barcode = new Barcode
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.Center,
                Width = 300,
                Height = 100
            };

            // Generowanie kodu kreskowego jako SKImage
            using (SKImage img = barcode.Encode(BarcodeStandard.Type.Code128, barcodeText, 300, 100))
            {
                using (SKData data = img.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (Stream stream = File.OpenWrite(filePath))
                    {
                        data.SaveTo(stream);
                    }
                }
            }
        }
    }
}
