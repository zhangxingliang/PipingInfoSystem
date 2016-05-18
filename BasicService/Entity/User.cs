using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicService
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
        private string userToken;
        public string UserToken
        {
            get { return userToken; }
            set
            {
                if (UserToken == value)
                    return;
                userToken = value;
                Notify("UserToken");
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
