using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Drawing;
using System.Threading;
using QRValidVaccination.ViewModels;
using QRCoder;
using ZXing;
using ZXing.QrCode;
using MessagingToolkit.QRCode.Codec.Data;
using System.Runtime.InteropServices;
using IronBarCode;
using Aspose.BarCode.BarCodeRecognition;
using System.Text;
using DataMatrix.net;

namespace QRValidVaccination
{ 
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        public FilterInfoCollection videoCaptureDevices { get; set; }
        public QRBarCodeVM qrbarCodeVM { get; set; }
        public Bitmap snapchot { get; set; }
        public ImageVM imageVM { get; set; }
        public videoVM videoVM { get; set; }
        
        #endregion

        #region Attributs
        VideoCaptureDevice LocalWebCam;
        ImageSource imagesource;
        public FilterInfoCollection LoaclWebCamsCollection;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            imageVM = new ImageVM(new Image(), QRImage);
            videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            this.videoVM = new videoVM();
            LocalWebCam = videoVM.LocalWebCam;
        }
        
        #region Event
       
        /// <summary>
        /// le botton "Exit" pour fermer le programme 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            string message = "Do you want to close this window?";
            string title = "Close Window";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(message, title, buttons,MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (videoVM != null)
                {
                    if (videoVM.CamIsClosed == false)
                    {
                        videoVM.Closewebcam_Click(QRImage);
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else if(result == MessageBoxResult.No)
                    {
                if (videoVM != null)
                {
                    if (videoVM.CamIsClosed == true)
                    {
                        videoVM.Closewebcam_Click(QRImage);
                    }
                }
            }
        }
        FilterInfoCollection filterinfocollection;
        VideoCaptureDevice videoCaptureDevice;
        /// <summary>
        /// lorsque en click sur le botton radio "open image" l'action 
        /// le botton open web cam devient invisible et open image est visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn2_Checked(object sender, RoutedEventArgs e) 
        {
            if (Openimage != null && Openwebcam != null)
            {
                if (btn2.IsChecked == true)
                {
                    Openimage.Visibility = Visibility.Visible; 
                    Openwebcam.Visibility = Visibility.Collapsed;
                }
            }
        }
        /// <summary>
        /// lorsque en click sur le botton radio "open web cam" l'action
        /// le botton open image devient invisible et open web cam est visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn1_Checked(object sender, RoutedEventArgs e) 
        {
            if (Openimage != null && Openwebcam != null)
            {
                if (btn1.IsChecked == true)
                {
                    Openimage.Visibility = Visibility.Collapsed;
                    Openwebcam.Visibility = Visibility.Visible;
                }
            }
        }
        /// <summary>
        /// initialisé le frame de camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    QRImage.Source = bi;
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }
        /// <summary>
        /// Ajout d'un événement de click au bouton Démarrer qui vous permet d'afficher l'image de la caméra dans l' espace QRImage
        /// lorsque il y a un image précédent lors de click sur le botton l'espace de image QRImage devient vide est le message invisible pour ajouter nouvelle image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Openwebcam_Click(object sender, RoutedEventArgs e)
        {
            videoVM.Openwebcam_Click(Cam_NewFrame);
            if(affiche.Text != null || message.Visibility == Visibility.Visible)
            {
                affiche.Clear();
                message.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// pour ouvrir les fichiers et choisir un image et affiche cette image dans dans l'espace QRImage
        /// lorsque il y a un image précédent lors de click sur le botton l'espace de image QRImage devient vide est le message invisible pour ajoute nouvelle image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Openimage_Click(object sender, RoutedEventArgs e)
        {
            imageVM.OpenImage(QRImage);
            if (affiche.Text != null || message.Visibility == Visibility.Visible)
            {
                affiche.Clear();
                message.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        ///  botton "close" pour fermer le web cam
        ///  et supprimer le resultat précédent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Closewebcam_Click(object sender, RoutedEventArgs e) 
        {
            videoVM.Closewebcam_Click(Openwebcam, QRImage);
            if (affiche.Text != null || message.Visibility == Visibility.Visible)
            {
                affiche.Clear();
                message.Visibility = Visibility.Collapsed;
                message1.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// botton confirm pour lire l'image de fichier et le web cam et affiche la source de cette image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (QRImage.Source == null )
            {
                MessageBox.Show("Choose open image or open web cam ","Open Media",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            { 
            string result = "";
            if (btn1.IsChecked == true)
            {
                message.Visibility = Visibility.Visible;
                result = videoVM.Confirm_Click(QRImage.Source, LocalWebCam);
                affiche.Text = result;
                if (result == "")
                {
                    message.Visibility = Visibility.Collapsed;
                    message1.Visibility = Visibility.Visible;
                }
                else
                {
                    message.Visibility = Visibility.Visible;
                    message1.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                message.Visibility = Visibility.Visible;
                result = imageVM.Confirm_Click(QRImage.Source);
                affiche.Text = result;
                if (result == "")
                {
                    message.Visibility = Visibility.Collapsed;
                    message1.Visibility = Visibility.Visible;
                }
                else
                {
                    message.Visibility = Visibility.Visible;
                    message1.Visibility = Visibility.Collapsed;
                }
                }
            }
        }
        /// <summary>
        /// pour controler main window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridOfWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }
        /// <summary>
        /// pour effacer le contenu de text box 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            affiche.Clear();
            message.Visibility = Visibility.Collapsed;
            message1.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}