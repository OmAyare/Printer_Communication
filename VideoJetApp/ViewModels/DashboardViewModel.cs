using DataManager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VideoJetApp.Commands;

namespace VideoJetApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly FileBrowserService fileBrowserService;
        private readonly ConnectionService _connectionService;
        //private Window _currentWindow;
        private TcpClient _client;

        public ICommand BrowseFileCommand { get; }
       // public ICommand SignOutCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand SendCommand { get; }




        private DataTable fileData;
        public DataTable FileData
        {
            get { return fileData; }
            set { fileData = value; OnPropertyChanged(nameof(FileData)); }
        }

        private string fileName;
        public string FileName
        {
            get => fileName;
            set { fileName = value; OnPropertyChanged(nameof(FileName)); }
        }

        //private string ipAddress;
        //public string IpAddress
        //{
        //    get { return ipAddress; }
        //    set { ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        //}

        //private int port;

        //public int Port
        //{
        //    get { return port; }
        //    set { port = value; OnPropertyChanged(nameof(Port)); }
        //}

        private string connectionstatus;

        public string ConnectionStatus
        {
            get { return connectionstatus; }
            set { connectionstatus = value; OnPropertyChanged(nameof(ConnectionStatus)); }
        }



        public DashboardViewModel()
        {
            _connectionService = new ConnectionService();

            //SignOutCommand = new RelayCommand(SignOut);

            fileBrowserService = new FileBrowserService();
            BrowseFileCommand = new RelayCommand(BrowseFile);

            //ConnectCommand = new RelayCommand(ConnectToServer);
            StartCommand = new RelayCommand(SendStartCommand);
            SendCommand = new RelayCommand(SendRowToServer);

            ConnectionStatus = "Disconnected";

        } 

        private void BrowseFile()
        {

            FileData = fileBrowserService.BrowseAndReadFile();
            FileName = fileBrowserService.SelectedFileName;

            //var dataTable = fileBrowserService.BrowseAndReadFile();
            //if (dataTable != null)
            //{
            //    FileName = fileBrowserService.SelectedFileName;
            //    FileData = dataTable;
            //}
        }

        //private void SignOut()
        //{

        //    var result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

        //    if (result == MessageBoxResult.Yes)
        //    {
        //        var loginWindow = new VideoJetApp.MainWindow();
        //        loginWindow.Show();
        //    }
        //    _currentWindow.Close();
        //}

        //private void ConnectToServer()
        //{
        //    bool isConnected = _connectionService.Connect(IpAddress, Port);
        //    ConnectionStatus = _connectionService.ConnectionStatus;

        //    if (isConnected)
        //    {
        //        // Optionally, you can update the UI to show that the connection was successful
        //        ConnectionStatus = "Connected";
        //    }
        //    else
        //    {
        //        // Handle the failed connection case, e.g., show an error message
        //        ConnectionStatus = "Failed to connect";
        //    }
        //}

        private void SendStartCommand()
        {
            try
            {
                if (_connectionService != null && _connectionService.IsConnected)
                {
                    string command = "SST|1|<CR>";
                    _connectionService.Send(command);
                }
                else
                {
                    MessageBox.Show("Please connect to the server before starting the printer.", "Connection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send command: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int currentRowIndex = 0;
        private async void SendRowToServer()
        {
            if (FileData == null || FileData.Rows.Count == 0)
            {
                MessageBox.Show("No data to send. Please load an Excel file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_connectionService.IsConnected)
            {
                MessageBox.Show("Please connect to the server before sending data.", "Connection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (!FileData.Columns.Contains("Status"))
                {
                    FileData.Columns.Add("Status", typeof(string)); // Add a Status column if not present
                }

                while (currentRowIndex < FileData.Rows.Count)
                {
                    DataRow currentRow = FileData.Rows[currentRowIndex];

                    // Format the row as per the required protocol
                    string message = FormatRowForServer(currentRow);

                    // Send the data to the server
                    _connectionService.Send(message);

                    // Wait for the PRC<CR> acknowledgment
                    string response = await _connectionService.Receive();  // Await the server response
                  //  MessageBox.Show("Row sent successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (response == "PRC")
                    {
                        currentRow["Status"] = "Acknowledged";

                        // Refresh the DataGrid
                        FileData.AcceptChanges();

                        currentRowIndex++;  // Only move to the next row if we received the correct acknowledgment
                    }
                    else
                    {
                        MessageBox.Show("Unexpected response from server: " + response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }

                if (currentRowIndex >= FileData.Rows.Count)
                {
                    MessageBox.Show("All rows have been sent.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show($"Error sending row: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // row format
        private string FormatRowForServer(DataRow row)
        {
            // Format: JDI|VAR1=C1|VAR2=C2|VAR3=C3|<CR>
            StringBuilder formattedRow = new StringBuilder("JDI");
            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                formattedRow.Append($"|VAR{i + 1}={row[i]}");
            }
            formattedRow.Append("|<CR>");
            return formattedRow.ToString();
        }

    }
}

