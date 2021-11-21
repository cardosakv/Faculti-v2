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
    public class Inbox : DatabaseWorker, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private User _contact;
        public User Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        private string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            set
            {
                _lastMessage = value;
                OnPropertyChanged("LastMessage");
            }
        }

        private DateTime _lastUpdate;
        public DateTime LastUpdate
        { 
            get { return _lastUpdate; }
            set 
            { 
                _lastUpdate = value;
                ElapsedTime = Time.ToElapsed(_lastUpdate);
                OnPropertyChanged("ElapsedTime");
            }
        }

        private string _elapsedTime;
        public string ElapsedTime
        {
            get { return _elapsedTime; }
            private set { _elapsedTime = value; }
        }

        private int _lastUserId;
        public int LastUserId
        {
            get { return _lastUserId; }
            set { _lastUserId = value; }
        }

        /// <summary>
        /// Gets the general information of the contact.
        /// </summary>
        public void RetrieveAuthorInfo(IDbConnection connection)
        {
            Contact.RetrieveGeneralInfoAsync(Contact.Id, connection);
        }

        /// <summary>
        /// Poupulates the inbox information using the value of inbox Id parameter.
        /// </summary>
        public async void RetrieveInfoAsync(int userId, IDbConnection connection)
        {
            var cmdText = $@"SELECT INBOX_ID, USER_ID1, USER_ID2, LAST_MESSAGE, LAST_UPDATE, LAST_USER_ID FROM INBOXES WHERE USER_ID1 = {userId} OR USER_ID2 = {userId}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        ///  Poupulates the inbox information after setting the value of inbox Id.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT INBOX_ID, USER_ID1, USER_ID2, LAST_MESSAGE, LAST_UPDATE, LAST_USER_ID FROM INBOXES WHERE INBOX_ID = {Id}";
            
            await Task.Run(() => ReadData(cmdText, connection));
        }


        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            Contact = new();

            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                var userId1 = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                var userId2 = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                Contact.Id = userId1 == 0 ? userId2 : userId1;
                LastMessage = reader.IsDBNull(3) ? null : reader.GetString(3);
                LastUpdate = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetOracleDate(4).Value;
                LastUserId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);            }
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
