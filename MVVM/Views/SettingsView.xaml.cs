using System.Windows.Controls;

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
        }
    }
}
