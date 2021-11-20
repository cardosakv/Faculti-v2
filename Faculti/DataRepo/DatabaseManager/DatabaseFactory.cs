using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faculti.DataRepo.DatabaseManager
{
    public sealed class DatabaseFactory
    {
        public static DatabaseFactorySectionHandler _sectionHandler = 
            (DatabaseFactorySectionHandler)ConfigurationManager.GetSection("DatabaseFactoryConfiguration");

        private DatabaseFactory() { }

        public static Database CreateDatabase()
        {
            // Verify a DatabaseFactoryConfiguration line exists in the App.config
            if (_sectionHandler.Name.Length == 0)
            {
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of App.config.");
            }

            try
            {
                // Find the class
                Type database = Type.GetType(_sectionHandler.Name);

                // Get it's constructor
                ConstructorInfo constructor = database.GetConstructor(new Type[] { });

                // Invoke it's constructor, which returns an instance.
                Database createdObject = (Database)constructor.Invoke(null);

                // Initialize the connection string property for the database.
                createdObject.connectionString = _sectionHandler.ConnectionString;

                // Pass back the instance as a Database
                return createdObject;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error instantiating database {_sectionHandler.Name}.\n {ex.Message}");
            }
        }
    }
}
