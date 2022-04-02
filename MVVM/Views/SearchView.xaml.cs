using StreamScheduler.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace StreamScheduler.MVVM.Views
{
    /// <summary>
    /// Interaction logic for SeachView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView() {
            InitializeComponent();
            txtChannelLinks.Visibility = Visibility.Hidden;
        }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = link;
            Process.Start(psi);
        }

        private void dgSearchVideos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgSearchVideos.SelectedItem == null) { return; }
            try {
                VideoViewModel video = (VideoViewModel)dgSearchVideos.SelectedItem;
                imgSelectedThumbnail.Source = new BitmapImage(new Uri(video.ThumbnailUrl));
                txtSelectedTitle.Text = video.Title;
                txtYoutubeLink.Text = "https://www.youtube.com/channel/" + video.ChannelUrl;
                txtChannelLinks.Visibility = Visibility.Visible;
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex);
            }
        }
        private void dgSearchVideos_DoubleClick(object sender, EventArgs e) {
            if (dgSearchVideos.SelectedItem == null) { return; }

            VideoViewModel video = (VideoViewModel)dgSearchVideos.SelectedItem;
            OpenLink("https://www.youtube.com/watch?v=" + video.VideoUrl);
        }

        private void YoutubeLink_Click(object sender, EventArgs e) {
            if (dgSearchVideos.SelectedItem == null) { return; }

            Video video = (Video)dgSearchVideos.SelectedItem;
            OpenLink(txtYoutubeLink.Text);
        }
    }
}
