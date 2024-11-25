using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoJetApp.Commands;
using VideoJetApp.Models;

namespace VideoJetApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        LoginServices LoginServices;
        public LoginViewModel()
        {
            LoginServices = new LoginServices();
            CurrentEmployee = new LoginModels();
            checkCommand = new RelayCommand(CheckLogin);
        }

        private LoginModels currentEmployee;

        public LoginModels CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private RelayCommand checkCommand;

        public RelayCommand CheckCommand
        {
            get { return checkCommand; }
        }

        public void CheckLogin()
        {
            try
            {
                bool isValid = LoginServices.CheckLogin(currentEmployee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
