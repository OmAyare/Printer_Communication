using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoJetApp.Commands;

namespace VideoJetApp.ViewModels
{
    public class BtnViewModel: INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly ConnectionService _connectionService;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand SignOutCommand { get; }

        public BtnViewModel()
        {
            _connectionService = new ConnectionService();

            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            SignOutCommand = new RelayCommand(SignOut);

            // Default to Dashboard view
            //CurrentView = new View.Dashboard1();
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            CurrentView = new Views.DashboardView();
        }

        private void ShowSettings()
        {
            CurrentView = new Views.Settings();
        }

        private void SignOut()
        {

            var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var loginWindow = new VideoJetApp.MainWindow();
                loginWindow.Show();
            }
            Application.Current.Shutdown();
        }
        //private void SignOut()
        //{
        //    var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        Application.Current.Shutdown();
        //    }
        //}
    }

}

