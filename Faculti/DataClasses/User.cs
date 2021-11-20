using Faculti.DataRepo.DatabaseManager;
using Faculti.Helpers;
using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Faculti.DataClasses
{
    public class User : DatabaseWorker, INotifyPropertyChanged
    {
        public User() { }

        public User(UserType type, string? email, string? phoneNumberInHash, string passwordInHash)
        {
            _type = type;
            _email = email;
            _phoneNumberInHash = phoneNumberInHash;
            _passwordInHash = passwordInHash;
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private UserType _type;
        public UserType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set 
            { 
                _email = value; 
                OnPropertyChanged("Email"); 
            }
        }

        private string _passwordInHash;
        public string PasswordInHash
        {
            get { return _passwordInHash; }
            set 
            { 
                _passwordInHash = value; 
                OnPropertyChanged("PasswordInHash"); 
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set 
            { 
                _firstName = value; 
                OnPropertyChanged("FirstName"); 
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set 
            { 
                _lastName = value; 
                OnPropertyChanged("LastName"); 
            }
        }

        public string FullName
        {
            get { return $"{_firstName} {_lastName}"; }
        }

        private string _phoneNumberInHash;
        public string PhoneNumberInHash
        {
            get { return _phoneNumberInHash; }
            set 
            { 
                _phoneNumberInHash = value; 
                OnPropertyChanged("PhoneNumberInHash"); 
            }
        }

        private Image _picture;
        public Image Picture
        {
            get { return _picture; }
            set 
            { 
                _picture = value; 
                OnPropertyChanged("Picture"); 
            }
        }

        private DateTime _lastOnline;
        public DateTime LastOnline
        {
            get { return _lastOnline; }
            set 
            { 
                _lastOnline = value;
                OnPropertyChanged("LastOnline");
            }
        }

        private Status _status;
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                SetStatusAsync(value);
                OnPropertyChanged("Status");
            }
        }

        private DateTime _lastPicChange;
        public DateTime LastPicChange
        {
            get { return _lastPicChange; }
            set { _lastPicChange = value; }
        }

        private IDbConnection _dbConnection;
        public IDbConnection DbConnection
        {
            get { return _dbConnection; }
            set 
            {
                _dbConnection.Close();
                _dbConnection = value; 
            }
        }

        /// <summary>
        /// Opens only one database connection for the user.
        /// </summary>
        public async Task CreateConnection()
        {
            if (_dbConnection != null) _dbConnection.Close();
            
            _dbConnection = await Database.CreateOpenConnectionAsync();
        }

        /// <summary>
        /// Adds the user to the database.
        /// </summary>
        public async void AddToDatabaseAsync()
        {
            try
            {
                var cmdText = $@"INSERT INTO USERS (FIRST_NAME, LAST_NAME, EMAIL, PASSWORD_IN_HASH, PHONE_NUMBER_IN_HASH) values ('{FirstName}', '{LastName}', '{Email}', '{PasswordInHash}', '{PhoneNumberInHash}')";
                using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);
                await Task.Run(() => command.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user to the database.\n" + ex);
            }
        }

        /// <summary>
        /// Checks if the user exists in the database.
        /// </summary> 
        public async Task<bool> HaveCredentialsMatchedAsync()
        {
            string toCheckField = string.IsNullOrEmpty(Email) ? "PHONE_NUMBER_IN_HASH" : "EMAIL";
            string toCheckValue = PhoneNumberInHash ?? Email;

            var cmdText = $@"SELECT COUNT(*) FROM USERS WHERE {toCheckField} = '{toCheckValue}' AND PASSWORD = '{PasswordInHash}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection); 

            var records = await Task.Run(() => command.ExecuteNonQuery());
            if (records > 0) // there is a record
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all information of the user from the database given the credentials.
        /// </summary>
        public async void RetrieveGeneralInfoAsync()
        {
            var cmdText = $@"SELECT USER_ID, USER_TYPE, FIRST_NAME, LAST_NAME, PHONE_NUMBER_IN_HASH, EMAIL, PASSWORD_IN_HASH, STATUS, PICTURE, LAST_PIC_CHANGE, LAST_ONLINE FROM USERS WHERE EMAIL = '{Email}' OR PHONE_NUMBER_IN_HASH = '{PhoneNumberInHash}'";

            await Task.Run(() => ReadData(cmdText, DbConnection));
        }

        /// <summary>
        /// Gets all information of the user from the database using the Id.
        /// </summary>
        public async void RetrieveGeneralInfoAsync(int userId, IDbConnection connection)
        {
            var cmdText = $@"SELECT USER_ID, USER_TYPE, FIRST_NAME, LAST_NAME, PHONE_NUMBER_IN_HASH, EMAIL, PASSWORD_IN_HASH, STATUS, PICTURE, LAST_PIC_CHANGE, LAST_ONLINE FROM USERS WHERE USER_ID = {userId}";

            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Sets the activity status of the user.
        /// </summary>
        private async void SetStatusAsync(Status status)
        {
            var cmdText = $"UPDATE USERS SET STATUS = '{status}' WHERE EMAIL = '{Email}' OR PHONE_NUMBER_IN_HASH = '{PhoneNumberInHash}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);
            await Task.Run(() => command.ExecuteNonQuery());
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                Type = reader.IsDBNull(1) ? UserType.Parent : (UserType)Enum.Parse(typeof(UserType), reader.GetString(1));
                FirstName = reader.IsDBNull(2) ? null : reader.GetString(2);
                LastName = reader.IsDBNull(3) ? null : reader.GetString(3);
                PhoneNumberInHash = reader.IsDBNull(4) ? null : reader.GetString(4);
                Email = reader.IsDBNull(5) ? null : reader.GetString(5);
                PasswordInHash = reader.IsDBNull(6) ? null : reader.GetString(6);
                Status = reader.IsDBNull(7) ? Status.Invisible : (Status)Enum.Parse(typeof(Status), reader.GetString(7));
                LastPicChange = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetOracleDate(9).Value;
                LastOnline = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetOracleDate(10).Value;

                byte[] image = reader.IsDBNull(8) ? null : (byte[])reader["PICTURE"];
                if (image == null)
                {
                    Picture = Properties.Resources.default_profile;
                }
                else
                {
                    MemoryStream ms = new(image);
                    Picture = Image.FromStream(ms);
                }
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
