using StreamScheduler.MVVM.ViewModels;
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
        public PlaylistView() {
            InitializeComponent();
            txtChannelLinks.Visibility = Visibility.Hidden;
        }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = "https://www.youtube.com/watch?v=" + link;
            Process.Start(psi);
        }

        private VideoViewModel SelectedVideo() {
            VideoViewModel video = (VideoViewModel)dgPlaylistVideos.SelectedItem;
            return video;
        }

        private void dgPlaylistVideos_MouseDoubleClick(object sender, EventArgs e) {
            OpenLink(SelectedVideo().VideoUrl);
        }
        private void dgPlaylistVideos_SelectionChanged(object sender, EventArgs e) {
            if (dgPlaylistVideos.SelectedItem == null) { return; }
            VideoViewModel video = (VideoViewModel)dgPlaylistVideos.SelectedItem;
            imgSelectedThumbnail.Source = new BitmapImage(new Uri(video.ThumbnailUrl));
            txtSelectedTitle.Text = video.Title;
            txtYoutubeLink.Text = "https://www.youtube.com/channel/" + video.ChannelUrl;
            txtChannelLinks.Visibility = Visibility.Visible;
        }
        private void YoutubeLink_Click(object sender, EventArgs e) {
            if (dgPlaylistVideos.SelectedItem == null) { return; }

            Video video = (Video)dgPlaylistVideos.SelectedItem;
            OpenLink(txtYoutubeLink.Text);
        }
    }
}
