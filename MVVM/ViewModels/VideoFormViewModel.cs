using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace StreamScheduler.MVVM.ViewModels
{
    public class VideoFormViewModel : ObservableObject {
        private readonly SQLite sql = new SQLite();
        private ChannelViewModel _selectedChannel;
        private ObservableCollection<ChannelViewModel> _channels;
        private VideoViewModel _selectedVideo;
        private ObservableCollection<VideoViewModel> _videos;
        private string _formVideoUrl;
        private string _formVideoThumbnailUrl;
        private string _formVideoTitle;
        private string _formVideoDescription;
        private DateTime _formVideoStartDateTime;

        public string FormVideoUrl {
            get => _formVideoUrl;
            set {
                _formVideoUrl = value;
                OnPropertyChanged("FormVideoUrl");
            }
        }
        public string FormVideoThumbnailUrl {
            get => _formVideoThumbnailUrl;
            set {
                _formVideoThumbnailUrl = value;
                OnPropertyChanged("FormVideoThumbnailUrl");
            }
        }
        public string FormVideoTitle {
            get => _formVideoTitle;
            set {
                _formVideoTitle = value;
                OnPropertyChanged("FormVideoTitle");
            }
        }
        public string FormVideoDescription {
            get => _formVideoDescription;
            set {
                _formVideoDescription = value;
                OnPropertyChanged("FormVideoDescription");
            }
        }
        public DateTime FormVideoStartDate {
            get => _formVideoStartDateTime;
            set{
                _formVideoStartDateTime = value;
                OnPropertyChanged("FormVideoStartDate");
            }
        }

        public ChannelViewModel SelectedChannel {
            get => _selectedChannel;
            set {
                _selectedChannel = value;
                OnPropertyChanged("SelectedChannel");
            }
        }
        public ObservableCollection<ChannelViewModel> Channels {
            get => _channels;
            set {
                _channels = value;
                OnPropertyChanged("Channels");
            }
        }
        public VideoViewModel SelectedVideo {
            get => _selectedVideo;
            set {
                _selectedVideo = value;
                OnPropertyChanged("SelectedVideo");
                if (_selectedVideo != null) {
                    FormVideoTitle = _selectedVideo.Title;
                    FormVideoUrl = _selectedVideo.VideoUrl;
                    FormVideoThumbnailUrl = _selectedVideo.ThumbnailUrl;
                    FormVideoStartDate = _selectedVideo.StartDateTime;
                    foreach (ChannelViewModel channel in _channels) {
                        if (channel.ChannelUrl == _selectedVideo.ChannelUrl) { 
                            SelectedChannel = channel;
                        }
                    }
                }
            }
        }
        public ObservableCollection<VideoViewModel> Videos {
            get => _videos;
            set {
                _videos = value;
                OnPropertyChanged("Videos");
            }
        }

        public RelayCommand GetVideoInformationCommand { get; }
        public RelayCommand InsertVideoCommand { get; }
        public RelayCommand UpdateVideoCommand { get; }
        public RelayCommand DeleteVideoCommand { get; }
        public RelayCommand ClearSelectionCommand { get; }

        public VideoFormViewModel() {
            Channels = sql.GetAllChannelsNames();
            Videos = sql.GetAllVideos();
            FormVideoStartDate = DateTime.Now;

            Video video;
            Channel channel;

            GetVideoInformationCommand = new RelayCommand(async o => {
                await GetVideoInfomationTask(FormVideoUrl);
            }, o => { return !string.IsNullOrEmpty(FormVideoUrl); });

            InsertVideoCommand = new RelayCommand(o => {
                video = new Video(FormVideoTitle, FormVideoThumbnailUrl, FormVideoUrl, SelectedChannel.ChannelUrl);
                channel = new Channel(SelectedChannel.ChannelName, SelectedChannel.ChannelUrl);
                VideoViewModel videoView = new VideoViewModel(video,channel);
                sql.InsertVideo(videoView);
                Videos = sql.GetAllVideos();
                SelectedVideo = videoView;
            }, o => { return !string.IsNullOrEmpty(FormVideoTitle) && !string.IsNullOrEmpty(FormVideoUrl) && SelectedChannel != null; });
            UpdateVideoCommand = new RelayCommand(o => {
                video = new Video(FormVideoTitle, FormVideoThumbnailUrl, FormVideoUrl,FormVideoDescription, SelectedChannel.ChannelUrl);
                video.StartDateTime = FormVideoStartDate;
                channel = new Channel(SelectedChannel.ChannelName, SelectedChannel.ChannelUrl);
                VideoViewModel videoView = new VideoViewModel(video, channel);
                sql.UpdateVideo(videoView);
                Videos = sql.GetAllVideos();
                SelectedVideo = videoView;
            }, o => { return SelectedVideo != null && SelectedChannel != null; ; });
            DeleteVideoCommand = new RelayCommand(o => {
                video = new Video(FormVideoTitle, FormVideoThumbnailUrl, FormVideoUrl, SelectedChannel.ChannelUrl);
                channel = new Channel(SelectedChannel.ChannelName, SelectedChannel.ChannelUrl);
                VideoViewModel videoView = new VideoViewModel(video, channel);
                sql.DeleteVideo(videoView);
                Videos = sql.GetAllVideos();
                SelectedVideo = null;
            }, o => { return SelectedVideo != null; });
            ClearSelectionCommand = new RelayCommand(o => {
                SelectedVideo = null;
            });
        }

        public async Task GetVideoInfomationTask(string videoUrl) {
            List<Video> listVideos;
            Youtube youtube = new Youtube();
            listVideos = await youtube.GetVideoInformation(videoUrl);
            if (listVideos.Count > 0) {
                SelectedVideo = new VideoViewModel(listVideos[0], null);
            } else {
                MessageBox.Show("Video not found.");
            }
        }

    }
}
