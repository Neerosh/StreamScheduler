using StreamScheduler.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.Commands
{
    public class AddToPlaylistCommand : CommandBase
    {
        private readonly VideoViewModel _video;
        public AddToPlaylistCommand(VideoViewModel video) { 
            _video = video;
        }
        public override void Execute(object parameter) {
            SQLite sqlite = new SQLite();
            VideoViewModel video = (VideoViewModel) parameter;
            sqlite.AddPlaylistVideo(video.VideoUrl);
        }
    }
}
