using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.ViewModels
{
    public class PlaylistViewModel : ObservableObject
    {
        private ObservableCollection<VideoViewModel> _videos;
        private readonly SQLite sql = new SQLite();
        private VideoViewModel _selectedVideo;

        public VideoViewModel SelectedVideo {
            get => _selectedVideo;
            set {
                _selectedVideo = value;
                OnPropertyChanged("SelectedVideo");
            }
        }
        public ObservableCollection<VideoViewModel> Videos {
            get => _videos;
            set {
                _videos = value;
                OnPropertyChanged("Videos");
            }
        }

        public RelayCommand DeleteVideoPlaylistCommand { get; }
        public RelayCommand ClearPlaylistCommand { get; }

        private void OpenLink(string link) {
            //open link on default browser win 10 
            var psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = link;
            Process.Start(psi);
        }

        public PlaylistViewModel() {
            _videos = sql.ListAvaliablePlaylistVideos();

            DeleteVideoPlaylistCommand = new RelayCommand(o => {
                sql.AddPlaylistVideo(SelectedVideo.VideoUrl);
            }, o => { return SelectedVideo != null; });
            ClearPlaylistCommand = new RelayCommand(o => {
                sql.ClearPlaylistVideos();
            }, o => { return Videos.Count > 0; });
        }
    }
}
