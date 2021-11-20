using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataRepository.DatabaseManager
{
    public class DatabaseWorker
    {
        private static readonly Database _database;

        static DatabaseWorker()
        {
            try
            {
                _database = DatabaseFactory.CreateDatabase();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static Database Database
        {
            get { return _database; }
        }
    }
}
