using Faculti.DataRepo.DatabaseManager;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataClasses
{
    public class Messages : DatabaseWorker, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _senderId;
        public int SenderId
        {
            get { return _senderId; }
            set { _senderId = value; }
        }

        private int _receiverId;
        public int ReceiverId
        {
            get { return _receiverId; }
            set { _receiverId = value; }
        }

        private Image _image;
        public Image Image
        {
            get { return _image; }
            set 
            { 
                _image = value;
                OnPropertyChanged("Image");
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set 
            { 
                _text = value.Trim();
                OnPropertyChanged("Text");
            }
        }

        private bool _hasRead;
        public bool HasRead
        {
            get { return _hasRead; }
            set
            {
                _hasRead = value;
                OnPropertyChanged("HasRead");
            }
        }

        /// <summary>
        ///  Poupulates the inbox information after setting the value of inbox Id.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT HAS_READ FROM MESSAGES WHERE MESSAGE_ID = {Id}";

            await Task.Run(() =>
            {
                using IDbCommand command = Database.CreateCommand(cmdText, connection);
                using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        if (reader.GetString(0) == "Y")
                            HasRead = true;
                        else
                            HasRead = false;
                    }
                }
            });
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion
    }
}
