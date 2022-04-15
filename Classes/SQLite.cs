using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Data.Sqlite;
using StreamScheduler.MVVM.ViewModels;

namespace StreamScheduler
{
    internal class SQLite
    {
        private SqliteConnection connection = new SqliteConnection("Data Source=SchedulerDB.db;");
        private SqliteCommand command = new SqliteCommand();
        private SqliteDataReader reader;

        public SQLite() {
            CreateDatabase();
        }

        private void CreateDatabase() {
            connection.Open();
            command.Connection = connection;
            command.CommandText = "PRAGMA encoding = \"UTF-16\";"+
                                    "CREATE TABLE IF NOT EXISTS Channels (" +
                                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                    "Name Varchar(100) NOT NULL," +
                                    "Description Varchar(10000),"+
                                    "Url Varchar(100) NOT NULL," +
                                    "UNIQUE (Url)" +
                                  ");"+
                                  "CREATE TABLE IF NOT EXISTS Videos (" +
                                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                    "Title Varchar(200) NOT NULL," +
                                    "VideoUrl Varchar(2000) NOT NULL," +
                                    "Description Varchar(5000),"+
                                    "ThumbnailUrl Varchar(2000)," +
                                    "StartDateTime DateTime NOT NULL," +
                                    "ChannelUrl Varchar(2000) NOT NULL," +
                                    "UNIQUE (VideoUrl,ChannelUrl)" +
                                    "FOREIGN KEY(ChannelUrl) REFERENCES Channels(Url) ON DELETE CASCADE"+
                                  ");" +
                                  "CREATE TABLE IF NOT EXISTS Playlist (" +
                                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                    "VideoUrl INTEGER NOT NULL," +
                                    "UserNotified TINYINT DEFAULT 0," +
                                    "UNIQUE (VideoUrl)" +
                                  ");" + 
                                  "CREATE TABLE IF NOT EXISTS Settings (" +
                                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                    "Name Varchar(200) NOT NULL," +
                                    "Value Varchar(200) NOT NULL,"+
                                    "UNIQUE (Name)"+
                                  ");";
            try {
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public void InsertChannel(ChannelViewModel channel) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "INSERT INTO Channels(Name,Url,Description)"+
                                     " Values ('" + channel.ChannelName + "','" + channel.ChannelUrl + "','"+channel.ChannelDescription.Replace("'", "''") + "')";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void UpdateChannel(ChannelViewModel channel) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "UPDATE Channels SET Name = '" + channel.ChannelName + "',Description = '" + channel.ChannelDescription.Replace("'", "''") + "'" +
                                     " WHERE Url = '"+channel.ChannelUrl+"'";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void DeleteChannel(ChannelViewModel channel) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "DELETE FROM Channels "+
                                     " WHERE Url = '" + channel.ChannelUrl + "'";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }


        public void InsertVideo(VideoViewModel video) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "INSERT INTO Videos(Title,VideoUrl,ChannelUrl,ThumbnailUrl,StartDateTime,Description)" +
                                     " VALUES (\"" + video.Title + "\",\"" + video.VideoUrl + "\",\"" +
                                                     video.ChannelUrl + "\",\"" + video.ThumbnailUrl + "\",'" +
                                                     video.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "',\""+
                                                     video.VideoDescription+"\");";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void UpdateVideo(VideoViewModel video) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "UPDATE Video SET Title = \"" + video.Title + "\", VideoUrl = \"" + video.VideoUrl +
                                      "\", ChannelUrl = \"" + video.ChannelUrl + "\",ThumbnailUrl = \"" + video.ThumbnailUrl +
                                      "\", StartDateTime ='" + video.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'"+
                                      ",\""+ video.VideoDescription +"\""+
                                      " WHERE Url = '" + video.VideoUrl + "'";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void DeleteVideo(VideoViewModel video) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "DELETE FROM Videos " +
                                     " WHERE VideoUrl = '" + video.ChannelUrl + "'";
                command.ExecuteNonQuery();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public void UpdateVideos(List<Video> listVideos) {
            connection.Open();
            command.Connection = connection;
            string formatedDateTime = "";

            try {
                foreach (Video video in listVideos) {
                    //formatedDateTime = video.StartDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    formatedDateTime = video.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    command.CommandText = "INSERT OR IGNORE INTO Videos(Title,VideoUrl,ChannelUrl,ThumbnailUrl,StartDateTime)" +
                              "\n VALUES (\"" + video.Title + "\",\"" + video.VideoUrl + "\",\"" + video.ChannelUrl + "\",\"" + video.ThumbnailUrl + "\",'" + formatedDateTime + "');"+
                              "\n UPDATE videos SET Title = \"" + video.Title + "\",ChannelUrl = \"" + video.ChannelUrl + "\", ThumbnailUrl = \"" + video.ThumbnailUrl + "\", StartDateTime = '" + formatedDateTime + "' WHERE VideoUrl =\"" + video.VideoUrl + "\"";
                    //Clipboard.SetText(command.CommandText);
                    command.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public ObservableCollection<VideoViewModel> ListAvailableVideos() {
            SqliteDataReader reader;
            ObservableCollection<VideoViewModel> listVideos = new ObservableCollection<VideoViewModel>();
            connection.Open();
            command.Connection = connection;
            command.CommandText = "SELECT VID.Title,VID.ThumbnailUrl,VID.VideoUrl,VID.StartDateTime,VID.ChannelUrl,CHA.Name FROM Videos VID" +
                                  " LEFT JOIN Channels CHA ON VID.ChannelUrl = CHA.Url"+
                                  " WHERE VID.StartDateTime > datetime('now','localtime')";
            try {
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    Video video = new Video(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(4));
                    video.SetStartDateTimeSQL(reader.GetString(3));
                    Channel channel = new Channel(reader.GetString(5), reader.GetString(4));
                    listVideos.Add(new VideoViewModel(video,channel));
                }
                reader.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
            return listVideos;
        }
        public ObservableCollection<VideoViewModel> ListAvaliablePlaylistVideos() {
            SqliteDataReader reader;
            ObservableCollection<VideoViewModel> listVideos = new ObservableCollection<VideoViewModel>();
            connection.Open();
            command.Connection = connection;
            command.CommandText = "SELECT VID.Title,VID.ThumbnailUrl,VID.VideoUrl,VID.StartDateTime,VID.ChannelUrl,CHA.Name"+
                                  " FROM Playlist PLAY" +
                                  " LEFT JOIN Videos VID ON PLAY.VideoUrl = VID.VideoUrl"+
                                  " LEFT JOIN Channels CHA ON VID.ChannelUrl = CHA.Url"+
                                  " WHERE PLAY.UserNotified = 0";
            try {
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    Video video = new Video(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(4));
                    video.SetStartDateTimeSQL(reader.GetString(3));
                    Channel channel = new Channel(reader.GetString(5), reader.GetString(4));
                    listVideos.Add(new VideoViewModel(video, channel));
                }
                reader.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
            return listVideos;
        }

        public void AddPlaylistVideo(string videoUrl) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "INSERT OR IGNORE INTO Playlist(VideoUrl) Values ('" + videoUrl + "')";
                command.ExecuteNonQuery();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void UpdatePlaylistVideoUserNotified(string videoUrl) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "UPDATE Playlist SET UserNotified = 1 WHERE VideoUrl = '" + videoUrl + "'";
                command.ExecuteNonQuery();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public void DeletePlaylistVideo(string videoUrl) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "DELETE FROM Playlist WHERE VideoUrl = '" + videoUrl + "'";
                command.ExecuteNonQuery();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }
        public void ClearPlaylistVideos() {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "DELETE FROM Playlist";
                command.ExecuteNonQuery();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public void UpdateSettings(string name,string value) {
            connection.Open();
            command.Connection = connection;
            try {
                command.CommandText = "INSERT OR IGNORE INTO Settings(Name,Value) Values ('" + name + "','" + value + "');" +
                    "\n UPDATE Settings SET Value = '" + value + "' WHERE Name = '" + name + "'";
                command.ExecuteNonQuery();

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
        }

        public string GetSettingValue(string name) {
            connection.Open();
            command.Connection = connection;
            SqliteDataReader reader;
            string value = "";
            try {
                command.CommandText = "SELECT Value FROM Settings WHERE Name = '"+name+"'";
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    value = reader.GetString(0);
                }
                reader.Close();
                } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
            return value;
        }

        public ObservableCollection<ChannelViewModel> GetAllChannelsNames() {
            ObservableCollection<ChannelViewModel> channels = new ObservableCollection<ChannelViewModel>();
            try {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT name,url,description FROM channels";
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    channels.Add(new ChannelViewModel (new Channel (reader.GetString(0),reader.GetString(1),reader.GetString(2))));
                }
                reader.Close();
                connection.Close();
            } catch (Exception e) {
                MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
            }
            return channels;
        }

        public ObservableCollection<VideoViewModel> GetAllVideos() {
            ObservableCollection<VideoViewModel> videos = new ObservableCollection<VideoViewModel>();
            try {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT VID.Title,VID.ThumbnailUrl,VID.VideoUrl,VID.StartDateTime,IFNULL(VID.Description,''),VID.ChannelUrl,CHA.Name FROM Videos VID" +
                                      " LEFT JOIN Channels CHA ON VID.ChannelUrl = CHA.Url";
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    Video video = new Video(reader.GetString(0), reader.GetString(1), reader.GetString(2),reader.GetString(4), reader.GetString(5));
                    video.SetStartDateTimeSQL(reader.GetString(3));
                    Channel channel = new Channel(reader.GetString(6), reader.GetString(5));
                    videos.Add(new VideoViewModel(video,channel));
                }
                reader.Close();
                connection.Close();
            } catch (Exception e) {
                MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
            }
            return videos;
        }
    }
}
