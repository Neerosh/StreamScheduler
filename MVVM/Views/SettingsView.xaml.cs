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

namespace StreamScheduler.MVVM.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private SQLite sql = new SQLite();
        public SettingsView() {
            InitializeComponent();
            txtGoogleAPIKey.Text =  sql.GetGoogleAPIKey();
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e) {
            sql.UpdateSettings("GoogleAPIKey", txtGoogleAPIKey.Text);

        }
    }
}
