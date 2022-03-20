using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace StreamScheduler
{
    internal class Youtube
    {
        private SQLite sql = new SQLite();

        private YouTubeService CreateYoutubeService() {
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                ApiKey = sql.GetGoogleAPIKey(),
                ApplicationName = this.GetType().ToString(),
            });
            return youtubeService;
        }
        public async Task<List<Channel>> GetChannelInformation(string channelUrl) {
            var youtubeService = CreateYoutubeService();
            List<Channel> listChannels = new List<Channel>();

            if (youtubeService.ApiKey.Equals("")) { return listChannels; }
            var searchListRequest = youtubeService.Channels.List("snippet");
            searchListRequest.Id = channelUrl;
            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();
            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            if (searchListResponse.Items == null) { return listChannels; }
            foreach (var searchResult in searchListResponse.Items) {
                switch (searchResult.Kind) {
                    case "youtube#channel":
                        listChannels.Add(new Channel(searchResult.Snippet.Title, searchResult.Id, searchResult.Snippet.Description));
                        break;
                }
            }
            return listChannels;
        }
        public async Task<List<Video>> GetUpcomingVideos(string channelUrl) {
            List<Video> listVideos = new List<Video>();
            List<string[]> videoids = new List<string[]>();

            if (channelUrl == null || channelUrl.Equals("")) { return listVideos; }

            string videosIds = "";
            List<string[,]> videosTimes = new List<string[,]>();
            //string starttime;

            // Create the service.
            var youtubeService = CreateYoutubeService();

            if (youtubeService.ApiKey.Equals("")) { return listVideos; }

            var searchListRequest = youtubeService.Search.List("snippet");
            //searchListRequest.Q = "Free"; // Replace with your search term.
            searchListRequest.ChannelId = channelUrl;
            //searchListRequest.MaxResults = 10;
            searchListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.Upcoming;
            searchListRequest.Type = "video";

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            if (searchListResponse.Items == null) { return listVideos; }
            foreach (var searchResult in searchListResponse.Items) {
                switch (searchResult.Id.Kind) {
                    case "youtube#video":
                        if (videosIds.Length > 0) {
                            videosIds += ",";
                        }
                        videosIds += searchResult.Id.VideoId;
                        Video video = new Video(searchResult.Snippet.Title, searchResult.Snippet.Thumbnails.Medium.Url, searchResult.Id.VideoId, channelUrl);
                        //video.channelName = searchResult.Snippet.ChannelTitle;
                        listVideos.Add(video);
                        break;
                }
            }
            //videosTimes = await GetVideoStartTime(videosIds);
            var searchListRequest2 = youtubeService.Videos.List("liveStreamingDetails");
            searchListRequest2.Id = videosIds;
            //searchListRequest.MaxResults = 1;
            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse2 = await searchListRequest2.ExecuteAsync();
            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult2 in searchListResponse2.Items) {
                switch (searchResult2.Kind) {
                    case "youtube#video":
                        foreach (Video video in listVideos) {
                            if (searchResult2.Id.Equals(video.VideoUrl) && searchResult2.LiveStreamingDetails.ScheduledStartTime != null) {
                                video.SetStartDateTimeYoutube(searchResult2.LiveStreamingDetails.ScheduledStartTime.ToString());
                            }
                        }
                        break;
                }
            }
            return listVideos;
        }

    }
}
