using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipingInfoSystem.model
{
    public class User : INotifyPropertyChanged
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                if (userName == value)
                    return;
                userName = value;
                Notify("UserName");
            }
        }

        private int userType;
        public int UserType
        {
            get { return userType; }
            set
            {
                if (userType == value)
                    return;
                userType = value;
                Notify("UserType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
