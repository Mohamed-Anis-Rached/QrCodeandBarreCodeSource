using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QRValidVaccination.Interfaces
{
    public interface IVideo
    {
        string Confirm_Click(ImageSource imagesource, VideoCaptureDevice LocalWebCam);
        Bitmap ConvertToBitmap(ImageSource source);
        System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource image);
        string DecodeText(Bitmap oBitmap);
        void Openwebcam_Click(NewFrameEventHandler Cam_NewFrame);
        void Closewebcam_Click(System.Windows.Controls.Image QRImage);
        void Closewebcam_Click(System.Windows.Controls.Button openwebcam, System.Windows.Controls.Image QRImage);
    }
}
