using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Banktransactions.Repositorio
{
    public enum EQueryType : uint
    {
        QT_SELECT ,
        QT_UPDATE ,
        QT_DELETE ,
        QT_ADD
    }
    /// <summary>
    /// This is the Singleton class for DAO.
    /// </summary>
    public sealed class Repositorio
    {
        private static Repositorio _instance;
        private AppSettings appSettings;
        private IOptions<AppSettings> settings;

        private MySqlConnection sqlConnection { get; set; }
        private MySqlCommand cmd { get; set; }
        private MySqlParameter param { get; set; }

        private Repositorio()
        {
            // Read config file from app json string
            appSettings = settings.Value;
            // Create Connection layer object

        }
        public static Repositorio getInstance()
        {

            if (_instance == null)
            {
                lock (typeof(Repositorio))
                {
                    if (_instance == null)
                    {
                        _instance = new Repositorio();
                        _instance.sqlConnection = _instance.GetConnection();
                    }
                }
            }

            return _instance;

        }

        /// <summary>
        /// Enable Connection by getting his instance.
        /// </summary>
        /// <returns></returns>
        private MySqlConnection GetConnection()
        {
            if ( sqlConnection.State == (ConnectionState.Closed | ConnectionState.Broken))
            {
                cmd = sqlConnection.CreateCommand();
                param = cmd.CreateParameter();
                return new MySqlConnection(appSettings.DBUri);
            }
            else
            {
                return sqlConnection;
            }
        }

        private void AddParameters(Dictionary<string, object> Parameters)
        {
            if(sqlConnection == null && Parameters == null){
                return;
            }
            else {
                foreach (var item in Parameters)
                {
                    param.ParameterName = item.Key;
                    param.Value = item.Value ?? DBNull.Value;
                    cmd.Parameters.Add(param);                    
                }
            }
        }

        /// <summary>
        /// Execute a string query and get his return.
        /// </summary>
        /// <param name="Query">String to query</param>
        /// <param name="Parameters"> The dictionary to input parameters </param>
        /// <returns>List of contain the result from query</returns>
        public IList<string> ExecuteQuery(string Query, Dictionary<string, object> Parameters = null)
        {
            // Get connection from CDO
            // Execute query
            // Return List
           
            AddParameters(Parameters);

            IList<string> list = null;
            return list;
        }
        /// <summary>
        /// Add anykind of supported incoming registry
        /// </summary>
        /// <typeparam name="T">Type of object to add</typeparam>
        /// <param name="query">The query for register.</param>
        /// <param name="queryType">The type of register.</param>
        public void AddRegistry<T>(T query, EQueryType queryType)
        {
            if(query.GetType() == typeof(IList<string>))
            {
                // Build the query
                                
            }
            else if(query.GetType() == typeof(string))
            {
                // Execute if query is single
            }
        }
    }
}
