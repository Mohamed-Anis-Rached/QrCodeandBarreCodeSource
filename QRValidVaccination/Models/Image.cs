using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRValidVaccination
{
    public class Image : INotifyPropertyChanged
    {
        #region Membres de classe
        private string name = string.Empty;
        private string path = string.Empty;
        private Bitmap logo = null;
        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methodes
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Properties

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if (value != this.path)
                {
                    this.path = value;
                    NotifyPropertyChanged("Path");
                }
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }
        public Bitmap Logo
        {
            get
            {
                return this.logo;
            }

            set
            {
                if (value != this.logo)
                {
                    this.logo = value;
                    NotifyPropertyChanged("Logo");
                }
            }
        }
        public object Source { get; internal set; }
        #endregion
    }
}
