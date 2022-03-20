using StreamScheduler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        public RelayCommand SearchViewCommand { get; }
        public RelayCommand PlaylistViewCommand { get; }
        public RelayCommand SettingsViewCommand { get; }
        public RelayCommand ChannelFormViewCommand { get; }

        private SearchViewModel SearchVM { get; set; }
        private PlaylistViewModel PlaylistVM { get; set; }
        private SettingsViewModel SettingsVM { get; set; }
        private ChannelFormViewModel ChannelFormVM { get; set; }

        private object _currentView;

        public object CurrentView { 
            get { 
                return _currentView; }
            set { 
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() {
            SearchViewCommand = new RelayCommand(o => {
                SearchVM = new SearchViewModel();
                CurrentView = SearchVM;
            });
            PlaylistViewCommand = new RelayCommand(o => {
                PlaylistVM = new PlaylistViewModel();
                CurrentView = PlaylistVM;
            });
            SettingsViewCommand = new RelayCommand(o => {
                SettingsVM = new SettingsViewModel();
                CurrentView = SettingsVM;
            });
            ChannelFormViewCommand = new RelayCommand(o => {
                ChannelFormVM = new ChannelFormViewModel();
                CurrentView = ChannelFormVM;
            });
        }
    }
}
