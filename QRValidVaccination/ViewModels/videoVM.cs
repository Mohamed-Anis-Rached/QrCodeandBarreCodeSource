using AForge.Video;
using AForge.Video.DirectShow;
using DataMatrix.net;
using IronBarCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ZXing.QrCode;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge;
using System.Drawing.Imaging;
using System.Threading;
using QRValidVaccination.ViewModels;
using QRCoder;
using ZXing;
using MessagingToolkit.QRCode.Codec.Data;
using System.Runtime.InteropServices;
using Aspose.BarCode.BarCodeRecognition;
using System.Web.UI.WebControls;
using QRValidVaccination.Interfaces;

namespace QRValidVaccination.ViewModels
{
    public class videoVM : IVideo
    {
        #region Attributs
        public FilterInfoCollection LoaclWebCamsCollection { get;  set; }
        public VideoCaptureDevice LocalWebCam { get;  set; }
        public NewFrameEventHandler NewFrame { get; set; }
        public Bitmap snapchot { get; set; }
        public bool CamIsClosed { get; set; }
        #endregion

        #region Constructeur
        public videoVM()
        {
            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);// En ajoutant un gestionnaire d'événements Openwebcam_Click qui vous permet de détecter la caméra à partir de votre ordinateur, vous pouvez alors sélectionner la caméra que vous souhaitez utiliser comme code c# suivant
            LocalWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            CamIsClosed = false;
        }
        #endregion

        #region Methods



        /// <summary>
        ///  botton confirm pour lire le code a bar ou le QRcode et retourner le resultat
        /// </summary>
        /// <param name="imagesource"></param>
        /// <param name="LocalWebCam"></param>
        /// <returns></returns>
        public string Confirm_Click(ImageSource imagesource,VideoCaptureDevice LocalWebCam)
        {
            string p = "";
            try
            {
                if (imagesource == null)
                    p = String.Empty;
                var bitm = ConvertToBitmap(imagesource);
                LocalWebCam.SnapshotFrame += LocalWebCam_SnapshotFrame;
                BarcodeResult Result = IronBarCode.BarcodeReader.QuicklyReadOneBarcode(bitm, BarcodeEncoding.AllOneDimensional | BarcodeEncoding.AllTwoDimensional, true);
                if (Result != null)
                    p = Result.Value;
                ZXing.QrCode.QRCodeReader qr = new QRCodeReader();
            }
            catch(Exception es)
            {
                Console.WriteLine("Error : " + es.Message);
            }
            return p;
        }
        /// <summary>
        /// to convert image into bitmap
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
                if (image != null)
                {
                    encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image as System.Windows.Media.Imaging.BitmapSource));
                    encoder.Save(ms);
                    ms.Flush();
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
            return System.Drawing.Image.FromStream(ms);
        }
        public void LocalWebCam_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                snapchot = (Bitmap)eventArgs.Frame;
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }

        }
        /// <summary>
        /// Decoder Bitmap dans "string" resultat
        /// </summary>
        /// <param name="oBitmap"></param>
        /// <returns></returns>
        public string DecodeText(Bitmap oBitmap)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DmtxImageDecoder decoder = new DmtxImageDecoder();
                List<string> oList = decoder.DecodeImage(oBitmap);

                sb.Length = 0;
                foreach (string s in oList)
                {
                    sb.Append(s);
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Ajout d'un événement de click au bouton Démarrer qui vous permet d'afficher l'image de la caméra dans l' espace QRImage
        ///  ouvrir le web cam
        /// </summary>
        /// <param name="Cam_NewFrame"></param>
        public void Openwebcam_Click(NewFrameEventHandler Cam_NewFrame)
        {
            try
            {
                if (LocalWebCam != null)
                {
                    if (!LocalWebCam.IsRunning)
                    {
                        LocalWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);
                        LocalWebCam.Start();
                    }
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
        }
        /// <summary>
        /// botton "close" pour fermer le web cam 
        ///  arreter le signal de web cam
        /// arreter le web cam
        ///  le espace "Qrimage" de web cam est vide 
        /// </summary>
        /// <param name="Openwebcam"></param>
        /// <param name="QRImage"></param>
        public void Closewebcam_Click(System.Windows.Controls.Button Openwebcam, System.Windows.Controls.Image QRImage)
        {
            try
            {
                if (Openwebcam != null)
                {
                    if (LocalWebCam != null)
                    {
                        LocalWebCam.SignalToStop();
                        LocalWebCam.WaitForStop();
                        LocalWebCam.Stop();
                        QRImage.Source = null;
                    }
                }
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
        }
        /// <summary>
        /// arreter le signal de web cam
        /// arreter le web cam
        /// le espace "Qrimage" de web cam est vide
        /// </summary>
        /// <param name="QRImage"></param>
        public void Closewebcam_Click( System.Windows.Controls.Image QRImage)
        {
            try
            {
                LocalWebCam.SignalToStop();
                LocalWebCam.WaitForStop();
                LocalWebCam.Stop();
                QRImage.Source = null;
            }
            catch (Exception es)
            {
                Console.WriteLine("Error: " + es.Message);
            }
        }
        #endregion
    }
}
