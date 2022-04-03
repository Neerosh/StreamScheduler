using StreamScheduler.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LoopCheckIsLive();
        }
        private async Task LoopCheckIsLive() {
            bool result = false;
            double timeSpan;
            DateTime systemDateTime, itemDateTime;
            SQLite sql = new SQLite();
            ObservableCollection<VideoViewModel> playlistVideos;

            while (result == false) {
                await Task.Delay(120000); // 1 min
                sql = new SQLite();
                playlistVideos = sql.ListAvaliablePlaylistVideos();
                foreach (VideoViewModel video in playlistVideos) {
                    systemDateTime = DateTime.Now;
                    itemDateTime = video.StartDateTime;
                    timeSpan = (itemDateTime - systemDateTime).TotalMinutes;
                    MessageBox.Show("timeSpan =" + timeSpan);
                    if (timeSpan <= 3 && timeSpan >= 0) {
                        OpenLink(video.VideoUrl);
                        sql.UpdatePlaylistVideoUserNotified(video.VideoUrl);
                    }
                }
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
