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
    using System.Buffers;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    using global::MySql.Data.MySqlClient;


    /// <summary>
    ///<c>MySql</c>数据库的常用操作工具类库。
    /// </summary>
    public class MySQLUtility : RelationalDBUtilityBase
    {
        /// <summary>
        /// 创建<c>MySql</c>数据库操作对象实例。
        /// </summary>
        /// <param name="connStr">数据库连接字符串。</param>
        public MySQLUtility(string connStr)
            : base(connStr)
        { }

        /// <summary>
        /// 创建<c>MySql</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public MySQLUtility(ServersPolling polling, string[] masterConnStr)
            : base(polling, masterConnStr, null)
        { }

        /// <summary>
        /// 创建<c>MySql</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public MySQLUtility(ServersPolling polling, string[] masterConnStr, string[] slaveConnStr)
#else
        public MySQLUtility(ServersPolling polling, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, masterConnStr, slaveConnStr)
        { }

        /// <summary>
        /// 创建<c>MySql</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public MySQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr)
            : base(polling, role, masterConnStr, null)
        { }

        /// <summary>
        /// 创建<c>MySql</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public MySQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[] slaveConnStr)
#else
        public MySQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, role, masterConnStr, slaveConnStr)
        { }



        /// <summary>
        /// 创建一个<c>MySql</c>数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>对象实例。</returns>
        public MySqlConnection BuildSqlConnection()
        {

            if (TargetRole == DatabaseServerRole.Master)
            {
                return new MySqlConnection(_masterConnStr[_masterConnStrIndex]);
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
#if NETSTANDARD2_0                    
                return new MySqlConnection(_slaveConnStr[_slaveConnStrIndex]);
#else
                return new MySqlConnection(_slaveConnStr![_slaveConnStrIndex]);
#endif
            }
            else
            {
                return new MySqlConnection(_allConnStr[_allConnStrIndex]);
            }
        }

        /// <summary>
        /// 使用指定的<c>MySql</c>数据库连接字符串创建连接对象实例。
        /// </summary>
        /// <param name="connectionString"><c>MySql</c>数据库连接字符。</param>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>对象实例。</returns>
        /// <exception cref="ArgumentNullException">参数<paramref name="connectionString"/>的值为<c>null</c>。</exception>
        /// <exception cref="ArgumentException">参数<paramref name="connectionString"/>的值为空白字符。</exception>
        public MySqlConnection BuildSqlConnection(string connectionString)
        {
            if (null == connectionString) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(Common.Properties.Localization.the_database_connection_string_cannot_be_null_or_empty, nameof(connectionString));

            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public MySqlConnection BuildSqlConnectionAndOpen()
        {
            var conn = BuildSqlConnection();

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public MySqlConnection BuildSqlConnectionAndOpen(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<MySqlConnection> BuildSqlConnectionAndOpenAsync()
        {
            var conn = BuildSqlConnection();

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<MySqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<MySqlConnection> BuildSqlConnectionAndOpenAsync(CancellationToken cancellationToken = default)
        {
            var conn = BuildSqlConnection();

            await conn.OpenAsync(cancellationToken);

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个表示<c>MySQL</c>数据库链接对象<see cref="MySqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<MySqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync(cancellationToken);

            return conn;
        }



        /// <summary>
        /// 使用默认数据库连接对象创建一个可在<c>MySql</c>数据库执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="MySqlCommand"/>对象实例。</returns>
        public MySqlCommand BuildSqlCommand()
        {
            return new MySqlCommand(null, BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的<c>MySql</c>数据库连接对象创建一个执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <param name="conn">数据库连接对象实例。</param>
        /// <returns>返回一个<see cref="MySqlCommand"/>对象实例。</returns>
        public MySqlCommand BuildSqlCommand(MySqlConnection conn)
        {
            return new MySqlCommand(null, conn);
        }



        

        /// <summary>
        /// 创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
        public MySqlParameter BuildSqlParameter()
        {
            return new MySqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="MySqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="MySqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public MySqlParameter BuildSqlParameter(string parameterName, object value)
#else
        public MySqlParameter BuildSqlParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return new MySqlParameter(parameterName, value ?? DBNull.Value);
        }
        /// <summary>
        /// 创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="MySqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="parameterType">参数类型，即<see cref="MySqlParameter.MySqlDbType"/>属性的值。</param>
        /// <param name="size">参数大小，即<see cref="MySqlParameter.Size"/>属性的值。</param>
        /// <param name="direction">参数方向，即<see cref="MySqlParameter.Direction"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="MySqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public MySqlParameter BuildSqlParameter(string parameterName, MySqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object value = default)
#else
        public MySqlParameter BuildSqlParameter(string parameterName, MySqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object? value = default)
#endif //NETSTANDARD2_0
        {
            return new MySqlParameter(parameterName, parameterType, size)
            {
                Direction = direction,
                Value = value ?? DBNull.Value
            };
        }



        #region Override base class RelationalDBUtilityBase
        /// <summary>
        /// 安全处理程序
        /// </summary>
        /// <param name="value">预使用字符串拼接的值。</param>
        /// <returns>处理后的字符串值。</returns>
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
        /// <param name="value">放在<c>LIKE</c>中的值。</param>
        /// <returns>% 返回 [%]，_ 返回 [_]。</returns>
        public override string WildcardEscape(string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            throw new NotImplementedException();
        }



        /// <summary>
        /// 创建一个<c>MySql</c>数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="MySqlConnection"/>对象实例。</returns>
        public override IDbConnection BuildConnection()
        {
            return BuildSqlConnection();
        }

        public override IDbConnection BuildConnection(string connectionString)
        {
            return BuildSqlConnection(connectionString);
        }

        public override IDbConnection BuildConnectionAndOpen()
        {
            return BuildSqlConnectionAndOpen();
        }

        public override IDbConnection BuildConnectionAndOpen(string connectionString)
        {
            return BuildSqlConnectionAndOpen(connectionString);
        }

        public async override Task<IDbConnection> BuildConnectionAndOpenAsync()
        {
            return await BuildSqlConnectionAndOpenAsync();
        }

        public async override Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString)
        {
            return await BuildSqlConnectionAndOpenAsync(connectionString);
        }

        public async override Task<IDbConnection> BuildConnectionAndOpenAsync(CancellationToken cancellationToken = default)
        {
            return await BuildSqlConnectionAndOpenAsync(cancellationToken);
        }

        public async override Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            return await BuildSqlConnectionAndOpenAsync(connectionString, cancellationToken);
        }


        /// <summary>
        /// 使用默认数据库连接对象创建一个可在<c>MySql</c>数据库执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="MySqlCommand"/>对象实例。</returns>
        public override IDbCommand BuildCommand()
        {
            return BuildSqlCommand(BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的<c>MySql</c>数据库连接对象创建一个执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <param name="conn"><see cref="MySqlConnection"/>对象实例。</param>
        /// <returns>返回一个<see cref="MySqlCommand"/>对象实例。</returns>
        public override IDbCommand BuildCommand(IDbConnection conn)
        {
            return BuildSqlCommand(ConvertToSqlConnection(conn));
        }




        /// <summary>
        /// 创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter()
        {
            return BuildSqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="MySqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="MySqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, value);
        }
        /// <summary>
        /// 使用指定参数名、类型和大小创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, int parameterType, int size)  //如果给 size 赋默认值就会导致 参数值为 int 类型时把值当作参数类型。
        {
            return BuildSqlParameter(parameterName, (MySqlDbType)parameterType, size);
        }
        /// <summary>
        /// 使用指定参数名、类型、大小和方向创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, ParameterDirection direction, int parameterType, int size = default)
        {
            return BuildSqlParameter(parameterName, (MySqlDbType)parameterType, size, direction);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型和大小创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (MySqlDbType)parameterType, size, value: value);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型、大小和方向创建一个<see cref="MySqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="MySqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, ParameterDirection direction, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, ParameterDirection direction, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (MySqlDbType)parameterType, size, direction, value);
        }

        /// <summary>
        /// 将参数对象的参数名、参数值和方向重置为指定的值，类型和大小随着<paramref name="value"/>的类型默认重置。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <exception cref="NotImplementedException"></exception>
#if NETSTANDARD2_0
        public override void ResetParameter(IDbDataParameter parameter, string parameterName, object value, ParameterDirection direction = ParameterDirection.Input)
#else
        public override void ResetParameter(IDbDataParameter parameter, string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input)
#endif //NETSTANDARD2_0
        {
            if (parameter is MySqlParameter p)
            {
                p.ResetDbType();
                
                if (p.Direction != direction) p.Direction = direction;
                if (p.Size > 0) p.Size = 0;
                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(MySqlParameter).FullName}”类型的参数对象。", nameof(parameter));
            }

            //还没有验证以上实现对象重置后是否达到重置效果，在验证之前始终抛出异常，以免调用后达不到重置效果引起不必要的麻烦。
            throw new NotImplementedException("还没有验证参数对象重置后是否达到重置效果。在验证之前始终抛出异常，以免调用后达不到重置效果引起不必要的麻烦。");
        }

        /// <summary>
        /// 将参数对象的参数名、参数值、类型、大小和方向重置为指定的值。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <param name="direction">参数方向。</param>
#if NETSTANDARD2_0
        public override void ResetParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#else
        public override void ResetParameter(IDbDataParameter parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#endif //NETSTANDARD2_0
        {
            if (parameter is MySqlParameter p)
            {
                var type = (MySqlDbType)parameterType;

                if (p.MySqlDbType != type) p.MySqlDbType = type;
                if (p.Size != size) p.Size = size;
                if (p.Direction != direction) p.Direction = direction;

                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(MySqlParameter).FullName}”类型的参数对象。", nameof(parameter));
            }
        }

        /// <summary>
        /// 如果<paramref name="parameter"/>不为<c>null</c>则将参数对象的参数名、参数值、类型、大小和方向重置为指定的值，否则用这些参数创建一个新的参数对象。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <param name="direction">参数方向。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter ResetOrBuildParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#else
        public override IDbDataParameter ResetOrBuildParameter(IDbDataParameter? parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input)
#endif //NETSTANDARD2_0
        {
            if (parameter is null)
            {
                return BuildSqlParameter(parameterName, (MySqlDbType)parameterType, size, direction, value);
            }
            else
            {
                ResetParameter(parameter, parameterName, value, parameterType, size, direction);
                return parameter;
            }
        }





        /// <summary>
        /// 执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        public override int ExecuteNonQuery(IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, IEnumerable<IDataParameter> commandParameters = null)
#else
        public override int ExecuteNonQuery(IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSql(ExecuteNonQuery, transaction, commandText, commandType, commandParameters);

#if NETSTANDARD2_0
        public override int ExecuteNonQuery(IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IEnumerable<IDataParameter> commandParameters = null)
#else
        public override int ExecuteNonQuery(IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSql(ExecuteNonQuery, connection, commandText, commandType, commandParameters);


#if NETSTANDARD2_0
        public override Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        public override Task<int> ExecuteNonQueryAsync(IDbTransaction transaction, string commandText, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSqlAsync(ExecuteNonQueryAsync, transaction, commandText, commandType, cancellationToken, commandParameters);


#if NETSTANDARD2_0
        public override Task<int> ExecuteNonQueryAsync(IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        public override Task<int> ExecuteNonQueryAsync(IDbConnection connection, string commandText, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSqlAsync(ExecuteNonQueryAsync, connection, commandText, commandType, cancellationToken, commandParameters);



        /// <summary>
        /// 执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override object ExecuteScalar(IDbTransaction transaction, string commandText, CommandType commandType, IEnumerable<IDataParameter> commandParameters)
#else
        protected override object? ExecuteScalar(IDbTransaction transaction, string commandText, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters)
#endif
            => ExecuteSql(ExecuteScalar, transaction, commandText, commandType, commandParameters);

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override object ExecuteScalar(IDbConnection connection, string commandText, CommandType commandType, IEnumerable<IDataParameter> commandParameters)
#else
        protected override object? ExecuteScalar(IDbConnection connection, string commandText, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters)
#endif
            => ExecuteSql(ExecuteScalar, connection, commandText, commandType, commandParameters);

        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override Task<object> ExecuteScalarAsync(IDbTransaction transaction, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        protected override Task<object?> ExecuteScalarAsync(IDbTransaction transaction, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSqlAsync(ExecuteScalarAsync, transaction, commandText, commandType, cancellationToken, commandParameters);

        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override Task<object> ExecuteScalarAsync(IDbConnection connection, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        protected override Task<object?> ExecuteScalarAsync(IDbConnection connection, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
            => ExecuteSqlAsync(ExecuteScalarAsync, connection, commandText, commandType, cancellationToken, commandParameters);


        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭<see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbCommand command, IEnumerable<IDataParameter> commandParameters, CommandBehavior commandBehavior)
#else
        protected override IDataReader ExecuteReader(IDbCommand command, IEnumerable<IDataParameter?>? commandParameters, CommandBehavior commandBehavior)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
#if NET5_0_OR_GREATER
            if (command is not MySqlCommand cmd)
#else
            if (!(command is MySqlCommand cmd))
#endif
            {
                throw new InvalidCastException(String.Format(Common.Properties.Localization.is_not_a_valid_object_of_type_XXX, nameof(command), typeof(MySqlCommand).FullName));
            }
            if (cmd.Transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (cmd.Transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(cmd.Transaction));
                if (cmd.Connection == null)
                {
                    cmd.Connection = cmd.Transaction.Connection;
                }
                else if (!Object.ReferenceEquals(cmd.Connection, cmd.Transaction.Connection)) throw new ArgumentException(Common.Properties.Localization.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (cmd.Connection == null)
            {
                cmd.Connection = BuildSqlConnection();
            }

            bool mustCloseConnection = cmd.Connection?.State != ConnectionState.Open;

            try
            {
                if (commandParameters != null) AttachParameters(cmd, commandParameters);

                if (mustCloseConnection)
                {
                    cmd.Connection.Open();

                    if ((commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;
                }

                // 创建数据阅读器 
                return cmd.ExecuteReader(commandBehavior);
            }
            catch
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) cmd.Connection?.Close();

                throw;
            }
        }

        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior)"/>或<see cref="ExecuteReader(IDbTransaction, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务,或者为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters)
#else
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters)
#endif
            => ExecuteSql(ExecuteReader, transaction, commandText, commandType, commandBehavior, commandParameters);
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior)"/>或<see cref="ExecuteReader(IDbConnection, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters)
#else
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters)
#endif
            => ExecuteSql(ExecuteReader, connection, commandText, commandType, commandBehavior, commandParameters);

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭<see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, out IDbCommand command)
#else
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, out IDbCommand command)
#endif
        {
#if NET5_0_OR_GREATER
            (IDataReader reader, command) = ExecuteSql(ExecuteReaderAndReturnCommand, transaction, commandText, commandType, commandBehavior, commandParameters);
            return reader;
#else
            (IDataReader Reader, IDbCommand Cmd) tuple = ExecuteSql(ExecuteReaderAndReturnCommand, transaction, commandText, commandType, commandBehavior, commandParameters);
            command = tuple.Cmd;
            return tuple.Reader;
#endif
        }
        /// <summary>
        /// 执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭<see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, out IDbCommand command)
#else
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, out IDbCommand command)
#endif
        {
#if NET5_0_OR_GREATER
            (IDataReader reader, command) = ExecuteSql(ExecuteReaderAndReturnCommand, connection, commandText, commandType, commandBehavior, commandParameters);
            return reader;
#else
            (IDataReader Reader, IDbCommand Cmd) tuple = ExecuteSql(ExecuteReaderAndReturnCommand, connection, commandText, commandType, commandBehavior, commandParameters);
            command = tuple.Cmd;
            return tuple.Reader;
#endif
        }

        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="command">一个有效的用于执行数据库命令的对象。</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command, IEnumerable<IDataParameter> commandParameters, CommandBehavior commandBehavior, CancellationToken cancellationToken = default)
#else
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command, IEnumerable<IDataParameter?>? commandParameters, CommandBehavior commandBehavior, CancellationToken cancellationToken = default)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
#if NET5_0_OR_GREATER
            if (command is not MySqlCommand cmd)
#else
            if (!(command is MySqlCommand cmd))
#endif
            {
                throw new InvalidCastException(String.Format(Common.Properties.Localization.is_not_a_valid_object_of_type_XXX, nameof(command), typeof(MySqlCommand).FullName));
            }
            if (cmd.Transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (cmd.Transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(cmd.Transaction));
                if (!Object.ReferenceEquals(cmd.Connection, cmd.Transaction.Connection)) throw new ArgumentException(Common.Properties.Localization.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (cmd.Connection == null)
            {
                cmd.Connection = BuildSqlConnection();
            }

            bool mustCloseConnection = cmd.Connection?.State != ConnectionState.Open;

            try
            {
                if (commandParameters != null) AttachParameters(cmd, commandParameters);

                if (mustCloseConnection)
                {
                    var openTask = cmd.Connection.OpenAsync(cancellationToken);

                    if ((commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                    await openTask;
                }


                // 创建数据阅读器 
                return await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken);
            }
            catch
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) cmd.Connection?.Close();

                throw;
            }
        }
        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbTransaction, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        protected override Task<IDataReader> ExecuteReaderAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken = default)
#else
        protected override Task<IDataReader> ExecuteReaderAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken = default)
#endif
        => ExecuteSqlAsync(ExecuteReaderAsync, transaction, commandText, commandType, commandBehavior, commandParameters, cancellationToken);
        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbConnection, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        protected override Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken = default)
#else
        protected override Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken = default)
#endif
            => ExecuteSqlAsync(ExecuteReaderAsync, connection, commandText, commandType, commandBehavior, commandParameters, cancellationToken);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="transaction">一个有效的事务,或者为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器和输出<c>command</c>对象实例，用于在外部调用<see cref="IDbCommand.Parameters"/><c>.Clear()</c>方法。</returns>
#if NETSTANDARD2_0
        protected override Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken)
#else
        protected override Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken)
#endif
            => ExecuteSqlAsync(ExecuteReaderAndReturnCommandAsync, transaction, commandText, commandType, commandBehavior, commandParameters, cancellationToken);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器和输出<c>command</c>对象实例，用于在外部调用<see cref="IDbCommand.Parameters"/><c>.Clear()</c>方法。</returns>
#if NETSTANDARD2_0
        protected override Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken)
#else
        protected override Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken)
#endif
            => ExecuteSqlAsync(ExecuteReaderAndReturnCommandAsync, connection, commandText, commandType, commandBehavior, commandParameters, cancellationToken);
        #endregion Override base class RelationalDBUtilityBase





        /// <summary>
        /// 将<paramref name="conn"/>转换为<see cref="MySqlConnection"/>对象实例。
        /// </summary>
        /// <param name="conn"><see cref="MySqlConnection"/>对象。</param>
        /// <returns>返回<see cref="MySqlConnection"/>对象实例。</returns>
        /// <exception cref="ArgumentException">如果<paramref name="conn"/>不是<see cref="MySqlConnection"/>对象实例且不能转换成<see cref="MySqlConnection"/>对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private MySqlConnection ConvertToSqlConnection(IDbConnection conn)
#else
        private MySqlConnection ConvertToSqlConnection(IDbConnection? conn)
#endif // NETSTANDARD2_0
        {
            if (!(conn is MySqlConnection result)) throw new ArgumentException(String.Format(Common.Properties.Localization.this_instance_only_accepts_database_connection_objects_of_type_XXX, typeof(MySqlConnection).FullName), nameof(conn));

            return result;
        }

        /// <summary>
        /// 将<paramref name="tran"/>转换为<see cref="MySqlTransaction"/>对象实例。
        /// </summary>
        /// <param name="tran"><see cref="MySqlTransaction"/>对象实例。</param>
        /// <returns>返回<see cref="MySqlTransaction"/>对象实例。</returns>
        /// <exception cref="ArgumentException">如果<paramref name="tran"/>不是<see cref="MySqlTransaction"/>对象实例且不能转换成<see cref="MySqlTransaction"/>对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private MySqlTransaction ConvertToSqlTransaction(IDbTransaction tran)
#else
        private MySqlTransaction ConvertToSqlTransaction(IDbTransaction? tran)
#endif // NETSTANDARD2_0
        {
            if (!(tran is MySqlTransaction result)) throw new ArgumentException(String.Format(Common.Properties.Localization.this_instance_only_accepts_transaction_objects_of_type_XXX, typeof(MySqlTransaction).FullName), nameof(tran));

            return result;
        }



        #region SqlHelper
        /// <summary>
        /// 将<paramref name="parameters"/>中的元素添加到<see cref="MySqlCommand.Parameters"/>集合。
        /// 这个方法将值为<c>null</c>的<see cref="ParameterDirection.Input"/>或<see cref="ParameterDirection.InputOutput"/>参数赋值为<see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param>
        /// <param name="parameters">可迭代的SQL参数对象集合。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="command"/>为<c>null</c>是引发此异常。</exception>
#if NETSTANDARD2_0
        private void AttachParameters(MySqlCommand command, IEnumerable<MySqlParameter> parameters)
        {
            foreach (MySqlParameter p in parameters)
#else
        private void AttachParameters(MySqlCommand command, IEnumerable<MySqlParameter?> parameters)
        {
            foreach (MySqlParameter? p in parameters)
#endif
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
                else if (BreakWhenParametersElementIsNull)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 将<paramref name="parameters"/>中的元素添加到<see cref="MySqlCommand.Parameters"/>集合。
        /// 这个方法将值为<c>null</c>的<see cref="ParameterDirection.Input"/>或<see cref="ParameterDirection.InputOutput"/>参数赋值为<see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param>
        /// <param name="parameters">可迭代的SQL参数对象集合。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="command"/>为<c>null</c>是引发此异常。</exception>
#if NETSTANDARD2_0
        private void AttachParameters(MySqlCommand command, IEnumerable<IDataParameter> parameters)
        {
            if (parameters is IEnumerable<MySqlParameter> sqlParameters)
            {
                AttachParameters(command, sqlParameters);
            }
            else
            {
                foreach (MySqlParameter p in parameters)
#else
        private void AttachParameters(MySqlCommand command, IEnumerable<IDataParameter?> parameters)
        {
            if (parameters is IEnumerable<MySqlParameter?> sqlParameters)
            {
                AttachParameters(command, sqlParameters);
            }
            else
            {
                foreach (MySqlParameter? p in parameters)
#endif
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
                    else if (BreakWhenParametersElementIsNull)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 预处理用户提供的命令。
        /// </summary>
        /// <param name="command">要处理的命令对象。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param>
        /// <param name="commandParameters">和命令相关联的参数，如果没有参数为<c>null</c>。</param>
        /// <param name="mustCloseConnection"> 如果连接是打开的则为<c>true</c>，否则为<c>false</c>。</param>
#if NETSTANDARD2_0
        private void PrepareCommand(MySqlCommand command, CommandType commandType, IEnumerable<IDataParameter> commandParameters, out bool mustCloseConnection)
#else
        private void PrepareCommand(MySqlCommand command, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters, out bool mustCloseConnection)
#endif // NETSTANDARD2_0
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (command.CommandType != commandType) command.CommandType = commandType;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (command.Connection == null)
            {
                command.Connection = BuildSqlConnectionAndOpen();
                mustCloseConnection = true;
            }
            else if (command.Connection.State == ConnectionState.Open)
            {
                mustCloseConnection = false;
            }
            else
            {
                mustCloseConnection = true;
                command.Connection.Open();
            }
        }

        /// <summary>
        /// 预处理用户提供的命令。 
        /// </summary>
        /// <param name="command">要处理的命令对象。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param>
        /// <param name="commandParameters">和命令相关联的参数数组，如果没有参数为<c>null</c>。</param>
        /// <param name="cancellationToken">传播取消操作通知的<c>Token</c>。</param>
        /// <returns>返回一个布尔值，表示连接字符串是否需要主动关闭。<c>true</c>表示需要主动关闭连接，否则不需要主动关闭连接。</returns>
#if NETSTANDARD2_0
        private async ValueTask<bool> PrepareCommandAsync(MySqlCommand command, CommandType commandType, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken = default)
#else
        private async ValueTask<bool> PrepareCommandAsync(MySqlCommand command, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (command.CommandType != commandType) command.CommandType = commandType;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (command.Connection == null)
            {
                command.Connection = await BuildSqlConnectionAndOpenAsync(cancellationToken);
                return true;
            }
            else if (command.Connection.State == ConnectionState.Open)
            {
                return false;
            }
            else
            {
                await command.Connection.OpenAsync(cancellationToken);
                return true;
            }
        }

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="exec">执行SQL命令的方法。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        private T ExecuteSql<T>(Func<MySqlCommand, T> exec, IDbTransaction transaction, string commandText, CommandType commandType, IEnumerable<IDataParameter> commandParameters)
#else
        private T ExecuteSql<T>(Func<MySqlCommand, T> exec, IDbTransaction transaction, string commandText, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters)
#endif
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。    
            if (transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            var tran = ConvertToSqlTransaction(transaction);
            MySqlCommand cmd = new MySqlCommand(commandText, tran.Connection, tran);
            try
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                PrepareCommand(cmd, commandType, commandParameters, out _);
                return exec(cmd);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="exec">执行SQL命令的方法。</param>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandParameters">可迭代的SQL参数对象集合，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        private T ExecuteSql<T>(Func<MySqlCommand, T> exec, IDbConnection connection, string commandText, CommandType commandType, IEnumerable<IDataParameter> commandParameters)
#else
        private T ExecuteSql<T>(Func<MySqlCommand, T> exec, IDbConnection connection, string commandText, CommandType commandType, IEnumerable<IDataParameter?>? commandParameters)
#endif
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            MySqlCommand cmd = new MySqlCommand(commandText, ConvertToSqlConnection(connection));
            try
            {
                PrepareCommand(cmd, commandType, commandParameters, out mustCloseConnection);
                return exec(cmd);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();
            }
        }

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="execAsync">异步执行SQL命令的方法。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CancellationToken, Task<T>> execAsync, IDbTransaction transaction, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CancellationToken, Task<T>> execAsync, IDbTransaction transaction, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。    
            if (transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            var tran = ConvertToSqlTransaction(transaction);
            MySqlCommand cmd = new MySqlCommand(commandText, tran.Connection, tran);
            try
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                _ = await PrepareCommandAsync(cmd, commandType, commandParameters, cancellationToken);
                return await execAsync(cmd, cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// 执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="execAsync">执行SQL命令的方法。</param>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandParameters">可迭代的SQL参数对象集合，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CancellationToken, Task<T>> execAsync, IDbConnection connection, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter> commandParameters = null)
#else
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CancellationToken, Task<T>> execAsync, IDbConnection connection, string commandText, CommandType commandType, CancellationToken cancellationToken = default, IEnumerable<IDataParameter?>? commandParameters = null)
#endif
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            MySqlCommand cmd = new MySqlCommand(commandText, ConvertToSqlConnection(connection));
            try
            {
                mustCloseConnection = await PrepareCommandAsync(cmd, commandType, commandParameters, cancellationToken);
                return await execAsync(cmd, cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection.Close();
            }
        }


        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior)"/>或<see cref="ExecuteReader(IDbTransaction, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务,或者为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        private T ExecuteSql<T>(Func<MySqlCommand, CommandBehavior, T> exec, IDbTransaction transaction, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter> commandParameters)
#else
        private T ExecuteSql<T>(Func<MySqlCommand, CommandBehavior, T> exec, IDbTransaction transaction, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter?>? commandParameters)
#endif
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            var tran = ConvertToSqlTransaction(transaction);
            MySqlCommand cmd = new MySqlCommand(commandText, tran.Connection, tran);
            try
            {
                PrepareCommand(cmd, commandType, commandParameters, out _);

                // 创建数据阅读器 
                return exec(cmd, commandBehavior);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior)"/>或<see cref="ExecuteReader(IDbConnection, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        private T ExecuteSql<T>(Func<MySqlCommand, CommandBehavior, T> exec, IDbConnection connection, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter> commandParameters)
#else
        private T ExecuteSql<T>(Func<MySqlCommand, CommandBehavior, T> exec, IDbConnection connection, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter?>? commandParameters)
#endif
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            MySqlCommand cmd = new MySqlCommand(commandText, ConvertToSqlConnection(connection));
            try
            {
                PrepareCommand(cmd, commandType, commandParameters, out mustCloseConnection);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return exec(cmd, commandBehavior);
            }
            catch
            {
                if (mustCloseConnection) connection.Close();

                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }


        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbTransaction, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CommandBehavior, CancellationToken, Task<T>> execAsync, IDbTransaction transaction, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken = default)
#else
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CommandBehavior, CancellationToken, Task<T>> execAsync, IDbTransaction transaction, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Common.Properties.Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            var tran = ConvertToSqlTransaction(transaction);
            MySqlCommand cmd = new MySqlCommand(commandText, tran.Connection, tran);
            try
            {
                await PrepareCommandAsync(cmd, commandType, commandParameters, cancellationToken);

                // 创建数据阅读器 
                return await execAsync(cmd, commandBehavior, cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IEnumerable{IDataParameter?}?, CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbConnection, CommandBehavior, CommandType, string, IEnumerable{IDataParameter?}?, CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CommandBehavior, CancellationToken, Task<T>> execAsync, IDbConnection connection, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter> commandParameters, CancellationToken cancellationToken = default)
#else
        private async Task<T> ExecuteSqlAsync<T>(Func<MySqlCommand, CommandBehavior, CancellationToken, Task<T>> execAsync, IDbConnection connection, string commandText, CommandType commandType, CommandBehavior commandBehavior, IEnumerable<IDataParameter?>? commandParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = connection.State != ConnectionState.Open;
            MySqlCommand cmd = new MySqlCommand(commandText, ConvertToSqlConnection(connection));
            try
            {
                await PrepareCommandAsync(cmd, commandType, commandParameters, cancellationToken);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return await execAsync(cmd, commandBehavior, cancellationToken);
            }
            catch
            {
                if (mustCloseConnection) connection.Close();

                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(MySqlCommand cmd)
            => cmd.ExecuteNonQuery();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private Task<int> ExecuteNonQueryAsync(MySqlCommand cmd, CancellationToken cancellationToken)
            => cmd.ExecuteNonQueryAsync(cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        private object ExecuteScalar(MySqlCommand cmd)
#else
        private object? ExecuteScalar(MySqlCommand cmd)
#endif
            => cmd.ExecuteScalar();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        private Task<object> ExecuteScalarAsync(MySqlCommand cmd, CancellationToken cancellationToken)
#else
        private Task<object?> ExecuteScalarAsync(MySqlCommand cmd, CancellationToken cancellationToken)
#endif
            => cmd.ExecuteScalarAsync(cancellationToken);

        private IDataReader ExecuteReader(MySqlCommand cmd, CommandBehavior commandBehavior)
            => cmd.ExecuteReader(commandBehavior);

        private (IDataReader, IDbCommand) ExecuteReaderAndReturnCommand(MySqlCommand cmd, CommandBehavior commandBehavior)
            => (cmd.ExecuteReader(commandBehavior), cmd);

        private async Task<IDataReader> ExecuteReaderAsync(MySqlCommand cmd, CommandBehavior commandBehavior, CancellationToken cancellationToken)
            => await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken);

        private async Task<(IDataReader, IDbCommand)> ExecuteReaderAndReturnCommandAsync(MySqlCommand cmd, CommandBehavior commandBehavior, CancellationToken cancellationToken)
            => (await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken), cmd);
        #endregion SqlHelper
    }
}