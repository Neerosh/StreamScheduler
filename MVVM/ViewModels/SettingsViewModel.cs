using StreamScheduler.Core;

namespace StreamScheduler.MVVM.ViewModels
{
    public class SettingsViewModel : ObservableObject {
        private string _googleApiKey;
        private string _playlistScanInterval;
        private readonly SQLite sql = new SQLite();
        public string GoogleApiKey {
            get => _googleApiKey;
            set {
                _googleApiKey = value;
                OnPropertyChanged("GoogleApiKey");
            }
        }
        public string PlaylistScanInterval {
            get => _playlistScanInterval;
            set {
                _playlistScanInterval = value;
                OnPropertyChanged("PlaylistScanInterval");
            }
        }


        public RelayCommand UpdateSettingsCommand { get; }

        public SettingsViewModel() {
            GoogleApiKey = sql.GetSettingValue("GoogleApiKey");
            PlaylistScanInterval = sql.GetSettingValue("PlaylistScanInterval");

            UpdateSettingsCommand = new RelayCommand( o => {
                sql.UpdateSettings("GoogleApiKey", GoogleApiKey);
                sql.UpdateSettings("PlaylistScanInterval", PlaylistScanInterval);
            });
        }
    }
}
