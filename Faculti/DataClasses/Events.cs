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
    public class Events : DatabaseWorker, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
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
        /// Poupulates the event information after setting the value of event Id.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT EVENT_ID, EVENT_TYPE, EVENT_TITLE, EVENT_DESC, EVENT_DATE, CREATED_TIME FROM EVENTS WHERE EVENT_ID = {Id}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Poupulates the event information using the event Id parameter.
        /// </summary>
        public async void RetrieveInfoAsync(int eventId, IDbConnection connection)
        {
            var cmdText = $@"SELECT EVENT_ID, EVENT_TYPE, EVENT_TITLE, EVENT_DESC, EVENT_DATE, CREATED_TIME FROM EVENTS WHERE EVENT_ID = {eventId}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                Type = reader.IsDBNull(1) ? null : reader.GetString(1);
                Title = reader.IsDBNull(2) ? null : reader.GetString(2);
                Description = reader.IsDBNull(3) ? null : reader.GetString(3);
                Date = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetOracleDate(4).Value;
                CreatedTime = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetOracleDate(5).Value;
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



