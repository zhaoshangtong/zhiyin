using Rays.Utility.DataTables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Rays.DAL
{
    public class DataHelper
    {
        string strConn = null;

        //连接字符串 
        public DataHelper(string connString = "MSSQL")
        {
            this.strConn = ConfigurationManager.ConnectionStrings[connString].ConnectionString;
        }

        #region 执行查询，返回DataTable对象  

        public DataTable GetDataTable(string strSQL)
        {
            return GetDataTable(strSQL, pas: null);
        }

        public DataTable GetDataTable(string strSQL, SqlParameter[] pas)
        {
            return GetDataTable(strSQL, pas, CommandType.Text);
        }

        public DataTable GetDataTable(string strSQL, object sqlParams)
        {
            return GetDataTable(strSQL, GetSqlParameter(sqlParams), CommandType.Text);
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        private SqlParameter[] GetSqlParameter(object sqlParams)
        {
            if (sqlParams == null) { return null; }
            List<SqlParameter> list = new List<SqlParameter>();
            if (sqlParams is System.Dynamic.ExpandoObject)
            {
                ((IDictionary<String, Object>)sqlParams).ToList().ForEach(x => list.Add(new SqlParameter(x.Key, x.Value)));
            }
            else
            {
                sqlParams.GetType().GetProperties().ToList().ForEach(x => list.Add(new SqlParameter(x.Name, x.GetValue(sqlParams, null))));
            }
            return list.ToArray();
        }

        /// <summary>  
        /// 执行查询，返回DataTable对象  
        /// </summary>  
        /// <param name="strSQL">sql语句</param>  
        /// <param name="pas">参数数组</param>  
        /// <param name="cmdtype">Command类型</param>  
        /// <returns>DataTable对象</returns>  
        public DataTable GetDataTable(string strSQL, SqlParameter[] pas, CommandType cmdtype)
        {
            DataTable dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (pas != null && pas.Any())
                {
                    da.SelectCommand.Parameters.AddRange(pas);
                }
                da.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// 分页获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetPagingTable(string sql, int pageIndex, int pageSize, string order)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from( select ROW_NUMBER() over(order  by " + order + ") as rowNumber,* from (");
            sb.Append("select * from (" + sql + ") as alawliet");
            sb.Append(" ) as t ) as t2");
            sb.Append(" where rowNumber between " + (((pageIndex - 1) * pageSize) + 1) + " and " + pageIndex * pageSize
                    + " ");
            sb.Append(" order by " + order);
            var dt = GetDataTable(sb.ToString());
            dt.Columns.Remove("rowNumber");
            return dt;
        }
        /// <summary>
        /// 获取条数和分页datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetPagingTableAndCount(string sql, int pageIndex, int pageSize, string order, out int count)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from( select ROW_NUMBER() over(order  by " + order + ") as rowNumber,* from (");
            sb.Append("select * from (" + sql + ") as alawliet");
            sb.Append(" ) as t ) as t2");
            sb.Append(" where rowNumber between " + (((pageIndex - 1) * pageSize) + 1) + " and " + pageIndex * pageSize
                    + " ");
            sb.Append(" order by " + order);
            var dt = GetDataTable(sb.ToString());
            dt.Columns.Remove("rowNumber");
            count = ExcuteScalarSQL($"select count(1) from ({sql}) t1");
            return dt;
        }

        internal async void ExecuteTransactionScope(Func<Task<object>> p)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                await p().ContinueWith((item) =>
                {
                    tran.Complete();
                }, TaskContinuationOptions.AttachedToParent);
            }
        }

        public DataTable GetPagingTable(string sql, int pageIndex, int pageSize, string order, out int total, object sqlParams = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from( select ROW_NUMBER() over(order  by " + order + ") as rowNumber,* from (");
            sb.Append("select * from (" + sql + ") as alawliet");
            sb.Append(" ) as t ) as t2");
            sb.Append(" where rowNumber between " + (((pageIndex - 1) * pageSize) + 1) + " and " + pageIndex * pageSize
                    + " ");
            sb.Append(" order by " + order);

            var dtTask = Task.Run(() =>
            {
                var dt = GetDataTable(sb.ToString(), GetSqlParameter(sqlParams));
                dt.Columns.Remove("rowNumber");
                return dt;
            });

            var totalTask = Task.Run(() =>
            {
                return GetObject($"select count(1) from ({sql}) t1", GetSqlParameter(sqlParams)).To<int>(0);
            });

            Task.WaitAll(dtTask, totalTask);

            total = totalTask.Result;
            return dtTask.Result;
        }

        public DataTable GetPagingTable(string sql, string order, int beginPoint, int pageSize, out int total, object sqlParams = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from( select ROW_NUMBER() over(order  by " + order + ") as rowNumber,* from (");
            sb.Append("select * from (" + sql + ") as alawliet");
            sb.Append(" ) as t ) as t2");
            sb.Append(" where rowNumber between " + (beginPoint) + " and " + (beginPoint + pageSize - 1)
                    + " ");
            sb.Append(" order by " + order);

            var dtTask = Task.Run(() =>
            {
                var dt = GetDataTable(sb.ToString(), GetSqlParameter(sqlParams));
                dt.Columns.Remove("rowNumber");
                return dt;
            });

            var totalTask = Task.Run(() =>
            {
                return GetObject($"select count(1) from ({sql}) t1", GetSqlParameter(sqlParams)).To<int>(0);
            });

            Task.WaitAll(dtTask, totalTask);

            total = totalTask.Result;
            return dtTask.Result;
        }
        #endregion

        #region 执行查询，返回DataSet对象 

        public DataSet GetDataSet(string strSQL)
        {
            return GetDataSet(strSQL, null);
        }

        public DataSet GetDataSet(string strSQL, SqlParameter[] pas)
        {
            return GetDataSet(strSQL, pas, CommandType.Text);
        }

        public DataSet GetDataSet(string strSQL, object sqlParams)
        {
            return GetDataSet(strSQL, GetSqlParameter(sqlParams), CommandType.Text);
        }

        /// <summary>  
        /// 执行查询，返回DataSet对象  
        /// </summary>  
        /// <param name="strSQL">sql语句</param>  
        /// <param name="pas">参数数组</param>  
        /// <param name="cmdtype">Command类型</param>  
        /// <returns>DataSet对象</returns>  
        public DataSet GetDataSet(string strSQL, SqlParameter[] pas, CommandType cmdtype)
        {
            DataSet dt = new DataSet(); ;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = cmdtype;
                if (pas != null)
                {
                    da.SelectCommand.Parameters.AddRange(pas);
                }
                da.Fill(dt);
            }
            return dt;
        }
        #endregion

        #region 执行非查询，存储过程和SQL语句

        public int ExcuteProc(string ProcName)
        {
            return ExcuteSQL(ProcName, null, CommandType.StoredProcedure);
        }

        public int ExcuteProc(string ProcName, SqlParameter[] pars)
        {
            return ExcuteSQL(ProcName, pars, CommandType.StoredProcedure);
        }

        public int ExcuteSQL(string strSQL)
        {
            int count = ExcuteSQL(strSQL, null);
            return count;
        }

        public int ExcuteSQL(string strSQL, SqlParameter[] paras)
        {
            return ExcuteSQL(strSQL, paras, CommandType.Text);
        }

        public int ExcuteSQL(string strSQL, object sqlParams)
        {
            return ExcuteSQL(strSQL, GetSqlParameter(sqlParams));
        }

        /// <summary>  
        /// 执行非查询存储过程和SQL语句  
        /// 增、删、改  
        /// </summary>  
        /// <param name="strSQL">要执行的SQL语句</param>  
        /// <param name="paras">参数列表，没有参数填入null</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <returns>返回影响行数</returns>  
        public int ExcuteSQL(string strSQL, SqlParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    cmd.CommandType = cmdType;
                    if (paras != null)
                    {
                        cmd.Parameters.AddRange(paras);
                    }
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
            }
            return i;

        }
        #endregion

        #region 执行查询，返回第一行，第一列
        public int ExcuteScalarSQL(string strSQL)
        {
            return ExcuteScalarSQL(strSQL, null);
        }

        public int ExcuteScalarSQL(string strSQL, SqlParameter[] paras)
        {
            return ExcuteScalarSQL(strSQL, paras, CommandType.Text);
        }
        public int ExcuteScalarProc(string strSQL, SqlParameter[] paras)
        {
            return ExcuteScalarSQL(strSQL, paras, CommandType.StoredProcedure);
        }
        /// <summary>  
        /// 执行SQL语句，返回第一行，第一列  
        /// </summary>  
        /// <param name="strSQL">要执行的SQL语句</param>  
        /// <param name="paras">参数列表，没有参数填入null</param>  
        /// <param name="cmdType"></param>  
        /// <returns>返回影响行数</returns>  
        public int ExcuteScalarSQL(string strSQL, SqlParameter[] paras, CommandType cmdType)
        {
            int i = 0;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = cmdType;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);
                }
                conn.Open();
                object obj = cmd.ExecuteScalar();
                if (Equals(obj, null) || Equals(obj, DBNull.Value) || Equals(obj, string.Empty))
                {
                    i = 0;
                }
                else
                {
                    i = Convert.ToInt32(cmd.ExecuteScalar());
                }
                conn.Close();
            }
            return i;

        }


        #endregion

        #region 执行查询，返回最大值，检查是否存在
        /// <summary>
        /// 得到最大值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public int GetMaxID(string FieldName, string TableName)
        {
            string strSQL = "select max(" + FieldName + ") from " + TableName;
            int maxId = 0;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                object obj = cmd.ExecuteScalar();
                if (Equals(obj, null) || Equals(obj, DBNull.Value) || Equals(obj, string.Empty))
                {
                    maxId = 0;
                }
                else
                {
                    maxId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                conn.Close();
            }
            return maxId;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public bool Exists(string strSQL)
        {
            DataTable dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = CommandType.Text;
                conn.Open();
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否存在（基于MySqlParameter）
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public bool Exists(string strSQL, SqlParameter[] paras)
        {
            DataTable dt = new DataTable(); ;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.CommandType = CommandType.Text;
                if (paras != null)
                {
                    da.SelectCommand.Parameters.AddRange(paras);
                }
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 查询获取单个值
        /// <summary>  
        /// 调用不带参数的存储过程获取单个值  
        /// </summary>  
        /// <param name="ProcName"></param>  
        /// <returns></returns>  
        public object GetObjectByProc(string ProcName)
        {
            return GetObjectByProc(ProcName, null);
        }
        /// <summary>  
        /// 调用带参数的存储过程获取单个值  
        /// </summary>  
        /// <param name="ProcName"></param>  
        /// <param name="paras"></param>  
        /// <returns></returns>  
        public object GetObjectByProc(string ProcName, SqlParameter[] paras)
        {
            return GetObject(ProcName, paras, CommandType.StoredProcedure);
        }
        /// <summary>  
        /// 根据sql语句获取单个值  
        /// </summary>  
        /// <param name="strSQL"></param>  
        /// <returns></returns>  
        public object GetObject(string strSQL)
        {
            return GetObject(strSQL, null);
        }
        /// <summary>  
        /// 根据sql语句 和 参数数组获取单个值  
        /// </summary>  
        /// <param name="strSQL"></param>  
        /// <param name="paras"></param>  
        /// <returns></returns>  
        public object GetObject(string strSQL, SqlParameter[] paras)
        {
            return GetObject(strSQL, paras, CommandType.Text);
        }

        /// <summary>  
        /// 执行SQL语句，返回首行首列  
        /// </summary>  
        /// <param name="strSQL">要执行的SQL语句</param>  
        /// <param name="paras">参数列表，没有参数填入null</param>  
        /// <returns>返回的首行首列</returns>  
        public object GetObject(string strSQL, SqlParameter[] paras, CommandType cmdtype)
        {
            object obj = null;
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand cmd = new SqlCommand(strSQL, conn);
                cmd.CommandType = cmdtype;
                if (paras != null)
                {
                    cmd.Parameters.AddRange(paras);

                }

                conn.Open();
                obj = cmd.ExecuteScalar();
                if (Equals(obj, null) || Equals(obj, DBNull.Value))
                {
                    obj = null;
                }
                conn.Close();
            }
            return obj;

        }

        #endregion

        #region 查询获取DataReader
        /// <summary>  
        /// 调用不带参数的存储过程，返回DataReader对象  
        /// </summary>  
        /// <param name="procName">存储过程名称</param>  
        /// <returns>DataReader对象</returns>  
        public SqlDataReader GetReaderByProc(string procName)
        {
            return GetReaderByProc(procName, null);
        }
        /// <summary>  
        /// 调用带有参数的存储过程，返回DataReader对象  
        /// </summary>  
        /// <param name="procName">存储过程名</param>  
        /// <param name="paras">参数数组</param>  
        /// <returns>DataReader对象</returns>  
        public SqlDataReader GetReaderByProc(string procName, SqlParameter[] paras)
        {
            return GetReader(procName, paras, CommandType.StoredProcedure);
        }
        /// <summary>  
        /// 根据sql语句返回DataReader对象  
        /// </summary>  
        /// <param name="strSQL">sql语句</param>  
        /// <returns>DataReader对象</returns>  
        public SqlDataReader GetReader(string strSQL)
        {
            return GetReader(strSQL, null);
        }
        /// <summary>  
        /// 根据sql语句和参数返回DataReader对象  
        /// </summary>  
        /// <param name="strSQL">sql语句</param>  
        /// <param name="paras">参数数组</param>  
        /// <returns>DataReader对象</returns>  
        public SqlDataReader GetReader(string strSQL, SqlParameter[] paras)
        {
            return GetReader(strSQL, paras, CommandType.Text);
        }
        /// <summary>  
        /// 查询SQL语句获取DataReader  
        /// </summary>  
        /// <param name="strSQL">查询的SQL语句</param>  
        /// <param name="paras">参数列表，没有参数填入null</param>  
        /// <returns>查询到的DataReader（关闭该对象的时候，自动关闭连接）</returns>  
        public SqlDataReader GetReader(string strSQL, SqlParameter[] paras, CommandType cmdtype)
        {
            SqlDataReader sqldr = null;
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(strSQL, conn);
            cmd.CommandType = cmdtype;
            if (paras != null)
            {
                cmd.Parameters.AddRange(paras);
            }
            conn.Open();
            //CommandBehavior.CloseConnection的作用是如果关联的DataReader对象关闭，则连接自动关闭  
            sqldr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return sqldr;
        }
        #endregion

        #region 批量插入数据

        /// <summary>  
        /// 往数据库中批量插入数据  
        /// </summary>  
        /// <param name="sourceDt">数据源表</param>  
        /// <param name="targetTable">服务器上目标表</param>  
        public void BulkToDB(DataTable sourceDt, string targetTable)
        {
            SqlConnection conn = new SqlConnection(strConn);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);   //用其它源的数据有效批量加载sql server表中  
            bulkCopy.DestinationTableName = targetTable;    //服务器上目标表的名称  
            bulkCopy.BatchSize = sourceDt.Rows.Count;   //每一批次中的行数  

            try
            {
                conn.Open();
                if (sourceDt != null && sourceDt.Rows.Count != 0)
                    bulkCopy.WriteToServer(sourceDt);   //将提供的数据源中的所有行复制到目标表中  
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }

        }
        #endregion

        #region 执行事务
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>       
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlTran(params string[] SQLStringList)
        {
            if (SQLStringList != null && SQLStringList.Any())
            {
                return ExecuteSqlTran(SQLStringList.ToList());
            }
            return 0;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>       
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlTran(List<string> SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        //循环  
                        foreach (DictionaryEntry myDE in SQLStringList)//循环哈希表（本例中 即，循环执行添加在哈希表中的sql语句  
                        {
                            string cmdText = myDE.Key.ToString();//获取键值（本例中 即，sql语句）  
                            SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;//获取键值（本例中 即，sql语句对应的参数）  
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms); //调用PrepareCommand()函数，添加参数  
                            int val = cmd.ExecuteNonQuery();//调用增删改函数ExcuteNoQuery()，执行哈希表中添加的sql语句  
                            cmd.Parameters.Clear(); //清除参数  
                        }
                        trans.Commit();//提交事务  
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        //添加参数  
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)//如果数据库连接为关闭状态  
                conn.Open();//打开数据库连接  
            cmd.Connection = conn;//设置命令连接  
            cmd.CommandText = cmdText;//设置执行命令的sql语句  
            if (trans != null)//如果事务不为空  
                cmd.Transaction = trans;//设置执行命令的事务  
            cmd.CommandType = CommandType.Text;//设置解释sql语句的类型为“文本”类型（也是就说该函数不适用于存储过程）  
            if (cmdParms != null)//如果参数数组不为空  
            {
                foreach (SqlParameter parameter in cmdParms) //循环传入的参数数组  
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value; //获取参数的值  
                    }
                    cmd.Parameters.Add(parameter);//添加参数  
                }
            }
        }
        #endregion

        #region Insert/Update 参考于 http://blog.csdn.net/xpoer/article/details/26287739

        /// <summary>
        /// 默认忽略插入主键id值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="obj"></param>
        /// <returns>数据库变动数</returns>
        public int Insert(string tableName, object obj)
        {
            return Insert(tableName, obj, new string[] { "id" });
        }

        public int Insert(string tableName, object obj, string[] ignoreProperties)
        {
            var str = "@";
            var objType = obj.GetType();
            var data = (ignoreProperties == null && ignoreProperties.Any())
                                    ? objType.GetProperties().ToDictionary(p => p.Name, p => p.GetValue(obj, null))
                                    : objType.GetProperties().Where(x => !ignoreProperties.Contains(x.Name)).ToDictionary(p => p.Name, p => p.GetValue(obj, null));

            var sql = string.Format("INSERT INTO {0}({2}) VALUES ({1}{3})",
                tableName, str, string.Join(",", data.Keys), string.Join("," + str, data.Keys));
            using (SqlConnection _conn = new SqlConnection(strConn))
            {
                using (var cmd = _conn.CreateCommand())
                {
                    foreach (string key in data.Keys)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = key;
                        p.Value = data[key] ?? DBNull.Value;
                        cmd.Parameters.Add(p);
                    }

                    if (_conn.State != ConnectionState.Open)//如果数据库连接为关闭状态  
                        _conn.Open();//打开数据库连接  
                    cmd.CommandText = sql;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 默认忽略更新主键id值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(string tableName, object values, object where)
        {
            return Update(tableName, values, where, new string[] { "id" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <param name="ignoreProperties">忽略更新值</param>
        /// <returns></returns>
        public int Update(string tableName, object values, object where, string[] ignoreProperties)
        {
            var str = "@";
            var fields = (ignoreProperties == null && ignoreProperties.Any())
                        ? values.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(values, null))
                        : values.GetType().GetProperties().Where(x => !ignoreProperties.Contains(x.Name)).ToDictionary(p => p.Name, p => p.GetValue(values, null));

            var sb = new StringBuilder();
            sb.AppendFormat("UPDATE {0} ", tableName);
            sb.AppendFormat("SET {0} ", string.Join(", ",
                fields.Keys.ToList().ConvertAll<string>(key => key + "=" + str + key).ToArray()));
            using (SqlConnection _conn = new SqlConnection(strConn))
            {
                using (var cmd = _conn.CreateCommand())
                {
                    foreach (string key in fields.Keys)
                    {
                        var p = cmd.CreateParameter();
                        p.ParameterName = key;
                        p.Value = fields[key] ?? DBNull.Value;
                        cmd.Parameters.Add(p);
                    }

                    if (where != null)
                    {
                        sb.Append("WHERE ");

                        if (where is String)
                        {
                            sb.Append(where.ToString());
                        }
                        else
                        {
                            var wheres = where.GetType().GetProperties()
                                .ToDictionary(p => p.Name, p => p.GetValue(where, null));

                            sb.Append(string.Join(", ",  // eg: field1=@_field1;  
                             wheres.Keys.ToList().ConvertAll<string>(key => key + "=" + str + "_" + key).ToArray()));

                            foreach (string key in wheres.Keys)
                            {
                                var p = cmd.CreateParameter();
                                p.ParameterName = "_" + key;
                                p.Value = wheres[key] ?? DBNull.Value;
                                cmd.Parameters.Add(p);
                            }

                        }
                    }

                    cmd.CommandText = sb.ToString();
                    if (_conn.State != ConnectionState.Open)//如果数据库连接为关闭状态  
                        _conn.Open();//打开数据库连接  
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region GetDataByRandom
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public T GetModelByRandom<T>(string tableName, string random) where T : class, new()
        {
            return GetDataTable($"select * from {tableName} where random=@random", new SqlParameter[] { new SqlParameter("random", random) }).ToList<T>().FirstOrDefault();
        }
        #endregion

        #region GetModelById
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetModelById<T>(string tableName, string id) where T : class, new()
        {
            return GetDataTable($"select * from {tableName} where id=@id", new SqlParameter[] { new SqlParameter("id", id) }).ToList<T>().FirstOrDefault();
        }
        #endregion

        #region GetModelByParams
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="sqlParams">匿名参数 eg: new { id=1, name="xxx"}</param>
        /// <returns></returns>
        public T GetModelByParams<T>(string tableName, object sqlParams) where T : class, new()
        {
            StringBuilder strSql = new StringBuilder($"select * from {tableName} where 1=1");
            var sqlParamList = GetSqlParameter(sqlParams);
            foreach (SqlParameter item in sqlParamList)
            {
                strSql.Append($" and {item.ParameterName}=@{item.ParameterName}");
            }
            return GetDataTable(strSql.ToString(), sqlParamList).ToList<T>().FirstOrDefault();
        }
        #endregion


        #region GetModelBySql
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetModelBySql<T>(string sql) where T : class, new()
        {
            return GetDataTable(sql)?.ToList<T>()?.FirstOrDefault();
        }
        #endregion


        
    }
}
