using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StreamScheduler.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand SearchViewCommand { get; }
        public RelayCommand PlaylistViewCommand { get; }
        public RelayCommand SettingsViewCommand { get; }
        public RelayCommand ChannelFormViewCommand { get; }
        public RelayCommand VideoFormViewCommand { get; }

        private SearchViewModel SearchVM { get; set; }
        private PlaylistViewModel PlaylistVM { get; set; }
        private SettingsViewModel SettingsVM { get; set; }
        private ChannelFormViewModel ChannelFormVM { get; set; }
        private VideoFormViewModel VideoFormVM { get; set; }

        private object _currentView;

        public object CurrentView { 
            get { 
                return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() {
            //Task taskCheckPlaylist = LoopCheckPlaylist();
            SearchViewCommand = new RelayCommand(o => {
                SearchVM = new SearchViewModel();
                CurrentView = SearchVM;
            });
            PlaylistViewCommand = new RelayCommand(o => {
                PlaylistVM = new PlaylistViewModel();
                CurrentView = PlaylistVM;
            });
            SettingsViewCommand = new RelayCommand(o => {
                SettingsVM = new SettingsViewModel();
                CurrentView = SettingsVM;
            });
            ChannelFormViewCommand = new RelayCommand(o => {
                ChannelFormVM = new ChannelFormViewModel();
                CurrentView = ChannelFormVM;
            });
            VideoFormViewCommand = new RelayCommand(o => {
                VideoFormVM = new VideoFormViewModel();
                CurrentView = VideoFormVM;
            });
        }

        private async Task LoopCheckPlaylist() {
            bool result = false;
            double timeSpan;
            DateTime systemDateTime, itemDateTime;
            ObservableCollection<VideoViewModel> playlistVideos;

            while (result == false) {
                SQLite sql = new SQLite();
                string playlistInterval = sql.GetSettingValue("PlaylistCheckInterval");
                int interval = 0;
                if (playlistInterval == null || playlistInterval.Equals(String.Empty)) {
                    interval = 60000; // 1 min
                } else {
                    interval = Convert.ToInt32(playlistInterval)*10000;
                }
                await Task.Delay(interval);
                playlistVideos = sql.ListAvaliablePlaylistVideos();
                foreach (VideoViewModel video in playlistVideos) {
                    systemDateTime = DateTime.Now;
                    itemDateTime = video.StartDateTime;
                    timeSpan = (itemDateTime - systemDateTime).TotalMinutes;
                    //MessageBox.Show("timeSpan =" + timeSpan);
                    if (timeSpan <= interval+1 && timeSpan >= 0) {
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
    }
}
