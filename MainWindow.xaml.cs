using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StreamScheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SQLite sql = new SQLite();

        public MainWindow() {
            InitializeComponent();
            sql.AddChannels();
            LoopCheckIsLive();
        }
        private async Task LoopCheckIsLive() {
            bool result = false;
            DateTime systemDateTime, itemDateTime;
            double timeSpan;

            while (result == false) {
                await Task.Delay(60000); // 1 min
                //foreach (Video video in dgPlaylistVideos.Items) { fix later
                //    systemDateTime = DateTime.Now;
                //    itemDateTime = video.StartDateTime;
                //    timeSpan = (itemDateTime - systemDateTime).TotalMinutes;
                //    //MessageBox.Show("timeSpan =" + timeSpan);
                //    if (timeSpan <= 1 && timeSpan >= 0) {
                //        OpenLink(video.VideoUrl);
                //    }
                //}
            }
        }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "https://www.youtube.com/watch?v=" + link;
            Process.Start(psi);
        }

        private void MenuBarMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            
        }
    }
}
