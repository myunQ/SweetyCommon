/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  SQL Server 数据库的常用操作方法。
 * 
 * Members Index:
 *      class SqlServerUtility
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.DataProvider.SqlServer
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;




    /// <summary>
    /// <c>SQL Server</c> 数据库的常用操作工具类库。
    /// </summary>
    public class SqlServerUtility : RelationalDBUtilityBase
    {
        /// <summary>
        /// 创建 <c>SQL Server</c> 数据库操作对象实例。
        /// </summary>
        /// <param name="connStr">数据库连接字符串。</param>
        public SqlServerUtility(string connStr)
            : base(connStr)
        { }

        /// <summary>
        /// 创建 <c>SQL Server</c> 数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public SqlServerUtility(ServersPolling polling, string[] masterConnStr)
            : base(polling, masterConnStr, null)
        { }

        /// <summary>
        /// 创建 <c>SQL Server</c> 数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public SqlServerUtility(ServersPolling polling, string[] masterConnStr, string[] slaveConnStr)
#else
        public SqlServerUtility(ServersPolling polling, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, masterConnStr, slaveConnStr)
        { }

        /// <summary>
        /// 创建 <c>SQL Server</c> 数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public SqlServerUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr)
            : base(polling, role, masterConnStr, null)
        { }

        /// <summary>
        /// 创建 <c>SQL Server</c> 数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public SqlServerUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[] slaveConnStr)
#else
        public SqlServerUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, role, masterConnStr, slaveConnStr)
        { }




        /// <summary>
        /// 创建一个 <c>SQL Server</c> 数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 对象实例。</returns>
        public SqlConnection BuildSqlConnection()
        {
            if (TargetRole == DatabaseServerRole.Master)
            {
                return new SqlConnection(_masterConnStr[_masterConnStrIndex]);
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
#if NETSTANDARD2_0                    
                return new SqlConnection(_slaveConnStr[_slaveConnStrIndex]);
#else
                return new SqlConnection(_slaveConnStr![_slaveConnStrIndex]);
#endif
            }
            else
            {
                return new SqlConnection(_allConnStr[_allConnStrIndex]);
            }
        }

        /// <summary>
        /// 使用指定的 <c>SQL Server</c> 数据库连接字符串创建连接对象实例。
        /// </summary>
        /// <param name="connectionString"><c>SQL Server</c> 数据库连接字符。</param>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 对象实例。</returns>
        /// <exception cref="ArgumentNullException">参数 <paramref name="connectionString"/> 的值为 <c>null</c>。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="connectionString"/> 的值为空白字符。</exception>
        public SqlConnection BuildSqlConnection(string connectionString)
        {
            if (null == connectionString) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_string_cannot_be_null_or_empty, nameof(connectionString));

            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public SqlConnection BuildSqlConnectionAndOpen()
        {
            var conn = BuildSqlConnection();

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public SqlConnection BuildSqlConnectionAndOpen(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public async Task<SqlConnection> BuildSqlConnectionAndOpenAsync()
        {
            var conn = BuildSqlConnection();

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public async Task<SqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public async Task<SqlConnection> BuildSqlConnectionAndOpenAsync(CancellationToken cancellationToken = default)
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
        /// <returns>返回一个表示 <c>SQL Server</c> 数据库链接对象 <see cref="SqlConnection"/> 并且已与数据库连接的对象实例。</returns>
        public async Task<SqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync(cancellationToken);

            return conn;
        }


        /// <summary>
        /// 使用默认数据库连接对象创建一个可在 <c>SQL Server</c> 数据库执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="SqlCommand"/> 对象实例。</returns>
        public SqlCommand BuildSqlCommand()
        {
            return new SqlCommand(null, BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的 <c>SQL Server</c> 数据库连接对象创建一个执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <param name="conn">数据库连接对象实例。</param>
        /// <returns>返回一个<see cref="SqlCommand"/>对象实例。</returns>
        public SqlCommand BuildSqlCommand(SqlConnection conn)
        {
            return new SqlCommand(null, conn);
        }



        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns><c>SQL Server</c> 数据库事务对象实例。</returns>
        public SqlTransaction BuildSqlTransaction()
        {
            return BuildSqlConnectionAndOpen().BeginTransaction();
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns><c>SQL Server</c> 数据库事务对象实例。</returns>
        public SqlTransaction BuildSqlTransaction(IsolationLevel level)
        {
            return BuildSqlConnectionAndOpen().BeginTransaction(level);
        }

        /* SQL Server 客户端目前没有异步实现。目前 System.Data.SqlClient.SqlConnection 的 BeginTransactionAsync 方法的官方文档介绍是继承 System.Data.Common.DbConnection 的 BeginTransactionAsync 方法。而此方法是的实现是“将委托给其同步对应项，并返回已完成的 Task ，这可能会阻止调用线程。”
         */
        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>SQL Server</c> 数据库事务对象实例。</returns>
        public async Task<SqlTransaction> BuildSqlTransactionAsync(CancellationToken cancellationToken = default)
        {
            return (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransaction();
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>SQL Server</c> 数据库事务对象实例。</returns>
        public async Task<SqlTransaction> BuildSqlTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            return (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransaction(level);
        }



        /// <summary>
        /// 创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
        public SqlParameter BuildSqlParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即 <see cref="SqlParameter.ParameterName"/> 属性的值。</param>
        /// <param name="value">参数值，即 <see cref="SqlParameter.Value"/> 属性的值。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
#if NETSTANDARD2_0
        public SqlParameter BuildSqlParameter(string parameterName, object value)
#else
        public SqlParameter BuildSqlParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }
        /// <summary>
        /// 创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即 <see cref="SqlParameter.ParameterName"/> 属性的值。</param>
        /// <param name="parameterType">参数类型，即 <see cref="SqlParameter.SqlDbType"/> 属性的值。</param>
        /// <param name="size">参数大小，即 <see cref="SqlParameter.Size"/> 属性的值。</param>
        /// <param name="direction">参数方向，即 <see cref="SqlParameter.Direction"/> 属性的值。</param>
        /// <param name="value">参数值，即 <see cref="SqlParameter.Value"/> 属性的值。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
#if NETSTANDARD2_0
        public SqlParameter BuildSqlParameter(string parameterName, SqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object value = default)
#else
        public SqlParameter BuildSqlParameter(string parameterName, SqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object? value = default)
#endif //NETSTANDARD2_0
        {
            return new SqlParameter(parameterName, parameterType, size)
            {
                Direction = direction,
                Value = value ?? DBNull.Value
            };
        }




        #region Override base class RelationalDBUtilityBase
        /// <summary>
        /// 安全处理程序。
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
        /// <param name="value">放在 <c>LIKE</c> 中的值。</param>
        /// <returns>% 返回 [%]，_ 返回 [_]。</returns>
        public override string WildcardEscape(string value)
        {
            if (String.IsNullOrEmpty(value)) return value;
            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
             * SQL Server 还支持自定义转义符，使用关键词“ESCAPE”，例如将“\”作为转义符，语法如下：
             * SELECT * FROM table_name WHERE column LIKE 'YW\_%' ESCAPE '\' -- 查出所有以 YW_开头的内容。
             * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            return value.Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");
        }



        /// <summary>
        /// 创建一个 <c>SQL Server</c> 数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="SqlConnection"/> 对象实例。</returns>
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
        /// 使用默认数据库连接对象创建一个可在 <c>SQL Server</c> 数据库执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="SqlCommand"/> 对象实例。</returns>
        public override IDbCommand BuildCommand()
        {
            return BuildSqlCommand(BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的 <c>SQL Server</c> 数据库连接对象创建一个执行 <c>T-SQL</c> 命令的对象实例。
        /// </summary>
        /// <param name="conn"><see cref="SqlConnection"/> 对象实例。</param>
        /// <returns>返回一个 <see cref="SqlCommand"/> 对象实例。</returns>
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

        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public async override Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await BuildSqlTransactionAsync(cancellationToken);
        }
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public async override Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            return await BuildSqlTransactionAsync(level, cancellationToken);
        }


        /// <summary>
        /// 创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
        public override IDbDataParameter BuildParameter()
        {
            return BuildSqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即 <see cref="SqlParameter.ParameterName"/> 属性的值。</param>
        /// <param name="value">参数值，即 <see cref="SqlParameter.Value"/> 属性的值。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, value);
        }
        /// <summary>
        /// 使用指定参数名、类型和大小创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, int parameterType, int size) //如果给 size 赋默认值就会导致 参数值为 int 类型时把值当作参数类型。
        {
            return BuildSqlParameter(parameterName, (SqlDbType)parameterType, size);
        }
        /// <summary>
        /// 使用指定参数名、类型、大小和方向创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, ParameterDirection direction, int parameterType, int size = default)
        {
            return BuildSqlParameter(parameterName, (SqlDbType)parameterType, size, direction);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型和大小创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (SqlDbType)parameterType, size, value: value);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型、大小和方向创建一个 <see cref="SqlParameter"/> 对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个 <see cref="SqlParameter"/> 对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, ParameterDirection direction, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, ParameterDirection direction, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (SqlDbType)parameterType, size, direction, value);
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
            if (parameter is SqlParameter p)
            {
                var type = (SqlDbType)parameterType;

                if (p.SqlDbType != type) p.SqlDbType = type;
                if (p.Size != size) p.Size = size;
                if (p.Direction != direction) p.Direction = direction;

                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(SqlParameter).FullName}”类型的参数对象。", nameof(parameter));
            }
        }

        /// <summary>
        /// 如果<paramref name="parameter"/>不会<c>null</c>则将参数对象的参数名、参数值、类型、大小和方向重置为指定的值，否则用这些参数创建一个新的参数对象。
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
            if (parameter is null) return BuildSqlParameter(parameterName, (SqlDbType)parameterType, size, direction, value);

            if (parameter is SqlParameter p)
            {
                var type = (SqlDbType)parameterType;

                if (p.SqlDbType != type) p.SqlDbType = type;
                if (p.Size != size) p.Size = size;
                if (p.Direction != direction) p.Direction = direction;

                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
                return p;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(SqlParameter).FullName}”类型的参数对象。", nameof(parameter));
            }
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
#if NETSTANDARD2_0
        protected override int ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#else
        protected override int ExecuteNonQuery(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out mustCloseConnection);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out _);
                }

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Parameters.Clear();
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
        protected override async Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#else
        protected override async ValueTask<int> ExecuteNonQueryAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }

                return await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();
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
#if NETSTANDARD2_0
        protected override object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#else
        protected override object ExecuteScalar(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out mustCloseConnection);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out _);
                }

                return cmd.ExecuteScalar();
            }
            finally
            {
                cmd.Parameters.Clear();
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
#if NETSTANDARD2_0
        protected override async Task<object> ExecuteScalarAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#else
        protected override async Task<object> ExecuteScalarAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }

                return await cmd.ExecuteScalarAsync(cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
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
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭 <see cref="IDataReader"/> 对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器。</returns> 
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, out IDbCommand command)
#else
        protected override IDataReader ExecuteReader(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, out IDbCommand command)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            command = cmd;
            try
            {
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out mustCloseConnection);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, out _);
                }


                // 创建数据阅读器 
                return (connectionOwnership == SqlConnectionOwnership.External)
                    ? cmd.ExecuteReader()
                    : cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                if (mustCloseConnection) connection.Close();

                throw;
            }
            /*finally
            {
                ClearUsedParametersFromCommand(cmd);
            }*/
        }

        /// <summary> 
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary> 
        /// <param name="command">一个有效的用于执行数据库命令的对象。</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns> 
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
#if NET5_0_OR_GREATER
            if (command is not SqlCommand cmd)
#else
            if (!(command is SqlCommand cmd))
#endif
            {
                throw new InvalidCastException(String.Format(Properties.LocalizationResources.is_not_a_valid_object_of_type_XXX, nameof(command), typeof(SqlCommand).FullName));
            }
            if (cmd.Transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (cmd.Transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(cmd.Transaction));
                if (cmd.Connection != null && !Object.ReferenceEquals(cmd.Connection, cmd.Transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (cmd.Connection == null)
            {
                if (connectionOwnership != SqlConnectionOwnership.Internal) connectionOwnership = SqlConnectionOwnership.Internal;
                cmd.Connection = BuildSqlConnection();
            }

            bool mustCloseConnection = false;

            try
            {
                if (commandParameters != null && commandParameters.Length > 0)
                {
                    AttachParameters(cmd, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters);
                }


                if (cmd.Connection.State != ConnectionState.Open)
                {
                    mustCloseConnection = true;
                    await cmd.Connection.OpenAsync(cancellationToken);
                }


                // 创建数据阅读器 
                return await (connectionOwnership == SqlConnectionOwnership.External
                    ? cmd.ExecuteReaderAsync(cancellationToken)
                    : cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken));
            }
            catch
            {
                if (mustCloseConnection) cmd.Connection.Close();

                throw;
            }
            /* 此方法的调用放通过 out command 参数在方法外部清空参数集合。
            finally
            {
                ClearUsedParametersFromCommand(cmd);
            }*/
        }

        /// <summary>
        /// 异步执行指定 <c>T-SQL</c> 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取存储过程的输出参数的，如果要获取存储过程输出参数的值请使用 <see cref="ExecuteReaderAsync(IDbCommand, IDataParameter[], SqlConnectionOwnership, CancellationToken)"/> 或 <see cref="ExecuteReader(IDbConnection?, IDbTransaction?, CommandType, string, IDataParameter[], SqlConnectionOwnership, out IDbCommand)"/> 方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param> 
        /// <param name="transaction">一个有效的事务或 <c>null</c>。</param> 
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param> 
        /// <param name="commandText">存储过程名或 <c>T-SQL</c> 语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为 <c>null</c>。</param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
#if NETSTANDARD2_0
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken cancellationToken = default)
#else
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken cancellationToken = default)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (transaction == null)
                {
                    mustCloseConnection = connection.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }
                else
                {
                    // SQL Server 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, ConvertToSqlParameterArrary(commandParameters, out var recyclingParameters), recyclingParameters, cancellationToken);
                }


                // 创建数据阅读器 
                return await (connectionOwnership == SqlConnectionOwnership.External
                    ? cmd.ExecuteReaderAsync(cancellationToken)
                    : cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken));
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
        #endregion Override base class RelationalDBUtilityBase



        /// <summary>
        /// 将 <see cref="IDataParameter"/>[] 转换为 <see cref="SqlParameter[]"/> 类型。
        /// </summary>
        /// <param name="parameters">参数集。</param>
        /// <param name="recycling">指示调用方是否需要调用 <see cref="ArrayPool{T}.Return(T[], bool)"/> 方法回收返回的数据。</param>
        /// <returns>如果 <paramref name="parameters"/> 为 <c>null</c> 或包含零个元素则返回 <c>null</c>，否则返回 <see cref="SqlParameter[]"/>。</returns>
#if NETSTANDARD2_0
        private SqlParameter[] ConvertToSqlParameterArrary(IDataParameter[] parameters, out bool recycling)
#else
        private SqlParameter[]? ConvertToSqlParameterArrary(IDataParameter[]? parameters, out bool recycling)
#endif // NETSTANDARD2_0
        {
            if (parameters == null || parameters.Length == 0)
            {
                recycling = false;
                return null;
            }

            if (parameters is SqlParameter[] result)
            {
                recycling = false;
                return result;
            }


            result = ArrayPool<SqlParameter>.Shared.Rent(parameters.Length);
            try
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] == null)
                    {
                        if (BreakWhenParametersElementIsNull)
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    result[i] = (SqlParameter)parameters[i];
                }

                recycling = true;
            }
            catch
            {
                recycling = false;
                ArrayPool<SqlParameter>.Shared.Return(result, true);
            }

            return result;
        }

        /// <summary>
        /// 将 <paramref name="conn"/> 转换为 <see cref="SqlConnection"/> 对象实例。
        /// </summary>
        /// <param name="conn"><see cref="SqlConnection"/> 对象。</param>
        /// <returns>返回 <see cref="SqlConnection"/> 对象实例。</returns>
        /// <exception cref="ArgumentException">如果 <paramref name="conn"/> 不是 <see cref="SqlConnection"/> 对象实例且不能转换成 <see cref="SqlConnection"/> 对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private SqlConnection ConvertToSqlConnection(IDbConnection conn)
#else
        private SqlConnection ConvertToSqlConnection(IDbConnection? conn)
#endif // NETSTANDARD2_0
        {
            if (!(conn is SqlConnection result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_database_connection_objects_of_type_XXX, "System.Data.SqlClient.SqlConnection"), nameof(conn));

            return result;
        }

        /// <summary>
        /// 将 <paramref name="tran"/> 转换为 <see cref="SqlTransaction"/> 对象实例。
        /// </summary>
        /// <param name="tran"><see cref="SqlTransaction"/> 对象实例。</param>
        /// <returns>返回 <see cref="SqlTransaction"/> 对象实例。</returns>
        /// <exception cref="ArgumentException">如果 <paramref name="tran"/> 不是 <see cref="SqlTransaction"/> 对象实例且不能转换成 <see cref="SqlTransaction"/> 对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private SqlTransaction ConvertToSqlTransaction(IDbTransaction tran)
#else
        private SqlTransaction ConvertToSqlTransaction(IDbTransaction? tran)
#endif // NETSTANDARD2_0
        {
            if (!(tran is SqlTransaction result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_transaction_objects_of_type_XXX, "System.Data.SqlClient.SqlTransaction"), nameof(tran));

            return result;
        }



        #region SqlHelper
        /// <summary>
        /// 将 <see cref="SqlParameter"/> 参数数组分配给 <see cref="SqlCommand"/> 对象实例。
        /// 这个方法将值为 <c>null</c> 的 <see cref="ParameterDirection.Input"/> 或 <see cref="ParameterDirection.InputOutput"/> 参数赋值为 <see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param> 
        /// <param name="commandParameters">参数数组。</param>
        /// <param name="recyclingParameters">指示是否需要调用 <see cref="ArrayPool{T}.Return(T[], bool)"/> 方法回收 <paramref name="commandParameters"/> 参数。</param>
        /// <exception cref="ArgumentNullException">当 <paramref name="command"/> 为 <c>null</c> 是引发此异常。</exception>
        private void AttachParameters(SqlCommand command, SqlParameter[] commandParameters, bool recyclingParameters)
        {
            if (command == null)
            {
                if (recyclingParameters) ArrayPool<SqlParameter>.Shared.Return(commandParameters, true);

                throw new ArgumentNullException(nameof(command));
            }

            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value. 
                        if ((p.Value == null) && (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input))
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

                if (recyclingParameters) ArrayPool<SqlParameter>.Shared.Return(commandParameters, true);
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
        /// <param name="recyclingParameters">指示是否需要调用 <see cref="ArrayPool{T}.Return(T[], bool)"/> 方法回收 <paramref name="commandParameters"/> 参数。</param>
        /// <param name="mustCloseConnection">如果连接是打开的则为 <c>true</c>，否则为 <c>false</c>。</param> 
#if NETSTANDARD2_0
        private void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, bool recyclingParameters, out bool mustCloseConnection)
#else
        private void PrepareCommand(SqlCommand command, SqlConnection? connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[]? commandParameters, bool recyclingParameters, out bool mustCloseConnection)
#endif // NETSTANDARD2_0
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters, recyclingParameters);

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
        /// <param name="recyclingParameters">指示是否需要调用 <see cref="ArrayPool{T}.Return(T[], bool)"/> 方法回收 <paramref name="commandParameters"/> 参数。</param>
        /// <param name="cancellationToken">传播取消操作通知的 <c>Token</c>。</param>
#if NETSTANDARD2_0
        private async Task PrepareCommandAsync(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, bool recyclingParameters, CancellationToken cancellationToken = default)
#else
        private async ValueTask PrepareCommandAsync(SqlCommand command, SqlConnection? connection, SqlTransaction? transaction, CommandType commandType, string commandText, SqlParameter[]? commandParameters, bool recyclingParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters, recyclingParameters);

            if (transaction == null)
            {
                command.Connection = connection;

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }
            }
            else
            {
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
            }
        }



        /*/// <summary>
        /// 清除参数,以便再次使用。
        /// 如果没有输出参数，则调用 <see cref="SqlCommand.Parameters.Clear()"/> 方法。
        /// 只有使用 <see cref="SqlDataReader"/> 对象时才需要使用此方法。否则直接调用 <see cref="SqlCommand.Parameters.Clear()"/> 方法即可。
        /// </summary>
        /// <remarks>
        /// 当 <see cref="SqlDataReader"/> 关闭时输出参数值会被提取，所以如果参数与 <see cref="SqlCommand"/> 分离，那么 <see cref="SqlDataReader"/> 无法设置其值。
        /// 
        /// 发生这种情况时，参数不能在其他命令中再次使用。
        /// </remarks>
        /// <param name="cmd">命令对象。</param>
        private static void ClearUsedParametersFromCommand(SqlCommand cmd)
        {
            bool canClear = true;
            foreach (SqlParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                {
                    canClear = false;
                    break;
                }
            }

            if (canClear)
            {
                cmd.Parameters.Clear();
            }
        }*/
        #endregion SqlHelper
    }
}
