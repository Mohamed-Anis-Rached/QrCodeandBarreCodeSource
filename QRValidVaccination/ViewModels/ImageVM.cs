using AForge.Video.DirectShow;
using IronBarCode;
using Microsoft.Win32;
using QRValidVaccination.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing.QrCode;

namespace QRValidVaccination.ViewModels
{
    public class ImageVM : INotifyPropertyChanged, IImage
    {
        #region Attributs
        private Image image { get; set; }

        public System.Windows.Controls.Image QRImage { get; set; }
        #endregion

        #region Constructeur
        public ImageVM(Image image, System.Windows.Controls.Image QRImage)
        {
            this.image = image;
            this.QRImage = QRImage;
        }
        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        /// <summary>
        /// pour choisir les caractéristique de l'image
        /// </summary>
        /// <param name="QRImage"></param>
        public void OpenImage(System.Windows.Controls.Image QRImage)
        {
            try
            {
                OpenFileDialog QR = new OpenFileDialog();
                if (QR.ShowDialog() == true)
                {
                    QR.Filter = "image jpg(*.jpg)|*.jpg|image png(*.png)|*.png";
                    QR.Multiselect = false;
                    image.Name = QR.FileName;
                    LoadImage(image.Name, QRImage);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
        }
        /// <summary>
        /// pour afficher l'image de fichier dans une espace 
        /// </summary>
        /// <param name="pathFile"></param>
        /// <param name="QRImage"></param>
        public void LoadImage(string pathFile, System.Windows.Controls.Image QRImage)
        {
            try
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(pathFile);
                logo.EndInit();
                QRImage.Source = logo;
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
        }
        /// <summary>
        /// convertir image en bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Bitmap ConvertToBitmap(ImageSource source)
        {
            Bitmap bitmap = null;
            try
            {
                if (source != null)
                {
                    System.Drawing.Image image = ImageWpfToGDI(source);
                    bitmap = new Bitmap(image);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
            return bitmap;
        }
        /// <summary>
        /// convertir image source en image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource image)
        {
            MemoryStream ms = new MemoryStream();

            try
            {

                var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image as System.Windows.Media.Imaging.BitmapSource));
                encoder.Save(ms);
                ms.Flush();
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
            return System.Drawing.Image.FromStream(ms);
        }
        /// <summary>
        /// botton confirm pour lire le code a bar ou le QRcode et retourner le resultat
        /// </summary>
        /// <param name="imagesource"></param>
        /// <returns></returns>
        public string Confirm_Click(ImageSource imagesource)
        {
            string result = "";

            try
            {
                var bitm = ConvertToBitmap(imagesource);
                BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitm, BarcodeEncoding.AllOneDimensional | BarcodeEncoding.AllTwoDimensional, true);
                Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitm, BarcodeEncoding.AllOneDimensional | BarcodeEncoding.AllTwoDimensional, true);
                if (Result != null)
                {
                    result = Result.Value;
                }
                ZXing.QrCode.QRCodeReader qr = new QRCodeReader();
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
            return result;
        }
        #endregion
    }

}