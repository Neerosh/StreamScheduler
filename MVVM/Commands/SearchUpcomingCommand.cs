using StreamScheduler.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamScheduler.MVVM.Commands
{
    public class SearchUpcomingCommand : CommandBase
    {
        private readonly VideoViewModel _video;
        public SearchUpcomingCommand(VideoViewModel video) { 
            _video = video;
        }
        public override void Execute(object parameter) {
            SQLite sqlite = new SQLite();
            List<Video> listVideos = null;
            Youtube youtube = new Youtube();
            SQLite sql = new SQLite();
            //lblSearch.Visibility = Visibility.Visible;

            //listVideos = await youtube.GetUpcomingVideos(channelUrl);
            sql.UpdateVideos(listVideos);
        }
    }
}
