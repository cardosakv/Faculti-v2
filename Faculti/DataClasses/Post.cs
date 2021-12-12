using Faculti.DataRepo.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Faculti.Helpers;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.IO;

namespace Faculti.DataClasses
{
    public class Post : DatabaseWorker, INotifyPropertyChanged
    {
        public int Id { get; set; }

        public User Author { get; set; }

        private string _type;
        public string Type
        {
            get => _type;
            set 
            { 
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private Image _image;
        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private User _mentionedUser;
        public User MentionedUser
        {
            get { return _mentionedUser; }
            set { _mentionedUser = value; }
        }

        public Class Class { get; set; }

        private DateTime _createdTime;
        public DateTime CreatedTime
        {
            get => _createdTime;
            set 
            { 
                _createdTime = value;
                ElapsedTime = Time.ToElapsed(_createdTime);
                OnPropertyChanged("ElapsedTime");
            }
        }

        public string ElapsedTime { get; private set; }

        /// <summary>
        /// Gets the general information of the post author.
        /// </summary>
        public void RetrieveAuthorInfo(IDbConnection connection)
        {
            Author.RetrieveGeneralInfoAsync(Author.Id, connection);
        }

        /// <summary>
        /// Populates the post information after setting the value of post Id.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT POST_ID, POST_TYPE, AUTHOR_ID, TEXT, CLASS_ID, MENTIONED_ID, CREATED_TIME, IMAGE FROM POSTS WHERE POST_ID = {Id}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Populates the post information using the postId parameter.
        /// </summary>
        public async void RetrieveInfoAsync(int postId, IDbConnection connection)
        {
            var cmdText = $@"SELECT POST_ID, POST_TYPE, AUTHOR_ID, TEXT, CLASS_ID, MENTIONED_ID, CREATED_TIME, IMAGE FROM POSTS WHERE POST_ID = {postId}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            Author = new();
            Class = new();
            MentionedUser = new();

            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (!reader.Read()) return;
            
            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
            Type = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            Author.Id = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            Text = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            Class.Id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            MentionedUser.Id = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
            CreatedTime = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetOracleDate(6).Value;

            byte[] image = reader.IsDBNull(7) ? null : (byte[])reader["IMAGE"];
            if (image == null) return;
            
            MemoryStream ms = new(image);
            Image = Image.FromStream(ms);
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
