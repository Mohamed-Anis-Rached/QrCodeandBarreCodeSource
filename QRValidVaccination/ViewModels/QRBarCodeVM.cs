using Aspose.BarCode.BarCodeRecognition;
using DataMatrix.net;
using QRValidVaccination.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ZXing;

namespace QRValidVaccination
{
    public class QRBarCodeVM : IQRBARCODE
    {
        #region Methods

        public void ReadCode(Bitmap image)
        {
            try
            {
                // Create instance of BarCodeReader class 
                using (BarCodeReader reader = new BarCodeReader(image, DecodeType.AllSupportedTypes))
                {
                    var r = reader.FoundBarCodes;
                    Console.WriteLine(r[0].CodeText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Decode Bitmap into string result
        /// </summary>
        /// <param name="oBitmap"></param>
        /// <returns></returns>
        public string DecodeText(Bitmap oBitmap)
        {
            DmtxImageDecoder decoder = new DmtxImageDecoder();
            List<string> oList = decoder.DecodeImage(oBitmap);

            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            foreach (string s in oList)
            {
                sb.Append(s);
            }
            return sb.ToString();
        }
        /// <summary>
        /// appelle un bibliothèque pour lire le code QR
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public string Qrreader(Bitmap x)
        {
            ZXing.BarcodeReader reader = new ZXing.BarcodeReader { AutoRotate = true, TryInverted = true };
            Result result = reader.Decode(x);
            string decoded = result.ToString().Trim();
            return decoded;
        }
        #endregion
    }
}