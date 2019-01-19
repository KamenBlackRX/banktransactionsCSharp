using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Banktransactions
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
    public sealed class BankDAO
    {
        private static BankDAO _instance;
        private AppSettings appSettings;
        private IOptions<AppSettings> settings;

        private BankDAO()
        {
            // Read config file from appjson string
            appSettings = settings.Value;
            // Create Connection layer object

        }
        public static BankDAO getInstance()
        {

            if (_instance == null)
            {
                lock (typeof(BankDAO))
                {
                    if (_instance == null)
                    {
                        _instance = new BankDAO();
                    }
                }
            }

            return _instance;

        }

        /// <summary>
        /// Execute a string query and get his return.
        /// </summary>
        /// <param name="Query">String to query</param>
        /// <returns>List of contain the result from query</returns>
        public IList<string> ExecuteQuery(string Query)
        {
            // Get connection from CDO
            // Execute query
            // Return List
            IList<string> list = null;
            return list;
        }

        public void AddRegistry<T>(T query, EQueryType queryType)
        {
            
            if(query.GetType() == typeof(IList<string>))
            {
                // Build the query
                IList<T> list = null;
            }
            else if(query.GetType() == typeof(string))
            {
                // Execute if query is single
            }
        }
    }
}
