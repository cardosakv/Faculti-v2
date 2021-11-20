using Faculti.DataRepo.DatabaseManager;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataClasses
{
    public class Student : DatabaseWorker, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime LastPicChange { get; set; }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
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

        private int _age;
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        private string _sex;
        public string Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                OnPropertyChanged("Sex");
            }
        }

        private Class _class;
        public Class Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private Parent _parent;
        public Parent Parent
        {
            get { return _parent; }
            set { _parent = value; }
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

        /// <summary>
        /// Gets the full class information of the student.
        /// </summary>
        public void RetrieveClassInfo(IDbConnection connection)
        {
            Class.RetrieveInfoAsync(connection);
        }

        /// <summary>
        /// Gets the parent general information of the student.
        /// </summary>
        public void RetrieveParentInfo(IDbConnection connection)
        {
            Parent.RetrieveGeneralInfoAsync(Parent.Id, connection);
        }

        /// <summary>
        /// Gets student information using its student code.
        /// </summary>
        public async void RetrieveInfoAsync(string studentCode, IDbConnection connection)
        {
            var cmdText = $@"SELECT STUDENT_ID, STUDENT_CODE, FIRST_NAME, LAST_NAME, AGE, SEX, CLASS_ID, PARENT_ID, LAST_PIC_CHANGE, PICTURE FROM STUDENTS WHERE STUDENT_CODE = '{studentCode}'";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Gets student information using its parent ID.
        /// </summary>
        public async void RetrieveInfoAsync(int parentId, IDbConnection connection)
        {
            var cmdText = $@"SELECT STUDENT_ID, STUDENT_CODE, FIRST_NAME, LAST_NAME, AGE, SEX, CLASS_ID, PARENT_ID, LAST_PIC_CHANGE, PICTURE FROM STUDENTS WHERE PARENT_ID = {parentId}";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Gets student information using the student ID specified.
        /// Add a value to the Id field first before calling this function.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT STUDENT_ID, STUDENT_CODE, FIRST_NAME, LAST_NAME, AGE, SEX, CLASS_ID, PARENT_ID, LAST_PIC_CHANGE, PICTURE FROM STUDENTS WHERE STUDENT_ID = {Id}";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        #region Reading database data
        private void ReadData(string cmdText, IDbConnection connection)
        {
            Class = new();
            Parent = new();

            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                Code = reader.IsDBNull(1) ? null : reader.GetString(1);
                FirstName = reader.IsDBNull(2) ? null : reader.GetString(1);
                LastName = reader.IsDBNull(3) ? null : reader.GetString(3);
                Age = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                Sex = reader.IsDBNull(5) ? null : reader.GetString(5);
                Class.Id = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                Parent.Id = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                LastPicChange = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetOracleDate(8).Value;

                byte[] image = reader.IsDBNull(9) ? null : (byte[])reader["PICTURE"];
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
