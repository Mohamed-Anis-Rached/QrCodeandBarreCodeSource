using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QRValidVaccination.Interfaces
{
    public interface IImage
    {
        void OpenImage(System.Windows.Controls.Image QRImage);
        Bitmap ConvertToBitmap(ImageSource source);
        System.Drawing.Image ImageWpfToGDI(System.Windows.Media.ImageSource image);
        void LoadImage(string pathFile, System.Windows.Controls.Image QRImage);
        string Confirm_Click(ImageSource imagesource);
    }
}
