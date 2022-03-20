using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.ViewModels
{
    public class ChannelFormViewModel : ObservableObject {
        private readonly SQLite sql = new SQLite();
        private ChannelViewModel _selectedChannel;
        private ObservableCollection<ChannelViewModel> _channels;
        private string _formChannelUrl;
        private string _formChannelName;
        private string _formChannelDescription;

        public string FormChannelUrl {
            get => _formChannelUrl;
            set {
                _formChannelUrl = value;
                OnPropertyChanged("FormChannelUrl");
            }
        }
        public string FormChannelName {
            get => _formChannelName;
            set {
                _formChannelName = value;
                OnPropertyChanged("FormChannelName");
            }
        }
        public string FormChannelDescription {
            get => _formChannelDescription;
            set {
                _formChannelDescription = value;
                OnPropertyChanged("FormChannelDescription");
            }
        }

        public ChannelViewModel SelectedChannel {
            get => _selectedChannel;
            set {
                _selectedChannel = value;
                OnPropertyChanged("SelectedChannel");
                FormChannelUrl = _selectedChannel.ChannelUrl;
                FormChannelName = _selectedChannel.ChannelName;
                FormChannelDescription = _selectedChannel.ChannelDescription;
            }
        }
        public ObservableCollection<ChannelViewModel> Channels {
            get => _channels;
            set {
                _channels = value;
                OnPropertyChanged("Channels");
            }
        }

        public RelayCommand GetChannelInformation { get; }
        public RelayCommand InsertChannel { get; }
        public RelayCommand UpdateChannel { get; }
        public RelayCommand DeleteChannel { get; }

        public ChannelFormViewModel() {
            _channels = sql.GetAllChannelsNames();
            InsertChannel = new RelayCommand(o => {
                sql.AddChannel(new ChannelViewModel(new Channel(FormChannelName,FormChannelUrl,FormChannelDescription)));
            });
        }

    }
}
