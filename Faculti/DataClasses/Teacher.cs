using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataClasses
{
    public class Teacher : User
    {
        private List<Class> _classList;
        public List<Class> ClassList
        {
            get { return _classList; }
            set { _classList = value; }
        }

        /// <summary>
        /// Gets the list of classes for the teacher.
        /// </summary>
        public async void RetrieveClassListAsync()
        {
            ClassList = new();
            var cmdText = $@"SELECT CLASS_ID FROM CLASSES WHERE TEACHER_ID = {Id} ORDER BY CLASS_ID";
            using IDbCommand command = Database.CreateCommand(cmdText, DbConnection);

            await Task.Run(() =>
            {
                using IDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var classId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);

                    Class _class = new();
                    _class.RetrieveInfoAsync(classId, DbConnection);
                    ClassList.Add(_class);
                }
            });
        }
    }
}
