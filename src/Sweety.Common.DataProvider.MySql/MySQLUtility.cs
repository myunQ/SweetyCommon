/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  MySQL 数据库的常用操作方法。
 *
 *  MySQL 官方文档：https://dev.mysql.com/doc/
 *  
 *  MySQL.Date 官方文档：https://dev.mysql.com/doc/connector-net/en/preface.html
 * 
 * Members Index:
 *      class MySQLUtility
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.DataProvider.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    using global::MySql.Data.MySqlClient;


    /// <summary>
    /// <c>MySql</c> 数据库的常用操作工具类库。
    /// </summary>
    public class MySQLUtility : RelationalDBUtilityBase
    {
        /// <summary>
        /// 构造函数，创建 <c>MySql</c> 数据库操作对象实例。
        /// </summary>
        public MySQLUtility()
            : base()
        { }



        /// <summary>
        /// 创建一个 <c>MySql</c> 数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlConnection"/> 对象实例。</returns>
        public MySqlConnection BuildSqlConnection()
        {

            if (TargetRole == DatabaseServerRole.Master)
            {
                return new MySqlConnection(__masterConnStr[_masterConnStrIndex]);
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
                return new MySqlConnection(__slaveConnStr[_slaveConnStrIndex]);
            }
            else
            {
                return new MySqlConnection(__allConnStr[_allConnStrIndex]);
            }
        }

        /// <summary>
        /// 使用指定的 <c>MySql</c> 数据库连接字符串创建连接对象实例。
        /// </summary>
        /// <param name="connectionString"><c>MySql</c> 数据库连接字符。</param>
        /// <returns>返回一个 <see cref="MySqlConnection"/> 对象实例。</returns>
        /// <exception cref="ArgumentNullException">参数 <paramref name="connectionString"/> 的值为 <c>null</c>。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="connectionString"/> 的值为空白字符。</exception>
        public MySqlConnection BuildSqlConnection(string connectionString)
        {
            if (null == connectionString) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_string_cannot_be_null_or_empty, nameof(connectionString));

            return new MySqlConnection(connectionString);
        }



        /// <summary>
        /// 使用默认数据库连接对象创建一个可在 <c>MySql</c> 数据库执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlCommand"/> 对象实例。</returns>
        public MySqlCommand BuildSqlCommand()
        {
            return new MySqlCommand(null, BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的 <c>MySql</c> 数据库连接对象创建一个执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <param name="conn">数据库连接对象实例。</param>
        /// <returns>返回一个 <see cref="MySqlCommand"/> 对象实例。</returns>
        public MySqlCommand BuildSqlCommand(MySqlConnection conn)
        {
            return new MySqlCommand(null, conn);
        }



        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns><c>MySql</c> 数据库事务对象实例。</returns>
        public MySqlTransaction BuildSqlTransaction()
        {
            return BuildSqlConnection().BeginTransaction();
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns><c>MySql</c> 数据库事务对象实例。</returns>
        public MySqlTransaction BuildSqlTransaction(IsolationLevel level)
        {
            return BuildSqlConnection().BeginTransaction(level);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>MySql</c> 数据库事务对象实例。</returns>
        public Task<MySqlTransaction> BuildSqlTransactionAsync(CancellationToken cancellationToken = default)
        {
            return BuildSqlConnection().BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>MySql</c> 数据库事务对象实例。</returns>
        public Task<MySqlTransaction> BuildSqlTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            return BuildSqlConnection().BeginTransactionAsync(level, cancellationToken);
        }
#endif //!NETSTANDARD2_0



        /// <summary>
        /// 创建一个 <see cref="MySqlParameter"/> 对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlParameter"/> 对象实例。</returns>
        public MySqlParameter BuildSqlParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个 <see cref="MySqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即 <see cref="MySqlParameter.ParameterName"/> 属性的值。</param>
        /// <param name="value">参数值，即 <see cref="MySqlParameter.Value"/> 属性的值。</param>
        /// <returns>返回一个 <see cref="MySqlParameter"/> 对象实例。</returns>
        public MySqlParameter BuildSqlParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }



        #region Override base class RelationalDBUtilityBase
        /// <summary>
        /// 安全处理程序
        /// </summary>
        /// <param name="value">预使用字符串拼接的值</param>
        /// <returns>处理后的字符串值</returns>
        public override string SafeHandler(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return value.Replace("'", "''");
            }

            return value;
        }



        /// <summary>
        /// 将通配符转换为普通符号。
        /// </summary>
        /// <param name="value">放在 <c>LIKE</c> 中的值。</param>
        /// <returns>% 返回 [%]，_ 返回 [_]。</returns>
        public override string WildcardEscape(string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            throw new NotImplementedException();
        }



        /// <summary>
        /// 创建一个 <c>MySql</c> 数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlConnection"/> 对象实例。</returns>
        public override IDbConnection BuildConnection()
        {
            return BuildSqlConnection();
        }


        /// <summary>
        /// 使用默认数据库连接对象创建一个可在 <c>MySql</c> 数据库执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlCommand"/> 对象实例。</returns>
        public override IDbCommand BuildCommand()
        {
            return BuildSqlCommand(BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的 <c>MySql</c> 数据库连接对象创建一个执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <param name="conn"><see cref="MySqlConnection"/> 对象实例。</param>
        /// <returns>返回一个 <see cref="MySqlCommand"/> 对象实例。</returns>
        public override IDbCommand BuildCommand(IDbConnection conn)
        {
            return BuildSqlCommand(ConvertToSqlConnection(conn));
        }


        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns>数据库事务对象实例。</returns>
        public override IDbTransaction BuildTransaction()
        {
            return BuildSqlTransaction();
        }
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns>数据库事务对象实例。</returns>
        public override IDbTransaction BuildTransaction(IsolationLevel level)
        {
            return BuildSqlTransaction(level);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public override Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default)
        {
            return BuildSqlTransactionAsync(cancellationToken)
                .ContinueWith(o => (IDbTransaction)o.Result);
        }
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public override Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            return BuildSqlTransactionAsync(level, cancellationToken)
                .ContinueWith(o => (IDbTransaction)o.Result);
        }
#endif //!NETSTANDARD2_0


        /// <summary>
        /// 创建一个 <see cref="MySqlParameter"/> 对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="MySqlParameter"/> 对象实例。</returns>
        public override IDbDataParameter BuildParameter()
        {
            return BuildSqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个 <see cref="MySqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即 <see cref="MySqlParameter.ParameterName"/> 属性的值。</param>
        /// <param name="value">参数值，即 <see cref="MySqlParameter.Value"/> 属性的值。</param>
        /// <returns>返回一个 <see cref="MySqlParameter"/> 对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, object value)
        {
            return BuildSqlParameter(parameterName, value);
        }




        /// <summary>
        /// 执行指定 <c>T-SQL</c> 命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param> 
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param> 
        /// <returns>返回受影响的记录数。</returns>
        protected override int ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out mustCloseConnection);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out _);
                }

                int retval = cmd.ExecuteNonQuery();

                //TODO:myun:清除参数、关闭连接，会不会影响获取输出参数额值？
                cmd.Parameters.Clear();

                return retval;
            }
            finally
            {
                if (mustCloseConnection) connection.Close();
            }
        }

        /// <summary>
        /// 异步执行指定 <c>T-SQL</c> 命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，命令文本或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param> 
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        protected override async Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null)
#else
        protected override async ValueTask<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null)
#endif
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }

                int retval = cancellationToken.HasValue
                    ? await cmd.ExecuteNonQueryAsync(cancellationToken.Value)
                    : await cmd.ExecuteNonQueryAsync();

                //TODO:myun:清除参数、关闭连接，会不会影响获取输出参数额值？
                cmd.Parameters.Clear();

                return retval;
            }
            finally
            {
                if (mustCloseConnection) connection.Close();
            }
        }

        /// <summary>
        /// 执行指定 <c>T-SQL</c> 命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param> 
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param> 
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
        protected override object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out mustCloseConnection);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out _);
                }

                object retval = cmd.ExecuteScalar();

                //TODO:myun:清除参数、关闭连接，会不会影响获取输出参数额值？
                cmd.Parameters.Clear();

                return retval;
            }
            finally
            {
                if (mustCloseConnection) connection.Close();
            }
        }

        /// <summary>
        /// 异步执行指定 <c>T-SQL</c> 命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param> 
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
        protected override async Task<object> ExecuteScalarAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }

                object retval = cancellationToken.HasValue
                    ? await cmd.ExecuteScalarAsync(cancellationToken.Value)
                    : await cmd.ExecuteScalarAsync();

                //TODO:myun:清除参数、关闭连接，会不会影响获取输出参数额值？
                cmd.Parameters.Clear();

                return retval;
            }
            finally
            {
                if (mustCloseConnection) connection.Close();
            }
        }


        /// <summary>
        /// 执行指定 <c>T-SQL</c> 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param> 
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param> 
        /// <returns>返回包含结果集的数据读取器。</returns> 
        protected override IDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null && transaction == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand();

                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out mustCloseConnection);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), out _);
                }


                // 创建数据阅读器 
                MySqlDataReader dataReader = (connectionOwnership == SqlConnectionOwnership.External)
                    ? cmd.ExecuteReader()
                    : cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // 清除参数,以便再次使用.. 
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command 
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command. 
                bool canClear = true;
                foreach (MySqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear) cmd.Parameters.Clear();

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection) connection.Close();

                throw;
            }
        }

        /// <summary>
        /// 异步执行指定 <c>T-SQL</c> 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken? cancellationToken = null)
        {
            if (connection == null && transaction == null) throw new ArgumentNullException(nameof(connection));
            if (transaction != null)
            {
                // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }

            bool mustCloseConnection = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand();

                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }
                else
                {
                    // MySql 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters), cancellationToken);
                }


                // 创建数据阅读器 
                DbDataReader dataReader = await (connectionOwnership == SqlConnectionOwnership.External
                    ? cancellationToken.HasValue
                        ? cmd.ExecuteReaderAsync(cancellationToken.Value)
                        : cmd.ExecuteReaderAsync()
                    : cancellationToken.HasValue
                        ? cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken.Value)
                        : cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection));

                // 清除参数,以便再次使用.. 
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command 
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command. 
                bool canClear = true;
                foreach (MySqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear) cmd.Parameters.Clear();


                return dataReader;
            }
            catch
            {
                if (mustCloseConnection) connection.Close();

                throw;
            }
        }
        #endregion Override base class RelationalDBUtilityBase

        /* 使用 MySqlHelper 实现的异步返回读取器
        protected override async Task<MySqlDataReader> ExecuteReaderAsync(IDbConnection connection, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken? cancellationToken = null)
        {
            return MySqlHelper.ExecuteReaderAsync(ConvertToSqlConnection(connection), commandText, cancellationToken.Value, ConvertToSqlParameterArrary(commandParameters));
        }
        */


        /// <summary>
        /// 将 <see cref="IDataParameter"/>[] 转换为 <see cref="MySqlParameter[]"/> 类型。
        /// </summary>
        /// <param name="parameters">参数集。</param>
        /// <returns>如果 <paramref name="parameters"/> 为 <c>null</c> 或包含零个元素则返回 <c>null</c>，否则返回 <see cref="MySqlParameter[]"/>。</returns>
        private MySqlParameter[] ConvertToSqlParameterArrary(IDataParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0) return null;

            if (parameters is MySqlParameter[] result) return result;

            result = new MySqlParameter[parameters.Length];
            for (int i = parameters.Length - 1; i >= 0; i--)
            {
                result[i] = (MySqlParameter)parameters[i];
            }

            return result;
        }

        /// <summary>
        /// 将 <paramref name="conn"/> 转换为 <see cref="MySqlConnection"/> 对象实例。
        /// </summary>
        /// <param name="conn"><see cref="MySqlConnection"/> 对象。</param>
        /// <returns>返回 <see cref="MySqlConnection"/> 对象实例。</returns>
        /// <exception cref="ArgumentException">如果 <paramref name="conn"/> 不是 <see cref="MySqlConnection"/> 对象实例且不能转换成 <see cref="MySqlConnection"/> 对象实例则抛出此异常。</exception>
        private MySqlConnection ConvertToSqlConnection(IDbConnection conn)
        {
            if (!(conn is MySqlConnection result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_database_connection_objects_of_type_XXX, typeof(MySqlConnection).FullName), nameof(conn));

            return result;
        }

        /// <summary>
        /// 将 <paramref name="tran"/> 转换为 <see cref="MySqlTransaction"/> 对象实例。
        /// </summary>
        /// <param name="tran"><see cref="MySqlTransaction"/> 对象实例。</param>
        /// <returns>返回 <see cref="MySqlTransaction"/> 对象实例。</returns>
        /// <exception cref="ArgumentException">如果 <paramref name="tran"/> 不是 <see cref="MySqlTransaction"/> 对象实例且不能转换成 <see cref="MySqlTransaction"/> 对象实例则抛出此异常。</exception>
        private MySqlTransaction ConvertToSqlTransaction(IDbTransaction tran)
        {
            if (!(tran is MySqlTransaction result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_transaction_objects_of_type_XXX, typeof(MySqlTransaction).FullName), nameof(tran));

            return result;
        }



        #region SqlHelper
        /// <summary>
        /// 将 <see cref="MySqlParameter"/> 参数数组分配给 <see cref="MySqlCommand"/> 对象实例。
        /// 这个方法将值为 <c>null</c> 的 <see cref="ParameterDirection.Input"/> 或 <see cref="ParameterDirection.InputOutput"/> 参数赋值为 <see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param> 
        /// <param name="commandParameters">参数数组。</param>
        /// <exception cref="ArgumentNullException">当 <paramref name="command"/> 为 <c>null</c> 是引发此异常。</exception>
        private void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandParameters != null)
            {
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if (p.Value == null && (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// 预处理用户提供的命令。
        /// </summary>
        /// <param name="command">要处理的命令对象。</param> 
        /// <param name="connection">数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或者是 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 命令。</param> 
        /// <param name="commandParameters">和命令相关联的参数数组，如果没有参数为 <c>null</c>。</param> 
        /// <param name="mustCloseConnection">如果连接是打开的则为 <c>true</c>，否则为 <c>false</c>。</param> 
        private void PrepareCommand(MySqlCommand command, MySqlConnection connection, MySqlTransaction transaction, CommandType commandType, string commandText, MySqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (transaction == null)
            {
                command.Connection = connection;

                if (connection.State == ConnectionState.Open)
                {
                    mustCloseConnection = false;
                }
                else
                {
                    mustCloseConnection = true;
                    connection.Open();
                }
            }
            else
            {
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                mustCloseConnection = false;
            }
        }

        /// <summary>
        /// 预处理用户提供的命令。 
        /// </summary>
        /// <param name="command">要处理的命令对象。</param> 
        /// <param name="connection">数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或者是 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 命令。</param> 
        /// <param name="commandParameters">和命令相关联的参数数组，如果没有参数为 <c>null</c>。</param> 
        /// <param name="cancellationToken">传播取消操作通知的 <c>Token</c>。</param>
#if NETSTANDARD2_0
        private async Task PrepareCommandAsync(MySqlCommand command, MySqlConnection connection, MySqlTransaction transaction, CommandType commandType, string commandText, MySqlParameter[] commandParameters, CancellationToken? cancellationToken = null)
#else
        private async ValueTask PrepareCommandAsync(MySqlCommand command, MySqlConnection connection, MySqlTransaction transaction, CommandType commandType, string commandText, MySqlParameter[] commandParameters, CancellationToken? cancellationToken = null)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (transaction == null)
            {
                command.Connection = connection;

                if (connection.State != ConnectionState.Open)
                {
                    if (cancellationToken.HasValue)
                    {
                        await connection.OpenAsync(cancellationToken.Value);
                    }
                    else
                    {
                        await connection.OpenAsync();
                    }
                }
            }
            else
            {
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
            }
        }
        #endregion SqlHelper
    }
}
