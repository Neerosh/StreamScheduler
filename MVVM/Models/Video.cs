using System;
using System.Globalization;
using System.Windows;

namespace StreamScheduler
{
    public class Video
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Description { get; set; }
        public string ChannelUrl { get; set; }

        public Video(string title, string thumbnailUrl, string videoUrl) {
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            VideoUrl = videoUrl;
        }
        public Video(string title, string thumbnailUrl, string videoUrl, string channelUrl) {
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            VideoUrl = videoUrl;
            ChannelUrl = channelUrl;
        }
        public Video(string title, string thumbnailUrl, string videoUrl,string description, string channelUrl) {
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            VideoUrl = videoUrl;
            ChannelUrl = channelUrl;
            Description = description;
        }

        public void SetStartDateTimeYoutube(string startDateTime) {
            try {
                StartDateTime = DateTime.Parse(startDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None);
                StartDateTime = DateTime.ParseExact(startDateTime, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void SetStartDateTimeSQL(string startDateTime) {
            try {
                StartDateTime = DateTime.ParseExact(startDateTime, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
