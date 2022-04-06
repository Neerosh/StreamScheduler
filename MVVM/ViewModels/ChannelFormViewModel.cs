using StreamScheduler.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

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
                if (_selectedChannel != null) {
                    FormChannelUrl = _selectedChannel.ChannelUrl;
                    FormChannelName = _selectedChannel.ChannelName;
                    FormChannelDescription = _selectedChannel.ChannelDescription;
                }
            }
        }
        public ObservableCollection<ChannelViewModel> Channels {
            get => _channels;
            set {
                _channels = value;
                OnPropertyChanged("Channels");
            }
        }

        public RelayCommand GetChannelInformationCommand { get; }
        public RelayCommand InsertChannelCommand { get; }
        public RelayCommand UpdateChannelCommand { get; }
        public RelayCommand DeleteChannelCommand { get; }
        public RelayCommand ClearSelectionCommand { get; }

        public ChannelFormViewModel() {
            Channels = sql.GetAllChannelsNames();

            GetChannelInformationCommand = new RelayCommand(async o => {
                await GetChannelInfomationTask(FormChannelUrl);
            }, o => { return !string.IsNullOrEmpty(FormChannelUrl); });

            InsertChannelCommand = new RelayCommand(o => {
                ChannelViewModel channelView = new ChannelViewModel(new Channel(FormChannelName, FormChannelUrl, FormChannelDescription));
                sql.InsertChannel(channelView);
                Channels = sql.GetAllChannelsNames();
                SelectedChannel = channelView;
            }, o => { return !string.IsNullOrEmpty(FormChannelName) && !string.IsNullOrEmpty(FormChannelUrl) && !string.IsNullOrEmpty(FormChannelDescription); });
            UpdateChannelCommand = new RelayCommand(o => {
                ChannelViewModel channelView = new ChannelViewModel(new Channel(FormChannelName, FormChannelUrl, FormChannelDescription));
                sql.UpdateChannel(channelView);
                Channels = sql.GetAllChannelsNames();
                SelectedChannel = channelView;
            }, o => { return SelectedChannel != null; });
            DeleteChannelCommand = new RelayCommand(o => {
                ChannelViewModel channelView = new ChannelViewModel(new Channel(FormChannelName, FormChannelUrl, FormChannelDescription));
                sql.DeleteChannel(channelView);
                Channels = sql.GetAllChannelsNames();
            }, o => { return SelectedChannel != null; });
            ClearSelectionCommand = new RelayCommand(o => {
                SelectedChannel = new ChannelViewModel(new Channel("", "", ""));
            });
        }

        public async Task GetChannelInfomationTask(string channelUrl) {
            List<Channel> listChannels;
            Youtube youtube = new Youtube();
            SQLite sql = new SQLite();
            listChannels = await youtube.GetChannelInformation(channelUrl);
            if (listChannels.Count > 0) {
                SelectedChannel = new ChannelViewModel(listChannels[0]);
            } else {
                MessageBox.Show("Channel not found.");
            }
        }

    }
}
