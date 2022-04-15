using StreamScheduler.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace StreamScheduler.MVVM.ViewModels
{
    public class SearchViewModel : ObservableObject {
        private ObservableCollection<VideoViewModel> _videos;
        private readonly ObservableCollection<ChannelViewModel> _channels;
        private readonly SQLite sql = new SQLite();
        private VideoViewModel _selectedVideo;
        private ChannelViewModel _selectedChannel;

        public VideoViewModel SelectedVideo { 
            get => _selectedVideo; 
            set {
                _selectedVideo = value;
                OnPropertyChanged("SelectedVideo");
            }
        }

        public ChannelViewModel SelectedChannel {
            get => _selectedChannel; 
            set {
                _selectedChannel = value;
                OnPropertyChanged("SelectedChannel");
            }
        }
        public ObservableCollection<VideoViewModel> Videos { 
            get => _videos;
            set {
                _videos = value;
                OnPropertyChanged("Videos");
            } 
        }
        public ObservableCollection<ChannelViewModel> Channels => _channels;
        public RelayCommand AddToPlaylistCommand { get; }
        public RelayCommand SearchUpcomingVideosCommand { get; }
        //public AddToPlaylistCommand AddToPlaylistCommand { get; }


        public SearchViewModel() {
            _videos = sql.ListAvailableVideos();
            _channels = sql.GetAllChannelsNames();

            AddToPlaylistCommand = new RelayCommand(o => {
                sql.AddPlaylistVideo(SelectedVideo.VideoUrl);
            }, o => { return SelectedVideo != null;  });
            SearchUpcomingVideosCommand = new RelayCommand(async o => {
                await SearchUpcomingVideos(SelectedChannel.ChannelUrl);
                Videos.Clear();
                Videos = sql.ListAvailableVideos();
            }, o => { return SelectedChannel != null; });
        }

        public async Task SearchUpcomingVideos(string channelUrl) {
            List<Video> listVideos;
            Youtube youtube = new Youtube();
            SQLite sql = new SQLite();

            listVideos = await youtube.GetUpcomingVideos(channelUrl);
            if (listVideos.Count > 0) {
                sql.UpdateVideos(listVideos);
            } else {
                MessageBox.Show("No upcoming livestreams found.");
            }
        }

    }
}
