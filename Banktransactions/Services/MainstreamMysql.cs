using System;

public class Test
{
}

/*
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
/// <summary>
/// Collection of SQL drivers and support.
/// </summary>
namespace Mainstream.Driver
{
    public class MySQL_Driver : IDisposable
    {
        #region Variables
        /// <summary>
        /// Public string for connect string of mysql
        /// </summary>
        public string sconn { get; set; }

        /// <summary>
        /// Infromation String
        /// </summary>
        public bool isLoading { get; set; }

        ///<summary>
        ///Getting if is connected
        /// </summary>
        public bool bIsConnOpened { get; set; }

        /// <summary>
        /// A string to execute a query
        /// </summary>
        public string query { get; set; }

        //Preload byte
        byte[] _byte = null;

        #endregion
        #region Instances
        ///<summary>
        ///Instances for SQL Driver, TO-DO catalog all isntances
        ///</summary>
        //mysql conn
        MySqlConnection _conn = new MySqlConnection();
        //mysql cmd
        MySqlCommand _cmd = new MySqlCommand();
        //mysql data reader
        MySqlDataReader _dr;
        //mysql Data Adapter
        MySqlDataAdapter _da = new MySqlDataAdapter();
        //Temp Data set 
        DataSet _ds;
        //Parametes
        MySqlParameter p = new MySqlParameter();
        //String Array _s
        string[] _s;


        #endregion
        #region Connection
        public class Connection : MySQL_Driver
        {
            /// <summary>
            /// Open Connection for mysql
            /// The string for connection is append in App.config file,
            /// and should be like
            /// add name="MySQL" connectionString="SERVER=8.8.8.8; DATABASE=foo; UID=bar; PASSWORD=foobar" providerName="MySql.Data.MySqlClient"
            /// </summary>
            public bool mysql_connect_Open()
            {
                this.sconn = ConfigurationManager.ConnectionStrings["MySQL_string_conn"].ToString();
                //Status Bool debugger 
#if DEBUG
                Mainstream.NativeMethods.MessageBox(new IntPtr(0), bIsConnOpened.ToString(), "OK!", 0);
#endif


                if (!bIsConnOpened)
                {
                    //Getting info and returning true for success
                    try
                    {

                        _conn.ConnectionString = sconn;
                        _conn.Open();
                        Console.WriteLine("OPENED");
                        bIsConnOpened = true;
                        return true;

                    }
                    catch (MySqlException ex)
                    {

                        //When handling errors, you can your application's response based 
                        //on the error number.
                        //The two most common error numbers when connecting are as follows:
                        //0: Cannot connect to server.
                        //1045: Invalid user name and/or password.
                        switch (ex.Number)
                        {
                            case 0:
                                Mainstream.NativeMethods.MessageBox(new IntPtr(0), "Cannot connect to server", "Contact administrator", 0);
                                break;

                            case 1045:
                                Mainstream.NativeMethods.MessageBox(new IntPtr(0), "Invalid username/password", "please try again", 0);
                                break;
                        }
                    }
                }
                return false;
            }
            /// <summary>
            /// Closing Connection for MySQL Driver
            /// </summary>            
            public bool mysql_connect_Close()
            {
                if (_conn.State == ConnectionState.Open && _conn.State == ConnectionState.Executing && _conn.State == ConnectionState.Broken)
                {
                    _conn.Close();
#if DEBUG
                    Mainstream.NativeMethods.MessageBox(new IntPtr(0), Constants.Constants.Errors.ERR_MYSQL_CONN_IS_ALREADY_OPEN.ToString(), "CLOSED", 0);

#endif
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
        #region Statments
        public class Statments : Connection
        {
            /// <summary>
            /// Select and Retrive Results from a SQL querry
            /// </summary>
            /// <param name="cmd">Enum to choose kind of command type operation</param>
            /// <param name="parameters">Dictonary for paramters including</param>
            public void MySQL_ExecuteNonQuerySingleCommandWithParameters(Constants.Constants.ComandSelect cmd, Dictionary<string, string> parameters)
            {
                //Open Conn
                this.mysql_connect_Open();
                //Set Loading param
                this.isLoading = true;
                #region Propriedades
                //Setting Proprienties
                _cmd.CommandText = this.query;
                _cmd.Connection = _conn;
                _cmd.Parameters.Clear();
                #endregion
                #region Parameters
                foreach (var param in parameters)
                {
                    _cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                #endregion
                //Execute Command
                try
                {
                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        _cmd.ExecuteNonQuery();
                    }
                    if (cmd == Constants.Constants.ComandSelect.Insert)
                    {
                        _cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    //Close Command               
                    _cmd.Dispose();
                    //Close Conn
                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                }
            }
            /// <summary>
            /// Execute and Retrive Results from a SQL query
            /// </summary>
            /// <param name="cmd">query to be executed</param>
            public void MySQL_ExecuteNonQuerySingleCommand(Constants.Constants.ComandSelect cmd)
            {
                //Open Conn
                this.mysql_connect_Open();
                //Set Loading param
                this.isLoading = true;
                #region Propriedades
                //Setting Proprienties
                _cmd.CommandText = this.query;
                _cmd.Connection = _conn;
                _cmd.Parameters.Clear();
                #endregion
                //Execute Command
                try
                {
                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        _cmd.ExecuteNonQuery();
                    }
                    if (cmd == Constants.Constants.ComandSelect.Insert)
                    {
                        _cmd.ExecuteNonQuery();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    //Close Command               
                    _cmd.Dispose();
                    //Close Conn
                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                }
            }
            /// <summary>
            /// Run a query in mysql and return a string with results
            /// </summary>
            /// <param name="cmd">type of command</param>
            /// <returns>Return a string array with results</returns>
            public string[] MySQL_ExecuteSingleCommand(Constants.Constants.ComandSelect cmd)
            {
                //Set Loading param
                this.isLoading = true;

                //Strings
                this._cmd.CommandText = query;
                //Instancing Connection
                this._cmd.Connection = _conn;

                try
                {
                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        //Instancing DR
                        _dr = _cmd.ExecuteReader();

                        while (_dr.Read())
                        {
                            _s = new string[_dr.FieldCount];
                            for (int i = 0; i < _dr.FieldCount; i++)
                            {
                                _s[i] = _dr.GetString(i);
                            }
                        }
                    }
                }

                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {

                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                    //DataReader Check
                    this._dr.Close();
                }

                return _s;
            }
            /// <summary>
            /// Execute a single command, to get a array of data
            /// </summary>
            /// <param name="parameters">The query parameters for sql instruciton</param>
            /// <param name="cmd">Enumerator for Type of command</param>
            /// <returns>Returns a string array from Data Reader results</returns>
            public string[] MySQL_ExecuteSingleCommandWithParameters(Dictionary<string, string> parameters, Constants.Constants.ComandSelect cmd)
            {
                //Set Loading param
                this.isLoading = true;
                //Open Connection
                this.mysql_connect_Open();
                #region Propriedades
                //Setting Proprienties
                this._cmd.CommandText = this.query;
                this._cmd.Connection = _conn;
                this._cmd.Parameters.Clear();
                #endregion
                #region Parameters
                foreach (var param in parameters)
                {
                    _cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                #endregion
                //Run Cmd and create array with loop
                try
                {
                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        //Instancing DR
                        _dr = _cmd.ExecuteReader();
                        //Creating string for storage data
                        _s = new string[_dr.FieldCount];
                        //Loop from DataReader
                        while (_dr.Read())
                        {
                            for (int i = 0; i < _dr.FieldCount; i++)
                            {
                                _s[i] = _dr.GetString(i);
                            }
                        }
                    }
                }

                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
                finally
                {

                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                    //DataReader Check
                    this._dr.Close();
                }

                return _s;
            }
            /// <summary>
            /// Execute a query and store on a Dataset, no parametizaded.
            /// </summary>
            /// <param name="cmd">type of Command</param>
            /// <returns>Returns the dataset results of query</returns>
            public DataSet MySQL_DataSet(Constants.Constants.ComandSelect cmd)
            {
                //Open Conn
                this.mysql_connect_Open();
                //Set Loading param
                this.isLoading = true;
                //Instancing DataSet
                this._ds = new DataSet();
                //Try
                try
                {
                    //Strings
                    this._cmd.CommandText = query;
                    //Instancing Connection
                    this._cmd.Connection = _conn;

                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        this._da.SelectCommand = _cmd;           //Select CMD
                        //Try to fill data source, throw ex
                        try
                        {
                            this._da.Fill(_ds);
                        }
                        catch (MySqlException mysql_ex)
                        {
                            Mainstream.NativeMethods.MessageBox(new IntPtr(0), "The excption " + mysql_ex.ToString() + "has been triggred", "A error has been trigged ", 0);
                        }

                    }

                    if (cmd == Constants.Constants.ComandSelect.Update)
                    {
                        this._da.UpdateCommand = _cmd;

                        try
                        {
                            this._da.Update(_ds);
                        }
                        catch (MySqlException mysql_ex)
                        {
                            Mainstream.NativeMethods.MessageBox(new IntPtr(0), "The excption " + mysql_ex.ToString() + "has been triggred", "A error has been trigged ", 0);
                        }



                    }

                    if (cmd == Constants.Constants.ComandSelect.Insert)
                    {
                        this._da.InsertCommand = _cmd;
                    }

                    if (cmd == Constants.Constants.ComandSelect.Delete)
                    {
                        this._da.DeleteCommand = _cmd;
                    }

                    // this._da.Fill(_ds);                 //Fill

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    this.mysql_connect_Close();

                    this.isLoading = false;
                    this._da.Dispose();

                    //DeallocDataSet
                    this._DeallocTemporaryDataSet();
                }

                //Return DataSet
                return _ds;
            }
            /// <summary>
            /// Retrive a DataSet from a mysql query with command.
            /// </summary>
            /// <param name="_type_command">Type of command</param>
            /// <param name="parameters"> Parammeter for inserting into query</param>
            /// <returns>Returns a dataset populed table </returns>
            public DataSet MySQL_DataReader_With_Param(Constants.Constants.ComandSelect _type_command, Dictionary<string, string> parameters)
            {
                //Open Conn
                this.mysql_connect_Open();
                //Set Loading param
                this.isLoading = true;
                #region Propriedades
                //Setting Proprienties
                _cmd.CommandText = this.query;
                _cmd.Connection = _conn;
                _cmd.Parameters.Clear();
                #endregion
                #region Parameters
                foreach (var param in parameters)
                {
                    _cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                #endregion
                //Try
                try
                {

                    //Strings
                    this._cmd.CommandText = query;
                    //Instancing Connection
                    this._cmd.Connection = _conn;

                    if (_type_command == Constants.Constants.ComandSelect.Select)
                    {
                        this._da.SelectCommand = _cmd;           //Select CMD
                        this._da.Fill(_ds);
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Update)
                    {
                        this._da.UpdateCommand = _cmd;
                        this._da.Update(_ds);                 //Update
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Insert)
                    {
                        this._da.InsertCommand = _cmd;
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Delete)
                    {
                        this._da.DeleteCommand = _cmd;
                    }

                    // this._da.Fill(_ds);                 //Fill

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    this.mysql_connect_Close();

                    this.isLoading = false;
                    this._da.Dispose();

                    //DeallocDataSet
                    this._DeallocTemporaryDataSet();
                }

                //Return DataSet
                return _ds;
            }
            /// <summary>
            /// Method contaning mysql reader results
            /// </summary>
            /// <param name="_type_command">Command constant for querry</param>
            /// <returns>return a IDatareader</returns>
            public IDataReader MySQL_DataReader(Constants.Constants.ComandSelect _type_command)
            {
                //Set Loading True 
                this.isLoading = true;
                #region Propriedades
                //Setting Proprienties
                _cmd.CommandText = this.query;
                _cmd.Connection = _conn;
                _cmd.Parameters.Clear();
                #endregion
                //Execute Command
                try
                {
                    this.mysql_connect_Open();

                    if (_type_command == Constants.Constants.ComandSelect.Select)
                    {
                        _dr = _cmd.ExecuteReader();

                    }

                    if (_type_command == Constants.Constants.ComandSelect.Update)
                    {
                        _dr = _cmd.ExecuteReader();
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Insert)
                    {
                        _dr = _cmd.ExecuteReader();
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Delete)
                    {
                        _dr = _cmd.ExecuteReader();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                }
                //Return Temp MysqlDataReader
                return _dr;
            }

            ///
            ///Method for Cleaning DataSet            
            ///
            
        private void _DeallocTemporaryDataSet()
            {
                _ds.Dispose();
            }
            /// <summary>
            /// Mysql Command with parameters, including Insert, Update and Delet statements.
            /// </summary>
            /// <param name="cmd">Type of Command</param>
            /// <param name="parameters">Parameters to be added</param>
            /// <returns>Returns mysql cmd</returns>
            public void MySQL_AdvancedQueryCommand(Constants.Constants.ComandSelect cmd, Dictionary<string, string> parameters)
            {
                //Set Loading ON
                this.isLoading = true;
                //Open Conn
                this.mysql_connect_Open();
                #region Propriedades
                //Setting Proprienties
                _cmd.CommandText = this.query;
                _cmd.Connection = _conn;
                #endregion
                #region Parameters
                foreach (var param in parameters)
                {
                    _cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
                #endregion
                //Execute Command
                try
                {
                    //Select Statements
                    if (cmd == Constants.Constants.ComandSelect.Select)
                    {
                        _cmd.ExecuteNonQueryAsync();


                    }
                    //Insert Statmensts
                    if (cmd == Constants.Constants.ComandSelect.Insert)
                    {
                        _cmd.ExecuteNonQuery();
                    }
                }
                //Get Error
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    //Close Command               
                    _cmd.Dispose();
                    //Close Conn
                    this.mysql_connect_Close();
                    //Set False 
                    this.isLoading = false;
                }
                //Return MysqlCommand
                //return _cmd;
            }
            /// <summary>
            /// Retrive a image blob
            /// </summary>
            /// <param name="q">string for query</param>
            /// <returns>A image byte array</returns>
            public byte[] RetriveBlobImage(string q)
            {
                //Conn
                this.mysql_connect_Open();
                //Params
                this.query = q;
                this._cmd.Connection = _conn;
                this._cmd.CommandText = query;

                //Execute Reader
                _dr = _cmd.ExecuteReader();

                //Fill Buffer
                while (_dr.Read())
                {
                    _byte = new byte[16384];
                    _byte = (byte[])(_dr["image"]);
                    if (_byte == null)
                    {
                        Console.WriteLine("NO IMAGE");
                    }
                }
                //Clos Conn and dispose
                _dr.Close();
                this.mysql_connect_Close();

                //Return Value
                return _byte;
            }
            /// <summary>
            /// Retrive asynchronius a Dataset with Populate data
            /// </summary>
            /// <param name="_type_command"></param>
            /// <returns></returns>
            public async Task<DataSet> MySQL_DataReaderAsync(Constants.Constants.ComandSelect _type_command)
            {
                //Open Conn
                this.mysql_connect_Open();
                //Set Loading param
                this.isLoading = true;
                //Instancing DataSet
                this._ds = new DataSet();
                //Try
                try
                {
                    //Strings
                    this._cmd.CommandText = query;
                    //Instancing Connection
                    this._cmd.Connection = _conn;

                    if (_type_command == Constants.Constants.ComandSelect.Select)
                    {
                        this._da.SelectCommand = _cmd;           //Select CMD
                        //Try to fill data source, throw ex
                        try
                        {
                            this._da.Fill(_ds);
                        }
                        catch (MySqlException mysql_ex)
                        {
                            Mainstream.NativeMethods.MessageBox(new IntPtr(0), "The excption " + mysql_ex.ToString() + "has been triggred", "A error has been trigged ", 0);
                        }

                    }

                    if (_type_command == Constants.Constants.ComandSelect.Update)
                    {
                        this._da.UpdateCommand = _cmd;

                        try
                        {
                            this._da.Update(_ds);
                        }
                        catch (MySqlException mysql_ex)
                        {
                            Mainstream.NativeMethods.MessageBox(new IntPtr(0), "The excption " + mysql_ex.ToString() + "has been triggred", "A error has been trigged ", 0);
                        }

                    }

                    if (_type_command == Constants.Constants.ComandSelect.Insert)
                    {
                        this._da.InsertCommand = _cmd;
                    }

                    if (_type_command == Constants.Constants.ComandSelect.Delete)
                    {
                        this._da.DeleteCommand = _cmd;
                    }

                    // this._da.Fill(_ds);                 //Fill

                }
                catch (MySqlException ex)
                {
                    Mainstream.NativeMethods.MessageBox(new IntPtr(0), "Error", ex.ToString(), 0);
                }

                return _ds;
            }
        }

        #endregion
        #region Dispose
        //Calling Dispose Methods
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~MySQL_Driver()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (_conn != null) { _conn.Dispose(); _conn = null; }
                if (_cmd != null) { _cmd.Dispose(); _cmd = null; }
                if (_dr != null) { _dr.Dispose(); _dr = null; }
                if (_da != null) { _da.Dispose(); _da = null; }
                if (_ds != null) { _ds.Dispose(); _ds = null; }
                if (_byte != null) { _byte = null; }
                if (p != null) { p = null; }
            }
        }
    #endregion
    }
}
*/


