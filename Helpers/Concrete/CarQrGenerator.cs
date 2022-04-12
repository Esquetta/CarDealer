using CarDealer.Helpers.Abstract;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealer.Helpers.Concrete
{
    public class CarQrGenerator : IQRCoder
    {
        
        public Byte[] GenerateCode(string url)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qRCodeImage = qrCode.GetGraphic(20);
            return BitmapToBytesCode(qRCodeImage);

        }
        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream= new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
