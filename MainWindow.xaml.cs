using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace StreamScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow() {
            InitializeComponent();
        }

        private void MenuBarMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }


    }
}
