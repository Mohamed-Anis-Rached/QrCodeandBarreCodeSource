using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRValidVaccination.Interfaces
{
   public interface IQRBARCODE
    {
        void ReadCode(Bitmap image);
        string DecodeText(Bitmap oBitmap);
        string Qrreader(Bitmap x);
    }
}
