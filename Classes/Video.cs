﻿using System;
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
        public string ChannelUrl { get; set; }
        public string ChannelName { get; set; }


        public Video(string title, string thumbnailUrl, string videoUrl, string channelUrl) {
            this.Title = title;
            this.ThumbnailUrl = thumbnailUrl;
            this.VideoUrl = videoUrl;
            this.ChannelUrl = channelUrl;
            //channelName = sql.GetChannelName(channelUrl);
        }
        public Video(string title, string thumbnailUrl, string videoUrl, string channelUrl,string channelName) {
            this.Title = title;
            this.ThumbnailUrl = thumbnailUrl;
            this.VideoUrl = videoUrl;
            //this.StartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            this.ChannelUrl = channelUrl;
            this.ChannelName = channelName;
        }

        public void SetStartDateTimeYoutube(string startDateTime) {
            try {
                StartDateTime = DateTime.Parse(startDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None);
                this.StartDateTime = DateTime.ParseExact(startDateTime, "M/d/yyyy H:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
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