using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.ViewModels
{
    public class ChannelViewModel : ObservableObject
    {
        private readonly Channel _channel;

        public string ChannelName => _channel.Name;
        public string ChannelUrl => _channel.Url;
        public string ChannelDescription => _channel.Description;

        public ChannelViewModel(Channel channel) {
            _channel = channel;
        }
    }
}
