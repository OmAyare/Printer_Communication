using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoJetApp.ViewModels;

namespace VideoJetApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginViewModel loginViewModel;
            public MainWindow()
            {
                InitializeComponent();
                loginViewModel = new LoginViewModel();
                this.DataContext = loginViewModel;
            }

            private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
            {
                if (DataContext is LoginViewModel viewModel)
                {
                    viewModel.CurrentEmployee.Password = ((PasswordBox)sender).Password;
                }
            }

            private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
            {
                if (PasswordBox.Visibility == Visibility.Visible)
                {
                    // Show the password as plain text
                    VisiblePasswordBox.Text = PasswordBox.Password;
                    PasswordBox.Visibility = Visibility.Collapsed;
                    VisiblePasswordBox.Visibility = Visibility.Visible;
                }
                else
                {
                    // Hide the password
                    PasswordBox.Password = VisiblePasswordBox.Text;
                    PasswordBox.Visibility = Visibility.Visible;
                    VisiblePasswordBox.Visibility = Visibility.Collapsed;
                }
            }
        }
    }

