using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VideoJetApp.ViewModels;

namespace VideoJetApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ConnectionService ConnectionService { get; private set; }

        public App()
        {
            ConnectionService = new ConnectionService();
        }

    }
}
