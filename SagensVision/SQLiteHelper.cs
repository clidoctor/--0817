using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace SagensVision
{
    public class SQLiteHelper
    {
        //private static string _dbName = "";
        public static SQLiteConnection _SQLiteConn = null;//连接对象
        private static SQLiteTransaction _SQLiteTrans = null;//事务对象
        private static bool _IsRunTrans = false;//事务运行标识
        private static string _SQLiteConnString = null;//连接字符串
        private static bool _AtuoCommit = false;//事务自动提交标识
        private static object lockobj = new object();
        public static string dbPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + "glue.db3";
        static public string SQLiteConnString
        {
            set
            {
                string dbname = "Data Source=";
                _SQLiteConnString = dbname + value ;
            }
            get
            {
                return _SQLiteConnString;
            }
        }

        //public SQLiteHelper(string dbPath)
        //{
        //    _dbName = dbPath;
        //    _SQLiteConnString = "Data Source=" + dbPath;
        //}
        /// <summary>
        /// 新建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns></returns>
        static public string NewDbFile()
        {
            try
            {
                SQLiteConnString = dbPath;
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }
                return "OK";
            }
            catch (Exception ex)
            {

                return "NewDbFile:"+ex.Message;
            }
        }

        static public string GetTableName()
        {
            try
            {
                OpenDb();
                DataTable dt = _SQLiteConn.GetSchema("Tables", null);
                string  tabelnames = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tabelnames += dt.Rows[i][2].ToString()+ ",";
                }
                CloseDb();
                return tabelnames;
            }
            catch (Exception ex)
            {
                CloseDb();
                return ex.Message;
            }
        }
        static public string IsHaveTable(string tableName)
        {
            lock (lockobj)
            {
                try
                {
                    OpenDb();
                    DataTable dt = _SQLiteConn.GetSchema("Tables", null);
                    List<string> tabelnames = new List<string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tabelnames.Add(dt.Rows[i][2].ToString());
                    }
                    foreach (string item in tabelnames)
                    {
                        string namet1 = item.TrimStart('[');
                        string name2 = namet1.TrimEnd(']');

                        DateTime tableTime = DateTime.Parse(name2);
                        TimeSpan subTime = DateTime.Now - tableTime;
                        if (subTime.Days > 3)
                        {
                            string ok = SQLiteHelper.DeleteTable(name2);
                            //if (ok == "OK")
                            //{
                            //    return "NG :" + "过期文件删除失败！";
                            //}
                        }
                    }

                    string name1 = tableName.TrimStart('[');
                    string name = name1.TrimEnd(']');              
                    if (tabelnames.Contains(name))
                    {
                        CloseDb();
                        return "OK";
                    }
                    else
                    {
                        CloseDb();
                        return "NG";
                    }               
                   
                }
                catch (Exception ex)
                {
                    CloseDb();
                    return "NG " +ex.Message;
                }

            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="nameStart">列名</param>
        /// <returns></returns>
        static public string NewTable(string nameStart)
        {
            lock (lockobj)
            {
                string tableName = DateTime.Now.ToString("[yyyy/MM/dd]");
                SQLiteConnection sqliteConn = new SQLiteConnection("Data source=" + dbPath);
                try
                {
                    if (sqliteConn.State != ConnectionState.Open)
                    {
                        sqliteConn.Open();
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.Connection = sqliteConn;
                        //判断是否存在表
                        DataTable dt = sqliteConn.GetSchema("Tables", null);
                        //List<string> tabelnames = new List<string>();
  
                    

                        if (IsHaveTable(tableName) == "OK")
                        {            

                            return "已存在" + tableName;
                        }

                        string names = "";
                        if (nameStart == "")
                        {
                            names = "(Name1)";
                        }
                        else
                        {
                            names = " (" + nameStart + ");";
                        }
                        cmd.CommandText = "CREATE TABLE " + tableName + names;
                        cmd.ExecuteNonQuery();
                    }
                    sqliteConn.Close();
                    sqliteConn.Dispose();
                    return "OK";
                }
                catch (Exception ex)
                {
                    sqliteConn.Close();
                    sqliteConn.Dispose();
                    return "NewTable:" + ex.Message;
                }
            }
        }

        static public string DeleteTable(string tableName)
        {
            SQLiteConnection sqliteConn = new SQLiteConnection(_SQLiteConnString);
            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    //判断是否存在表
                    //DataTable dt = sqliteConn.GetSchema("Tables", null);
                    //List<string> tabelnames = new List<string>();
                    //if (IsHaveTable(tableName) == "OK")
                    //{
                        tableName = "[" + tableName + "]";
                        cmd.CommandText = "DROP TABLE " + tableName ;
                        cmd.ExecuteNonQuery();
                    //}
                }
                sqliteConn.Close();
                sqliteConn.Dispose();
                return "OK";
            }
            catch (Exception ex)
            {
                sqliteConn.Close();
                sqliteConn.Dispose();
                return "DeleteTable:" + ex.Message;
            }
        }

        /// <summary>
        /// 打开当前数据库的连接
        /// </summary>
        /// <returns></returns>
        static public string OpenDb()
        {
            try
            {
                _SQLiteConn = new SQLiteConnection(_SQLiteConnString);
                _SQLiteConn.Open();
                return "OK";
            }
            catch (Exception ex)
            {
                return "OpenDb:" + ex.Message;
                
            }
        }
        /// <summary>
        /// 打开指定数据库的连接
        /// </summary>
        /// <param name="dbPath">数据库路径</param>
        /// <returns></returns>
        static public string OpenDb(string Path)
        {
            try
            {
                if (Path=="")
                {
                   Path = "Data Source=" + dbPath;
                }
               
                _SQLiteConn = new SQLiteConnection(Path);
                //_dbName = dbPath;
                _SQLiteConn.Open();
                return "OK";
            }
            catch (Exception ex)
            {

                return "OpenDb"+ex.Message ;
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <returns></returns>
       static public string CloseDb()
        {
            try
            {
                if (_SQLiteConn !=null &&_SQLiteConn.State!=ConnectionState.Closed)
                {
                    if (_IsRunTrans &&_AtuoCommit)
                    {
                       SQLiteHelper.Commit();
                    }
                    _SQLiteConn.Close();
                    _SQLiteConn = null;
                }
                return "OK";
            }
            catch (Exception ex)
            {

                return "CloseDb:"+ex.Message;
            }
        }
        /// <summary>
        /// 提交当前挂起的事务
        /// </summary>
       static public void Commit()
        {
            if (_IsRunTrans)
            {
                _SQLiteTrans.Commit();
                _IsRunTrans = false;
            }
        }
        /// <summary>
        /// 开始数据库事务
        /// </summary>
       static public  void  BeginTransaction()
        {
            _SQLiteConn.BeginTransaction();
            _IsRunTrans = true;
        }

        /// <summary>
        /// 开始数据库事务
        /// </summary>
        /// <param name="isolevel">事务锁级别</param>
        public void BeginTransaction(IsolationLevel isolevel)
        {
           _SQLiteConn.BeginTransaction(isolevel);
           _IsRunTrans = true;
        }
        /// <summary>
        /// 对数据库进行增删改操作，返回受影响行数
        /// </summary>
        /// <param name="sql">要执行的增删改sql语句</param>
        /// <param name="paramters">执行增删改所需要的参数，以在sql语句的顺序为准</param>
        /// <returns></returns>
       static public int ExecuteNonQuery(string sql,params SQLiteParameter[] paramters)
        {           
                int affectedRows = 0;
                using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        try
                        {
                            connection.Open();
                            command.CommandText = sql;
                            if (paramters.Length !=0)
                            {
                                command.Parameters.AddRange(paramters);
                               
                            }
                            affectedRows = command.ExecuteNonQuery();
                        connection.Close();
                    }
                        catch (Exception ex)
                        {
                            connection.Close();
                            return -999;
                        }
                    }
                }
                return affectedRows;
        }

        /// <summary>
        /// 执行查询语句，并返回第一个结果。
        /// </summary>
        /// <param name="sql">查询语句。</param>
        /// <returns>查询结果。</returns>
        /// <exception cref="Exception"></exception>
        static public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_SQLiteConnString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。 
        /// </summary> 
        /// <param name="sql">要执行的查询语句。</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        static public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters.Length != 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    try { adapter.Fill(data); }
                    catch (Exception) { throw; }
                    return data;
                }
            }
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例。 
        /// </summary> 
        /// <param name="sql">要执行的查询语句。</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        static public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                if (parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception) { throw; }
        }

        /// <summary> 
        /// 查询数据库中的所有数据类型信息。
        /// </summary> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        static public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
            {
                try
                {
                    connection.Open();
                    return connection.GetSchema("TABLES");
                }
                catch (Exception) { throw; }
            }
        }

        static public int Delete(string tablename, string startname,object deletename)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
            {
                tablename = "[" + tablename + "]";
                string commandText = "delete from " + tablename + " where " + startname + "= " + deletename;
                using (SQLiteCommand cmd = new SQLiteCommand(commandText,connection))
                {
                    try
                    {
                        connection.Open();
                        int id = cmd.ExecuteNonQuery();
                        connection.Close();
                        return id;
                    }
                    catch (Exception) { throw; }
                }             
            }
        }
        //select * from tblName where rDate Between '2008-6-10' and  '2008-6-12'
        /*
        WHERE SALARY GLOB '200*'	查找以 200 开头的任意值
        WHERE SALARY GLOB '*200*'	查找任意位置包含 200 的任意值
        WHERE SALARY GLOB '?00*'	查找第二位和第三位为 00 的任意值
        WHERE SALARY GLOB '2??'	查找以 2 开头，且长度至少为 3 个字符的任意值
        WHERE SALARY GLOB '*2'	查找以 2 结尾的任意值
        WHERE SALARY GLOB '?2*3'	查找第二位为 2，且以 3 结尾的任意值
        WHERE SALARY GLOB '2???3'	查找长度为 5 位数，且以 2 开头以 3 结尾的任意值
        */
        static public DataTable Query(string tablename, string startname, string findname)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
                {
                    tablename = "[" + tablename + "]";
                    string commandText = "select * from " + tablename + " where " + startname + " GLOB " + "'*" + findname + "*'";
                    using (SQLiteCommand cmd = new SQLiteCommand(commandText, connection))
                    {
                        DataTable dt = new DataTable();
                        SQLiteDataAdapter slda = new SQLiteDataAdapter(commandText, connection);
                        DataSet ds = new DataSet();
                        slda.Fill(ds);
                        dt = ds.Tables[0];
                        return dt;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        static public DataTable QueryDate(string tablename,string startname, string startTime,string endTime)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(_SQLiteConnString))
                {
                    tablename = "[" + tablename + "]";
                    string commandText = "select * from " + tablename + " where " + "时间" + " Between " + "'" + startTime + "'" + " and " + "'" + endTime + "'";
                    using (SQLiteCommand cmd = new SQLiteCommand(commandText, connection))
                    {
                        DataTable dt = new DataTable();
                        SQLiteDataAdapter slda = new SQLiteDataAdapter(commandText, connection);
                        DataSet ds = new DataSet();
                        slda.Fill(ds);
                        dt = ds.Tables[0];
                        return dt;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
