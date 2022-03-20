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
        private SQLite sql = new SQLite();
        private Task task;
        public SearchView() {
            InitializeComponent();
            //cboChannel.ItemsSource = sql.GetAllChannelsNames();
            txtChannelLinks.Visibility = Visibility.Hidden;
        }

        public async Task RunSearch(string channelUrl) {
            List<Video> listVideos;
            Youtube youtube = new Youtube();
            SQLite sql = new SQLite();

            listVideos = await youtube.GetUpcomingVideos(channelUrl);
            sql.UpdateVideos(listVideos);
        }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = link;
            Process.Start(psi);
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e) {
            if (dgSearchVideos.SelectedItem == null) { return; }

            Video video = (Video)dgSearchVideos.SelectedItem;
            sql.AddPlaylistVideo(video.VideoUrl);
        }
        private async void BtnSearchVideos(object sender, RoutedEventArgs e) {
            if (cboChannel.SelectedIndex < 0 || cboChannel.SelectedValue == null) { return; }
            if (task == null || task.IsCompleted) {
                string channelUrl = cboChannel.SelectedValue.ToString();
                task = RunSearch(channelUrl);
                await task;
                return;
            }
        }

        private void dgSearchVideos_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dgSearchVideos.SelectedItem == null) { return; }
            VideoViewModel video = (VideoViewModel)dgSearchVideos.SelectedItem;
            imgSelectedThumbnail.Source = new BitmapImage(new Uri(video.ThumbnailUrl));
            txtSelectedTitle.Text = video.Title;
            txtYoutubeLink.Text = "https://www.youtube.com/channel/" + video.ChannelUrl;
            txtChannelLinks.Visibility = Visibility.Visible;
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
