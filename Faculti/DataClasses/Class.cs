using Faculti.DataRepo.DatabaseManager;
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
    public class Class : DatabaseWorker, INotifyPropertyChanged
    {
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
            var cmdText = $@"SELECT CLASS_ID, GRADE_LEVEL, NAME, DESCRIPTION, TEACHER_ID FROM CLASSES WHERE CLASS_ID = {classId}";
            await Task.Run(() => ReadData(cmdText, connection));
        }

        /// <summary>
        /// Poupulates the class information with the specified Id of the class.
        /// </summary>
        public async void RetrieveInfoAsync(IDbConnection connection)
        {
            var cmdText = $@"SELECT CLASS_ID, GRADE_LEVEL, NAME, DESCRIPTION, TEACHER_ID FROM CLASSES WHERE CLASS_ID = {Id}";
            
            await Task.Run(() => ReadData(cmdText, connection));
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
                Name = reader.IsDBNull(2) ? null : reader.GetString(2);
                Description = reader.IsDBNull(3) ? null : reader.GetString(3);
                Teacher.Id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
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
