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

        public User(UserType type, string? email, string? phoneNumberInHash, string password)
        {
            Type = type;
            _email = email;
            _phoneNumberInHash = phoneNumberInHash;
            _password = password;
        }

        public int Id { get; set; }

        public UserType Type { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set 
            { 
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _passwordInHash;
        public string PasswordInHash
        {
            get => _passwordInHash;
            set
            {
                _passwordInHash = value;
                _password = Security.Decrypt(value);
                OnPropertyChanged("Password");
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                _passwordInHash = Security.Encrypt(value);
                OnPropertyChanged("Password");
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
                OnPropertyChanged("FullName");
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set 
            { 
                _lastName = value;
                OnPropertyChanged("LastName");
                OnPropertyChanged("FullName");
            }
        }

        public string FullName
        {
            get => $"{_firstName} {_lastName}";
            set => OnPropertyChanged("FullName");
        }

        private string _phoneNumberInHash;
        public string PhoneNumberInHash
        {
            get => _phoneNumberInHash;
            set
            {
                _phoneNumberInHash = value;
                _phoneNumber = Security.Decrypt(value);
                OnPropertyChanged("PhoneNumber");
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set 
            {
                _phoneNumber = value;
                _phoneNumberInHash = Security.Encrypt(value);
                OnPropertyChanged("PhoneNumber"); 
            }
        }

        private Image _picture;
        public Image Picture
        {
            get => _picture;
            set 
            { 
                _picture = value; 
                OnPropertyChanged("Picture"); 
            }
        }

        private DateTime _lastOnline;
        public DateTime LastOnline
        {
            get => _lastOnline;
            set 
            { 
                _lastOnline = value;
                OnPropertyChanged("LastOnline");
            }
        }

        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                SetStatusAsync(value);
                OnPropertyChanged("Status");
            }
        }

        public DateTime LastPicChange { get; set; }

        private IDbConnection _dbConnection;
        public IDbConnection DbConnection
        {
            get => _dbConnection;
            set 
            {
                _dbConnection.Close();
                _dbConnection = value; 
            }
        }

        /// <summary>
        /// Opens only one database connection for the user.
        /// </summary>
        public async Task CreateConnectionAsync()
        {
            if (_dbConnection != null) _dbConnection.Close();

            try
            {
                _dbConnection = await Database.CreateOpenConnectionAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating connection to the database.\n" + ex);
            }
        }

        /// <summary>
        /// Updates the password of the user to the database.
        /// </summary>
        public async Task UpdatePassword()
        {
            var cmdText = $@"UPDATE USERS SET PASSWORD_IN_HASH = '{PasswordInHash}' WHERE EMAIL = '{Email}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);
            await Task.Run(() =>
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error.\n" + ex);
                }
            });
        }

        /// <summary>
        /// Adds the user to the database.
        /// </summary>
        public async Task AddToDatabaseAsync()
        {
            var cmdText = $@"INSERT INTO USERS (FIRST_NAME, LAST_NAME, EMAIL, PASSWORD_IN_HASH, PHONE_NUMBER_IN_HASH, USER_TYPE) VALUES ('{FirstName}', '{LastName}', '{Email}', '{PasswordInHash}', '{PhoneNumberInHash}', '{Type}')";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);
            await Task.Run(() =>
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error.\n" + ex);
                }
            });
        }

        /// <summary>
        /// Checks if the email of the user is already registered on the database.
        /// </summary>
        public async Task<bool> IsEmailOrNumberAvailableAsync()
        {
            string toCheckField = string.IsNullOrEmpty(Email) ? "PHONE_NUMBER_IN_HASH" : "EMAIL";
            string toCheckValue = PhoneNumberInHash ?? Email;

            var cmdText = $@"SELECT COUNT(*) FROM USERS WHERE {toCheckField} = '{toCheckValue}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);

            var records = 0;
            await Task.Run(async () =>
            {
                try
                {
                    records = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    await CreateConnectionAsync();
                    records = Convert.ToInt32(command.ExecuteScalar());
                }
            });

            return records > 0;
        }

        /// <summary>
        /// Checks if the user exists in the database.
        /// </summary> 
        public async Task<bool> HaveCredentialsMatchedAsync()
        {
            string toCheckField = string.IsNullOrEmpty(Email) ? "PHONE_NUMBER_IN_HASH" : "EMAIL";
            string toCheckValue = PhoneNumberInHash ?? Email;

            var cmdText = $@"SELECT COUNT(*) FROM USERS WHERE {toCheckField} = '{toCheckValue}' AND PASSWORD_IN_HASH = '{PasswordInHash}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);

            var records = 0;
            await Task.Run(() => 
            {
                try
                {
                    records = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error.\n" + ex);
                }
            });

            return records > 0;
        }

        /// <summary>
        /// Gets all information of the user from the database given the credentials.
        /// </summary>
        public async void RetrieveGeneralInfoAsync()
        {
            var cmdText = $@"SELECT USER_ID, USER_TYPE, FIRST_NAME, LAST_NAME, PHONE_NUMBER_IN_HASH, EMAIL, PASSWORD_IN_HASH, STATUS_TYPE, PICTURE, LAST_PIC_CHANGE, LAST_ONLINE FROM USERS WHERE EMAIL = '{Email}' OR PHONE_NUMBER_IN_HASH = '{PhoneNumberInHash}'";
            await Task.Run(() =>
            {
                try
                {
                    ReadData(cmdText, DbConnection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error.\n" + ex);
                }
            });
        }

        /// <summary>
        /// Gets all information of the user from the database using the Id.
        /// </summary>
        public async void RetrieveGeneralInfoAsync(int userId, IDbConnection connection)
        {
            var cmdText = $@"SELECT USER_ID, USER_TYPE, FIRST_NAME, LAST_NAME, PHONE_NUMBER_IN_HASH, EMAIL, PASSWORD_IN_HASH, STATUS_TYPE, PICTURE, LAST_PIC_CHANGE, LAST_ONLINE FROM USERS WHERE USER_ID = {userId}";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Sets the activity status of the user.
        /// </summary>
        private async void SetStatusAsync(Status status)
        {
            var cmdText = $"UPDATE USERS SET STATUS_TYPE = '{status}' WHERE EMAIL = '{Email}' OR PHONE_NUMBER_IN_HASH = '{PhoneNumberInHash}'";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);
            await Task.Run(() =>
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection error.\n" + ex);
                }
            });
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Please invoke CreateConnection() first.\n" + ex);
            }
        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}