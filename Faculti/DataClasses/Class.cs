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
using System.Windows;

namespace Faculti.DataClasses
{
    public class Class : DatabaseWorker, INotifyPropertyChanged
    {
        public Class()
        {
            _picture = Properties.Resources.default_classbg;
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _gradeLevel;
        public int GradeLevel
        {
            get { return _gradeLevel; }
            set
            {
                _gradeLevel = value;
                OnPropertyChanged("GradeLevel");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
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

        private Teacher _teacher;
        public Teacher Teacher
        {
            get { return _teacher; }
            set { _teacher = value; }
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

        /// <summary>
        /// Gets the general information of the class.
        /// </summary>
        public void RetrieveTeacherInfo(IDbConnection connection)
        {
            Teacher.RetrieveGeneralInfoAsync(Teacher.Id, connection);
        }

        /// <summary>
        /// Poupulates the class information using the class Id.
        /// </summary>
        public async void RetrieveInfoAsync(int classId, IDbConnection connection)
        {
            var cmdText = $@"SELECT CLASS_ID, GRADE_LEVEL, NAME, DESCRIPTION, TEACHER_ID, PICTURE FROM CLASSES WHERE CLASS_ID = {classId}";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Poupulates the class information with the specified Id of the class.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT CLASS_ID, GRADE_LEVEL, NAME, DESCRIPTION, TEACHER_ID, PICTURE FROM CLASSES WHERE CLASS_ID = {Id}";
            
            await Task.Run(() => ReadData(cmdText, connection));
        }

        public async Task AddToDatabaseAsync(IDbConnection connection, int teacherId)
        {
            byte[] blob = (byte[])(new ImageConverter()).ConvertTo(Picture, typeof(byte[]));

            var cmdText = $@"INSERT INTO CLASSES (NAME, DESCRIPTION, PICTURE, TEACHER_ID) VALUES ('{Name}', '{Description}', :image_blob, {teacherId})";
            OracleCommand command = (OracleCommand)Database.CreateCommand(cmdText, connection);
            command.Parameters.Add("image_blob", OracleDbType.Blob, blob, ParameterDirection.Input);

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
            Teacher = new();
            using IDbCommand command = Database.CreateCommand(cmdText, connection);
            using OracleDataReader reader = (OracleDataReader)command.ExecuteReader();

            if (reader.Read())
            {
                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                GradeLevel = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                Teacher.Id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);

                byte[] image = reader.IsDBNull(5) ? null : (byte[])reader["PICTURE"];
                if (image == null)
                {
                    Picture = Properties.Resources.default_classbg;
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
