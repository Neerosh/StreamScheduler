using StreamScheduler.Core;
using System.Collections.ObjectModel;

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
                if (value != null) {
                    _videos = value;
                    OnPropertyChanged("Videos");
                }
            }
        }

        public RelayCommand DeleteVideoPlaylistCommand { get; }
        public RelayCommand ClearPlaylistCommand { get; }


        public PlaylistViewModel() {
            _videos = sql.ListAvaliablePlaylistVideos();

            DeleteVideoPlaylistCommand = new RelayCommand(o => {
                sql.DeletePlaylistVideo(SelectedVideo.VideoUrl);
                Videos.Remove(SelectedVideo);
            }, o => { return SelectedVideo != null; });
            ClearPlaylistCommand = new RelayCommand(o => {
                sql.ClearPlaylistVideos();
            }, o => { return Videos != null && Videos.Count > 0; });
        }
    }
}
