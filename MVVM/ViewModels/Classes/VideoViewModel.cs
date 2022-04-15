using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.ViewModels
{
    public class VideoViewModel : ObservableObject
    {
        private readonly Video _video;
        private readonly Channel _channel;

        public string Title => _video.Title;
        public string VideoUrl => _video.VideoUrl;
        public string ThumbnailUrl => _video.ThumbnailUrl;
        public DateTime StartDateTime => _video.StartDateTime;
        public string ChannelUrl => _video.ChannelUrl;
        public string VideoDescription => _video.Description;
        public string ChannelName => _channel.Name;

        public VideoViewModel(Video video,Channel channel) {
            _video = video;
            _channel = channel;
        }
    }
}
