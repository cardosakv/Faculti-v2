using Faculti.DataRepo.DatabaseManager;
using Faculti.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataClasses
{
    public class Comment : DatabaseWorker, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        private User _author;
        public User Author
        {
            get { return _author; }
            set { _author = value; }
        }

        private User _mentionedUser;
        public User MentionedUser
        {
            get { return _mentionedUser; }
            set { _mentionedUser = value; }
        }

        private DateTime _createdTime;
        public DateTime CreatedTime
        {
            get { return _createdTime; }
            set
            {
                _createdTime = value;
                ElapsedTime = Time.ToElapsed(_createdTime);
                OnPropertyChanged("ElapsedTime");
            }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get { return _elapsedTime; }
            private set { _elapsedTime = value; }
        }

        /// <summary>
        /// Gets the general information of the post author.
        /// </summary>
        public void RetrieveAuthorInfo(IDbConnection connection)
        {
            Author.RetrieveGeneralInfoAsync(Author.Id, connection);
        }

        /// <summary>
        /// Poupulates the comment information after setting the value of comment Id.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT COMMENT_ID, AUTHOR_ID, TEXT, MENTIONED_ID, CREATED_TIME FROM COMMENTS WHERE COMMENT_ID = {Id}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Poupulates the comment information using the specified commend Id parameter.
        /// </summary>
        public async void RetrieveInfoAsync(int commentId, IDbConnection connection)
        {
            var cmdText = $@"SELECT COMMENT_ID, AUTHOR_ID, TEXT, MENTIONED_ID, CREATED_TIME FROM COMMENTS WHERE COMMENT_ID = {commentId}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            Author = new();
            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                Author.Id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                Text = reader.IsDBNull(2) ? null : reader.GetString(2);
                MentionedUser.Id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                CreatedTime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetOracleDate(4).Value;
            }
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
