using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl
    {
        private SQLite sql = new SQLite();
        public PlaylistView() {
            InitializeComponent();
            RefreshDataGrids();
        }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "https://www.youtube.com/watch?v=" + link;
            Process.Start(psi);
        }

        private void RefreshDataGrids() {
            dgPlaylistVideos.DataContext = sql.ListPlaylistVideos();
        }

        private Video SelectedVideo() {
            //if (dgPlaylistVideos.SelectedItem == null) { return null; }

            Video video = (Video)dgPlaylistVideos.SelectedItem;
            return video;
        }

        private void dgPlaylistVideos_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            Video video = SelectedVideo();
            OpenLink(video.VideoUrl);
        }

        private void BtnDeleteVideo_Click(object sender, RoutedEventArgs e) {
            Video video = SelectedVideo();
            sql.DeletePlaylistVideo(video.VideoUrl);
            RefreshDataGrids();
        }

        private void BtnClearPlaylist_Click(object sender, RoutedEventArgs e) {
            sql.DeleteAllPlaylistVideos();
            RefreshDataGrids();
        }
    }
}
