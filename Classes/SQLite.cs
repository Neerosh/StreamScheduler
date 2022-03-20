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
                                    "ChannelUrl Varchar(2000) NOT NULL," +
                                    "ThumbnailUrl Varchar(2000)," +
                                    "StartDateTime DateTime NOT NULL," +
                                    "UNIQUE (VideoUrl)" +
                                  ");"+
                                  "CREATE TABLE IF NOT EXISTS Playlist (" +
                                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                    "VideoUrl INTEGER NOT NULL" +
                                  ");"+ 
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

        public Channel[] channelsHololive = new Channel[] {
                new Channel( "Tokino Sora","UCp6993wxpyDPHUpavwDFqgg"),
                new Channel( "Roboco","UCp6993wxpyDPHUpavwDFqgg" ),
                new Channel( "Suisei" ,"UC5CwaMl1eIgY8h02uZw7u8A"),
                new Channel( "Sakura Miko","UC-hM6YJuNYVAmUWxeIr9FeA" ),
                new Channel( "Azki" ,"UC0TXe_LYZ4scaW2XMyi5_kw" ),
                new Channel( "Yozora Mel","UCD8HOxPs4Xvsm8H0ZxXGiBw"),
                new Channel( "Shirakami Fubuki","UCdn5BQ06XqgXoAxIhbqw5Rg" ),
                new Channel( "Akai Haato","UC1CfXB_kRs3C-zaeTG3oGyg" ),
                new Channel( "Akirosenthal" ,"UCFTLzh12_nrtzqBPsTCqenA"),
                new Channel( "Natsuiro Matsuri","UCFTLzh12_nrtzqBPsTCqenA" ),
                new Channel( "Minato Aqua","UC1opHUrw8rvnsadT-iGp7Cg" ),
                new Channel( "Murasaki Shion","UCXTpFs_3PqI41qX2d9tL2Rw" ),
                new Channel( "Nakiri Ayame","UC7fk0CB07ly8oSl0aqKkqFg" ),
                new Channel( "Yuzuki Choco","UC1suqwovbL1kzsoaZgFZLKg" ),
                new Channel( "Oozora Subaru","UCvzGlP9oQwU--Y0r9id_jnA" ),
                new Channel( "Uruha Rushia","UCl_gCybOJRIgOXw6Qb4qJzQ" ),
                new Channel( "Kiryu Coco","UCS9uQI-jC3DE0L4IpXyvr6w" )
                };
        public void AddChannels() {
            connection.Open();
            command.Connection = connection;
            try {
                foreach (Channel channel in channelsHololive) {
                    command.CommandText = "INSERT OR IGNORE INTO Channels(Name,Url,Description) Values ('" + channel.Name + "','" + channel.Url + "','')";
                    command.ExecuteNonQuery();
                }
                
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

        public void UpdateVideos(List<Video> listVideos) {
            connection.Open();
            command.Connection = connection;
            string formatedDateTime = "";

            try {
                foreach (Video video in listVideos) {
                    //formatedDateTime = video.StartDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    formatedDateTime = video.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    command.CommandText = "INSERT OR IGNORE INTO videos(Title,VideoUrl,ChannelUrl,ThumbnailUrl,StartDateTime)" +
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
                    Video video = new Video(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetString(5));
                    video.SetStartDateTimeSQL(reader.GetString(3));
                    listVideos.Add(new VideoViewModel(video));
                }
                reader.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
            return listVideos;
        }
        public List<Video> ListPlaylistVideos() {
            SqliteDataReader reader;
            List<Video> listVideos = new List<Video>();
            connection.Open();
            command.Connection = connection;
            command.CommandText = "SELECT VID.Title,VID.ThumbnailUrl,VID.VideoUrl,VID.StartDateTime,VID.ChannelUrl,CHA.Name FROM Videos VID" +
                                  " LEFT JOIN Channels CHA ON VID.ChannelUrl = CHA.Url WHERE VID.VideoUrl IN (SELECT VideoUrl FROM Playlist)";
            try {
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    Video video = new Video(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(4), reader.GetString(5));
                    video.SetStartDateTimeSQL(reader.GetString(3));
                    listVideos.Add(video);
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
        public void DeleteAllPlaylistVideos() {
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

        public string GetGoogleAPIKey() {
            connection.Open();
            command.Connection = connection;
            SqliteDataReader reader;
            string googleApiKey = "";
            try {
                command.CommandText = "SELECT Value FROM Settings WHERE Name = 'GoogleAPIKey'";
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    googleApiKey = reader.GetString(0);
                }
                reader.Close();
                } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
            connection.Close();
            return googleApiKey;
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


        //public List<String> GetAllChannelsNames() {
        //    List<String> channelsNames = new List<String>();
        //    try {
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandText = "SELECT name FROM channels";
        //        reader = command.ExecuteReader();
        //        while (reader.Read()) {
        //            channelsNames.Add(reader.GetString(0));
        //        }
        //        reader.Close();
        //        connection.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return channelsNames;
        //}
        //public String GetChannelUrl(string name) {
        //    String channelUrl = "";
        //    try {
        //        MySqlConnection con = CreateConnection();
        //        con.Open();
        //        MySqlCommand command = new MySqlCommand("SELECT url FROM channels WHERE name = \"" + name + "\"", con);
        //        MySqlDataReader dataReader = command.ExecuteReader();
        //        while (dataReader.Read()) {
        //            channelUrl = dataReader["url"].ToString();
        //        }
        //        dataReader.Close();
        //        con.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return channelUrl;
        //}
        //public String GetChannelName(string url) {
        //    String channelName = "";
        //    try {
        //        MySqlConnection con = CreateConnection();
        //        con.Open();
        //        MySqlCommand command = new MySqlCommand("SELECT name FROM channels WHERE url = \"" + url + "\"", con);
        //        MySqlDataReader dataReader = command.ExecuteReader();
        //        while (dataReader.Read()) {
        //            channelName = dataReader["name"].ToString();
        //        }
        //        dataReader.Close();
        //        con.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return channelName;
        //}
        //public int GetGroupId(string groupName) {
        //    int groupId = 0;
        //    try {
        //        MySqlConnection con = CreateConnection();
        //        con.Open();
        //        MySqlCommand command = new MySqlCommand("SELECT groupId FROM groups WHERE name = \"" + groupName + "\"", con);
        //        MySqlDataReader dataReader = command.ExecuteReader();
        //        while (dataReader.Read()) {
        //            groupId = Convert.ToInt32(dataReader["groupId"]);
        //        }
        //        dataReader.Close();
        //        con.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return groupId;
        //}

        //public void UpdateGroups() {
        //    string groupName = "", command = "", channelUrl = "";
        //    int groupId = 0;

        //    for (int i = 0; i < groups.Length; i++) {
        //        groupName = groups[i];
        //        command = "INSERT INTO groups(groupName) " +
        //                  "SELECT \"" + groupName + "\" WHERE NOT EXISTS (SELECT groupName FROM groups WHERE groupName = \"" + groupName + "\" LIMIT 1)";
        //        RunNonQueryCommandSQL(command);
        //        groupId = GetGroupId(groupName);
        //        switch (groupName) {
        //            case "Hololive":
        //                for (int c = 0; c < channelsHololive.GetLength(0); c++) {
        //                    channelUrl = channelsHololive[c, 0];
        //                    UpdateGroupChannels(groupId, channelUrl);
        //                }
        //                break;
        //            case "Nijisanji":
        //                for (int c = 0; c < channelsNijisanji.GetLength(0); c++) {
        //                    channelUrl = channelsNijisanji[c, 0];
        //                    UpdateGroupChannels(groupId, channelUrl);
        //                }
        //                break;
        //        }
        //    }
        //}
        //public void UpdateGroupChannels(int groupId, string channelUrl) {
        //    string command = "";
        //    command = "INSERT INTO groupChannels(groupId,channelUrl) " +
        //              "SELECT " + groupId + ",\"" + channelUrl + "\" WHERE NOT EXISTS (SELECT groupId FROM groupChannels WHERE groupId = " + groupId + "AND channelUrl = \"" + channelUrl + "\" LIMIT 1)";
        //    RunNonQueryCommandSQL(command);
        //}
        //public void UpdateChannels() {
        //    string url = "", name = "", command = "", groupName = "";

        //    for (int i = 0; i < groups.Length; i++) {
        //        groupName = groups[i];
        //        switch (groupName) {
        //            case "Hololive":
        //                for (int c = 0; c < channelsHololive.GetLength(0); c++) {
        //                    url = channelsHololive[c, 0];
        //                    name = channelsHololive[c, 1];
        //                    command = "INSERT INTO channels(name,url) " +
        //                              "SELECT \"" + name + "\",\"" + url + "\" WHERE NOT EXISTS (SELECT Url FROM channels WHERE Url = \"" + url + "\" LIMIT 1)";
        //                    RunNonQueryCommandSQL(command);
        //                }
        //                break;
        //            case "Nijisanji":
        //                for (int c = 0; c < channelsNijisanji.GetLength(0); c++) {
        //                    url = channelsNijisanji[c, 0];
        //                    name = channelsNijisanji[c, 1];
        //                    command = "INSERT INTO channels(name,url) " +
        //                              "SELECT \"" + name + "\",\"" + url + "\" WHERE NOT EXISTS (SELECT Url FROM channels WHERE Url = \"" + url + "\" LIMIT 1)";
        //                    RunNonQueryCommandSQL(command);
        //                }
        //                break;
        //        }
        //    }
        //}
        //public void UpdateVideos(List<Video> listVideos) {
        //    string command = "", formatedDateTime = "";

        //    foreach (Video video in listVideos) {
        //        formatedDateTime = video.startDateTime.ToString("M/d/yyyy HH:mm:ss");
        //        command = "INSERT INTO videos(url,title,thumbnailUrl,startDateTime,channelUrl) " +
        //                  "SELECT \"" + video.url + "\",\"" + video.title + "\",\"" + video.thumbnail + "\",STR_TO_DATE('" + formatedDateTime + "','%m/%d/%Y %H:%i:%s'),\"" + video.channelUrl + "\"" +
        //                  " WHERE NOT EXISTS (SELECT url FROM videos WHERE url = \"" + video.url + "\" LIMIT 1)";
        //        Clipboard.SetText(command);
        //        MessageBox.Show(RunNonQueryCommandSQL(command));
        //    }
        //}

        //public List<Video> LoadTableVideos() {
        //    string commandString = "";
        //    string title, url, thumbnail, channelUrl, startDateTime;
        //    List<Video> listVideos = new List<Video>();
        //    Video video;
        //    try {
        //        MySqlConnection con = CreateConnection();
        //        con.Open();
        //        commandString = "SELECT Title, ThumbnailUrl, Url, StartDateTime, ChannelUrl FROM videos";
        //        MySqlCommand command = new MySqlCommand(commandString, con);
        //        MySqlDataReader dataReader = command.ExecuteReader();
        //        while (dataReader.Read()) {
        //            title = dataReader["title"].ToString();
        //            thumbnail = dataReader["ThumbnailUrl"].ToString();
        //            url = dataReader["url"].ToString();
        //            startDateTime = dataReader["StartDateTime"].ToString();
        //            channelUrl = dataReader["ChannelUrl"].ToString();
        //            video = new Video(title, thumbnail, url, startDateTime, channelUrl);
        //            listVideos.Add(video);

        //        }
        //        dataReader.Close();
        //        con.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return listVideos;
        //}

        //public DataSet LoadTableVideosDS() {
        //    string commandString = "";
        //    DataSet ds = new DataSet();

        //    try {
        //        MySqlConnection con = CreateConnection();
        //        con.Open();
        //        commandString = "SELECT VD.Title, VD.ThumbnailUrl, VD.Url, VD.StartDateTime, VD.ChannelUrl, CH.Name as 'ChannelName' FROM Videos VD LEFT JOIN Channels CH ON CH.Url = VD.ChannelUrl";
        //        MySqlCommand command = new MySqlCommand(commandString, con);

        //        MySqlDataAdapter adp = new MySqlDataAdapter(command);
        //        adp.Fill(ds, "LoadDataBinding");
        //        con.Close();
        //    } catch (Exception e) {
        //        MessageBox.Show("Error when executing SQLCommand:\n" + e.ToString());
        //    }
        //    return ds;
        //}


    }
}
