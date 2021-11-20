using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataClasses
{
    public class Parent : User
    {
        private Student _ownedStudent;
        public Student OwnedStudent
        {
            get 
            { 
                return _ownedStudent; 
            }
            set { _ownedStudent = value; }
        }

        /// <summary>
        /// Gets the full information of the parent's owned student.
        /// </summary>
        public void RetrieveOwnedStudentInfo()
        {
            OwnedStudent = new();
            OwnedStudent.RetrieveInfoAsync(Id, DbConnection);
        }
    }
}