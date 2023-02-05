using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRValidVaccination
{
    public class Video : INotifyPropertyChanged
    {
        #region Constructeur

        private string name = string.Empty;
        private string path = string.Empty;
        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion 

        #region Attributs
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
        #endregion
    }
}
