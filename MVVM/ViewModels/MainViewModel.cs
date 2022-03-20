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
            SearchVM = new SearchViewModel();
            PlaylistVM = new PlaylistViewModel();
            SettingsVM = new SettingsViewModel();
            ChannelFormVM = new ChannelFormViewModel();
            CurrentView = SearchVM;

            SearchViewCommand = new RelayCommand(o => {
               CurrentView = SearchVM;
            });
            PlaylistViewCommand = new RelayCommand(o => {
                CurrentView = PlaylistVM;
            });
            SettingsViewCommand = new RelayCommand(o => {
                CurrentView = SettingsVM;
            });
            ChannelFormViewCommand = new RelayCommand(o => {
                CurrentView = ChannelFormVM;
            });
        }
    }
}
