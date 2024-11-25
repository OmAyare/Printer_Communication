using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoJetApp.Models
{
    public class LoginModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int ID;

        public int id
        {
            get { return ID; }
            set { ID = value; OnPropertyChanged("id"); }
        }

        private string username;

        public string UserName
        {
            get { return username; }
            set { username = value; OnPropertyChanged("UserName"); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; OnPropertyChanged("Role"); }
        }



    }
}
