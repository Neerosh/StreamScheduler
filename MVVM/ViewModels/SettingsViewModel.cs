using StreamScheduler.Core;

namespace StreamScheduler.MVVM.ViewModels
{
    public class SettingsViewModel : ObservableObject {
        private string _googleApiKey;
        private readonly SQLite sql = new SQLite();
        public string GoogleApiKey {
            get => _googleApiKey;
            set {
                _googleApiKey = value;
                OnPropertyChanged("GoogleApiKey");
            }
        }

        public RelayCommand UpdateSettingsCommand { get; }

        public SettingsViewModel() {
            GoogleApiKey = sql.GetGoogleAPIKey();

            UpdateSettingsCommand = new RelayCommand(async o => {
                sql.UpdateSettings("GoogleAPIKey", GoogleApiKey);
            }, o => { return !string.IsNullOrEmpty(GoogleApiKey); });
        }
    }
}
