/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  PostgreSQL 数据库的常用操作方法。
 *
 *  PostgreSQL 官网 https://www.postgresql.org
 *  PostgreSQL 中文社区官网 http://www.postgres.cn/
 *  PostgreSQL 中文文档 http://www.postgres.cn/v2/document
 * 
 * Members Index:
 *      class PostgreSQLUtility
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.DataProvider.PostgreSQL
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    using Npgsql;
    using NpgsqlTypes;


    /// <summary>
    ///<c>PostgreSQL</c>数据库的常用操作工具类库。
    /// </summary>
    public class PostgreSQLUtility : RelationalDBUtilityBase
    {
        /// <summary>
        /// 创建<c>PostgreSQL</c>数据库操作对象实例。
        /// </summary>
        /// <param name="connStr">数据库连接字符串。</param>
        public PostgreSQLUtility(string connStr)
            : base(connStr)
        { }

        /// <summary>
        /// 创建<c>PostgreSQL</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public PostgreSQLUtility(ServersPolling polling, string[] masterConnStr)
            : base(polling, masterConnStr, null)
        { }

        /// <summary>
        /// 创建<c>PostgreSQL</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public PostgreSQLUtility(ServersPolling polling, string[] masterConnStr, string[] slaveConnStr)
#else
        public PostgreSQLUtility(ServersPolling polling, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, masterConnStr, slaveConnStr)
        { }

        /// <summary>
        /// 创建<c>PostgreSQL</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        public PostgreSQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr)
            : base(polling, role, masterConnStr, null)
        { }

        /// <summary>
        /// 创建<c>PostgreSQL</c>数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
#if NETSTANDARD2_0
        public PostgreSQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[] slaveConnStr)
#else
        public PostgreSQLUtility(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : base(polling, role, masterConnStr, slaveConnStr)
        { }



        /// <summary>
        /// 创建一个<c>PostgreSQL</c>数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>对象实例。</returns>
        public NpgsqlConnection BuildSqlConnection()
        {
            if (TargetRole == DatabaseServerRole.Master)
            {
                return new NpgsqlConnection(_masterConnStr[_masterConnStrIndex]);
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
#if NETSTANDARD2_0                    
                return new NpgsqlConnection(_slaveConnStr[_slaveConnStrIndex]);
#else
                return new NpgsqlConnection(_slaveConnStr![_slaveConnStrIndex]);
#endif
            }
            else
            {
                return new NpgsqlConnection(_allConnStr[_allConnStrIndex]);
            }
        }

        /// <summary>
        /// 使用指定的<c>PostgreSQL</c>数据库连接字符串创建连接对象实例。
        /// </summary>
        /// <param name="connectionString"><c>PostgreSQL</c>数据库连接字符。</param>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>对象实例。</returns>
        /// <exception cref="ArgumentNullException">参数<paramref name="connectionString"/>的值为<c>null</c>。</exception>
        /// <exception cref="ArgumentException">参数<paramref name="connectionString"/>的值为空白字符。</exception>
        public NpgsqlConnection BuildSqlConnection(string connectionString)
        {
            if (null == connectionString) throw new ArgumentNullException(nameof(connectionString));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_string_cannot_be_null_or_empty, nameof(connectionString));

            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public NpgsqlConnection BuildSqlConnectionAndOpen()
        {
            var conn = BuildSqlConnection();

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public NpgsqlConnection BuildSqlConnectionAndOpen(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            conn.Open();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<NpgsqlConnection> BuildSqlConnectionAndOpenAsync()
        {
            var conn = BuildSqlConnection();

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<NpgsqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync();

            return conn;
        }
        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<NpgsqlConnection> BuildSqlConnectionAndOpenAsync(CancellationToken cancellationToken = default)
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
        /// <returns>返回一个表示<c>PostgreSQL</c>数据库链接对象<see cref="NpgsqlConnection"/>并且已与数据库连接的对象实例。</returns>
        public async Task<NpgsqlConnection> BuildSqlConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            var conn = BuildSqlConnection(connectionString);

            await conn.OpenAsync(cancellationToken);

            return conn;
        }


        /// <summary>
        /// 使用默认数据库连接对象创建一个可在<c>PostgreSQL</c>数据库执行 T-SQL 命令的对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="NpgsqlCommand"/>对象实例。</returns>
        public NpgsqlCommand BuildSqlCommand()
        {
            return new NpgsqlCommand(null, BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的<c>PostgreSQL</c>数据库连接对象创建一个执行 T-SQL 命令的对象实例。
        /// </summary>
        /// <param name="conn">数据库连接对象实例。</param>
        /// <returns>返回一个<see cref="NpgsqlCommand"/>对象实例。</returns>
        public NpgsqlCommand BuildSqlCommand(NpgsqlConnection conn)
        {
            return new NpgsqlCommand(null, conn);
        }



        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns><c>PostgreSQL</c>数据库事务对象实例。</returns>
        public NpgsqlTransaction BuildSqlTransaction()
        {
            return BuildSqlConnectionAndOpen().BeginTransaction();
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns><c>PostgreSQL</c>数据库事务对象实例。</returns>
        public NpgsqlTransaction BuildSqlTransaction(IsolationLevel level)
        {
            return BuildSqlConnectionAndOpen().BeginTransaction(level);
        }

        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>PostgreSQL</c>数据库事务对象实例。</returns>
        public async Task<NpgsqlTransaction> BuildSqlTransactionAsync(CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            return (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransaction();
#else
            return await (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransactionAsync(cancellationToken);
#endif //NETSTANDARD2_0
        }

        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns><c>PostgreSQL</c>数据库事务对象实例。</returns>
        public async Task<NpgsqlTransaction> BuildSqlTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD2_0
            return (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransaction(level);
#else
            return await (await BuildSqlConnectionAndOpenAsync(cancellationToken)).BeginTransactionAsync(level, cancellationToken);
#endif //NETSTANDARD2_0
        }



        /// <summary>
        /// 创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
        public NpgsqlParameter BuildSqlParameter()
        {
            return new NpgsqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="NpgsqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="NpgsqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public NpgsqlParameter BuildSqlParameter(string parameterName, object value)
#else
        public NpgsqlParameter BuildSqlParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return new NpgsqlParameter(parameterName, value ?? DBNull.Value);
        }
        /// <summary>
        /// 创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="NpgsqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="parameterType">参数类型，即<see cref="NpgsqlParameter.NpgsqlDbType"/>属性的值。</param>
        /// <param name="size">参数大小，即<see cref="NpgsqlParameter.Size"/>属性的值。</param>
        /// <param name="direction">参数方向，即<see cref="NpgsqlParameter.Direction"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="NpgsqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public NpgsqlParameter BuildSqlParameter(string parameterName, NpgsqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object value = default)
#else
        public NpgsqlParameter BuildSqlParameter(string parameterName, NpgsqlDbType parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input, object? value = default)
#endif //NETSTANDARD2_0
        {
            return new NpgsqlParameter(parameterName, parameterType, size)
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
        /// <param name="value">放在<c>LIKE</c>中的值。</param>
        /// <returns>% 返回 \%，_ 返回 \_，\ 返回 \\。</returns>
        public override string WildcardEscape(string value)
        {
            if (String.IsNullOrEmpty(value)) return value;

            return value.Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
        }



        /// <summary>
        /// 创建一个<c>PostgreSQL</c>数据库连接对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="NpgsqlConnection"/>对象实例。</returns>
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
        /// 使用默认数据库连接对象创建一个可在<c>PostgreSQL</c>数据库执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="NpgsqlCommand"/>对象实例。</returns>
        public override IDbCommand BuildCommand()
        {
            return BuildSqlCommand(BuildSqlConnection());
        }

        /// <summary>
        /// 使用指定的<c>PostgreSQL</c>数据库连接对象创建一个执行<c>T-SQL</c>命令的对象实例。
        /// </summary>
        /// <param name="conn"><see cref="NpgsqlConnection"/>对象实例。</param>
        /// <returns>返回一个<see cref="NpgsqlCommand"/>对象实例。</returns>
        public override IDbCommand BuildCommand(IDbConnection conn)
        {
            return BuildSqlCommand(ConvertToSqlConnection(conn));
        }


        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public override IDbTransaction BuildTransaction()
        {
            return BuildSqlTransaction();
        }
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public override IDbTransaction BuildTransaction(IsolationLevel level)
        {
            return BuildSqlTransaction(level);
        }

        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public override async Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await BuildSqlTransactionAsync(cancellationToken);
        }
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public override async Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            return await BuildSqlTransactionAsync(level, cancellationToken);
        }


        /// <summary>
        /// 创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter()
        {
            return BuildSqlParameter();
        }

        /// <summary>
        /// 使用指定的参数名和参数值创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名，即<see cref="NpgsqlParameter.ParameterName"/>属性的值。</param>
        /// <param name="value">参数值，即<see cref="NpgsqlParameter.Value"/>属性的值。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, value);
        }
        /// <summary>
        /// 使用指定参数名、类型和大小创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, int parameterType, int size)  //如果给 size 赋默认值就会导致 参数值为 int 类型时把值当作参数类型。
        {
            return BuildSqlParameter(parameterName, (NpgsqlDbType)parameterType, size);
        }
        /// <summary>
        /// 使用指定参数名、类型、大小和方向创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
        public override IDbDataParameter BuildParameter(string parameterName, ParameterDirection direction, int parameterType, int size = default)
        {
            return BuildSqlParameter(parameterName, (NpgsqlDbType)parameterType, size, direction);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型和大小创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (NpgsqlDbType)parameterType, size, value: value);
        }
        /// <summary>
        /// 使用指定参数名、参数值、类型、大小和方向创建一个<see cref="NpgsqlParameter"/>对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个<see cref="NpgsqlParameter"/>对象实例。</returns>
#if NETSTANDARD2_0
        public override IDbDataParameter BuildParameter(string parameterName, object value, ParameterDirection direction, int parameterType, int size = default)
#else
        public override IDbDataParameter BuildParameter(string parameterName, object? value, ParameterDirection direction, int parameterType, int size = default)
#endif //NETSTANDARD2_0
        {
            return BuildSqlParameter(parameterName, (NpgsqlDbType)parameterType, size, direction, value);
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
            if (parameter is NpgsqlParameter p)
            {
                p.ResetDbType();

                if (p.Direction != direction) p.Direction = direction;
                if (p.Size > 0) p.Size = 0;
                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(NpgsqlParameter).FullName}”类型的参数对象。", nameof(parameter));
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
            if (parameter is NpgsqlParameter p)
            {
                var type = (NpgsqlDbType)parameterType;

                if (p.NpgsqlDbType != type) p.NpgsqlDbType = type;
                if (p.Size != size) p.Size = size;
                if (p.Direction != direction) p.Direction = direction;

                p.ParameterName = parameterName;
                p.Value = value ?? DBNull.Value;
            }
            else
            {
                throw new ArgumentException($"只接受“{typeof(NpgsqlParameter).FullName}”类型的参数对象。", nameof(parameter));
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
                return BuildSqlParameter(parameterName, (NpgsqlDbType)parameterType, size, direction, value);
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
        protected override int ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#else
        protected override int ExecuteNonQuery(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#endif //NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, out mustCloseConnection);
                }
                else
                {
                    // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, out _);
                }

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();
            }
        }

        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，命令文本或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回受影响的记录数。</returns>
#if NETSTANDARD2_0
        protected override async Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#else
        protected override async ValueTask<int> ExecuteNonQueryAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (transaction != null)
            {
                // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                if (transaction == null)
                {
                    mustCloseConnection = connection?.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, cancellationToken);
                }
                else
                {
                    // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, cancellationToken);
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
        /// 执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#else
        protected override object? ExecuteScalar(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                if (transaction == null)
                {
                    PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, out mustCloseConnection);
                }
                else
                {
                    // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, out _);
                }

                return cmd.ExecuteScalar();
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();
            }
        }

        /// <summary>
        /// 异步执行指定<c>T-SQL</c>命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
#if NETSTANDARD2_0
        protected override async Task<object> ExecuteScalarAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#else
        protected override async Task<object?> ExecuteScalarAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#endif // NETSTANDARD2_0
        {
            if (transaction != null)
            {
                // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));
                if (connection != null && !Object.ReferenceEquals(connection, transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                if (transaction == null)
                {
                    mustCloseConnection = connection?.State != ConnectionState.Open;
                    await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, cancellationToken);
                }
                else
                {
                    // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                    await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, cancellationToken);
                }

                return await cmd.ExecuteScalarAsync(cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();
            }
        }




        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭<see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected override IDataReader ExecuteReader(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior)
#else
        protected override IDataReader ExecuteReader(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior)
#endif //NETSTANDARD2_0
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
#if NET5_0_OR_GREATER
            if (command is not NpgsqlCommand cmd)
#else
            if (!(command is NpgsqlCommand cmd))
#endif
            {
                throw new InvalidCastException(String.Format(Properties.LocalizationResources.is_not_a_valid_object_of_type_XXX, nameof(command), typeof(NpgsqlCommand).FullName));
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
                if ((commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;
                cmd.Connection = BuildSqlConnection();
            }

            bool mustCloseConnection = cmd.Connection?.State != ConnectionState.Open;

            try
            {
                AttachParameters(cmd, commandParameters);

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
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IDataParameter[], CommandBehavior)"/>或<see cref="ExecuteReader(IDbConnection, CommandBehavior, CommandType, string, IDataParameter[], out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, out mustCloseConnection);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return cmd.ExecuteReader(commandBehavior);
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
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReader(IDbCommand, IDataParameter[], CommandBehavior)"/>或<see cref="ExecuteReader(IDbTransaction, CommandBehavior, CommandType, string, IDataParameter[], out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务,或者为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, out _);

                // 创建数据阅读器 
                return cmd.ExecuteReader(commandBehavior);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
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
        protected override IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, out IDbCommand command)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            command = cmd;

            try
            {
                PrepareCommand(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, out mustCloseConnection);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return cmd.ExecuteReader(commandBehavior);
            }
            catch
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection.Close();

                throw;
            }
        }
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
        protected override IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, out IDbCommand command)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            NpgsqlCommand cmd = new NpgsqlCommand();
            command = cmd;

            try
            {
                PrepareCommand(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, out _);

                // 创建数据阅读器 
                return cmd.ExecuteReader(commandBehavior);
            }
            catch
            {
                cmd.Parameters.Clear();

                throw;
            }
        }




        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="command">一个有效的用于执行数据库命令的对象。</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
#if NET5_0_OR_GREATER
            if (command is not NpgsqlCommand cmd)
#else
            if (!(command is NpgsqlCommand cmd))
#endif
            {
                throw new InvalidCastException(String.Format(Properties.LocalizationResources.is_not_a_valid_object_of_type_XXX, nameof(command), typeof(NpgsqlCommand).FullName));
            }
            if (cmd.Transaction != null)
            {
                // PostgreSQL 要获取 SqlTransaction 对象实例的话连接必须是打开的，所以如果传入 transaction 就可以忽略 connection 参数了。
                // 这里做 connection 和 transaction.Connection 是不是同一实例的验证只是为了避免调用方胡乱传值产生疑惑。
                if (cmd.Transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(cmd.Transaction));
                if (cmd.Connection != null && !Object.ReferenceEquals(cmd.Connection, cmd.Transaction.Connection)) throw new ArgumentException(Properties.LocalizationResources.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance);
            }
            else if (cmd.Connection == null)
            {
                if ((commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;
                cmd.Connection = BuildSqlConnection();
            }

            bool mustCloseConnection = cmd.Connection?.State != ConnectionState.Open;

            try
            {
                AttachParameters(cmd, commandParameters);

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
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IDataParameter[], CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbConnection, CommandBehavior, CommandType, string, IDataParameter[], CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = connection.State != ConnectionState.Open;
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, cancellationToken);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken);
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
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用<see cref="ExecuteReaderAsync(IDbCommand, IDataParameter[], CommandBehavior, CancellationToken)"/>或<see cref="ExecuteReaderAndGetCommandAsync(IDbTransaction, CommandBehavior, CommandType, string, IDataParameter[], CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务或<c>null</c>。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令或其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>语句。</param>
        /// <param name="commandParameters">参数数组，如果没有参数则为<c>null</c>。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器。</returns>
        protected override async Task<IDataReader> ExecuteReaderAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, cancellationToken);

                // 创建数据阅读器 
                return await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken);
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

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
        protected override async Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            bool mustCloseConnection = false;

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                mustCloseConnection = connection?.State != ConnectionState.Open;
                await PrepareCommandAsync(cmd, ConvertToSqlConnection(connection), null, commandType, commandText, commandParameters, cancellationToken);

                if (mustCloseConnection && (commandBehavior & CommandBehavior.CloseConnection) != CommandBehavior.CloseConnection) commandBehavior |= CommandBehavior.CloseConnection;

                // 创建数据阅读器 
                return (await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken), cmd);
            }
            catch
            {
                cmd.Parameters.Clear();
                if (mustCloseConnection) connection?.Close();

                throw;
            }
        }
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
        protected override async Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction.Connection == null) throw new ArgumentException(Properties.LocalizationResources.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction, nameof(transaction));

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                await PrepareCommandAsync(cmd, null, ConvertToSqlTransaction(transaction), commandType, commandText, commandParameters, cancellationToken);

                // 创建数据阅读器 
                return (await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken), cmd);
            }
            catch
            {
                cmd.Parameters.Clear();

                throw;
            }
        }
        #endregion Override base class RelationalDBUtilityBase




        /// <summary>
        /// 将<paramref name="conn"/>转换为<see cref="NpgsqlConnection"/>对象实例。
        /// </summary>
        /// <param name="conn"><see cref="NpgsqlConnection"/>对象。</param>
        /// <returns>返回<see cref="NpgsqlConnection"/>对象实例。</returns>
        /// <exception cref="ArgumentException">如果<paramref name="conn"/>不是<see cref="NpgsqlConnection"/>对象实例且不能转换成<see cref="NpgsqlConnection"/>对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private NpgsqlConnection ConvertToSqlConnection(IDbConnection conn)
#else
        private NpgsqlConnection ConvertToSqlConnection(IDbConnection? conn)
#endif //NETSTANDARD2_0
        {
            if (!(conn is NpgsqlConnection result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_database_connection_objects_of_type_XXX, typeof(NpgsqlConnection).FullName), nameof(conn));

            return result;
        }

        /// <summary>
        /// 将<paramref name="tran"/>转换为<see cref="NpgsqlTransaction"/>对象实例。
        /// </summary>
        /// <param name="tran"><see cref="NpgsqlTransaction"/>对象实例。</param>
        /// <returns>返回<see cref="NpgsqlTransaction"/>对象实例。</returns>
        /// <exception cref="ArgumentException">如果<paramref name="tran"/>不是<see cref="NpgsqlTransaction"/>对象实例且不能转换成<see cref="NpgsqlTransaction"/>对象实例则抛出此异常。</exception>
#if NETSTANDARD2_0
        private NpgsqlTransaction ConvertToSqlTransaction(IDbTransaction tran)
#else
        private NpgsqlTransaction ConvertToSqlTransaction(IDbTransaction? tran)
#endif // NETSTANDARD2_0
        {
            if (!(tran is NpgsqlTransaction result)) throw new ArgumentException(String.Format(Properties.LocalizationResources.this_instance_only_accepts_transaction_objects_of_type_XXX, typeof(NpgsqlTransaction).FullName), nameof(tran));

            return result;
        }



        #region SqlHelper
        /// <summary>
        /// 将<see cref="NpgsqlParameter"/>参数数组分配给<see cref="NpgsqlCommand"/>对象实例。
        /// 这个方法将值为<c>null</c>的<see cref="ParameterDirection.Input"/>或<see cref="ParameterDirection.InputOutput"/>参数赋值为<see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param>
        /// <param name="commandParameters">参数数组。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="command"/>为<c>null</c>是引发此异常。</exception>
        private void AttachParameters(NpgsqlCommand command, NpgsqlParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (commandParameters?.Length > 0)
            {
                foreach (NpgsqlParameter p in commandParameters)
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
        /// 将<see cref="NpgsqlParameter"/>参数数组分配给<see cref="NpgsqlCommand"/>对象实例。
        /// 这个方法将值为<c>null</c>的<see cref="ParameterDirection.Input"/>或<see cref="ParameterDirection.InputOutput"/>参数赋值为<see cref="DBNull.Value"/>。 
        /// </summary>
        /// <param name="command">命令对象实例。</param>
        /// <param name="commandParameters">参数数组。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="command"/>为<c>null</c>是引发此异常。</exception>
        private void AttachParameters(NpgsqlCommand command, IDataParameter[] commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (commandParameters?.Length > 0)
            {
                if (commandParameters is NpgsqlParameter[] paramenters)
                {
                    AttachParameters(command, paramenters);
                }
                else
                {
                    foreach (NpgsqlParameter p in commandParameters)
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
        }

        /// <summary>
        /// 预处理用户提供的命令。
        /// </summary>
        /// <param name="command">要处理的命令对象。</param>
        /// <param name="connection">数据库连接对象。</param>
        /// <param name="transaction">一个有效的事务或者是<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>命令。</param>
        /// <param name="commandParameters">和命令相关联的参数数组，如果没有参数为<c>null</c>。</param>
        /// <param name="mustCloseConnection"> 如果连接是打开的则为<c>true</c>，否则为<c>false</c>。</param>
#if NETSTANDARD2_0
        private void PrepareCommand(NpgsqlCommand command, NpgsqlConnection connection, NpgsqlTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, out bool mustCloseConnection)
#else
        private void PrepareCommand(NpgsqlCommand command, NpgsqlConnection? connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, IDataParameter[]? commandParameters, out bool mustCloseConnection)
#endif //NETSTANDARD2_0
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (transaction == null)
            {
                if (connection == null) throw new ArgumentException(String.Format(Properties.LocalizationResources.arguments_X_and_Y_cannot_be_null_at_the_same_time, nameof(connection), nameof(transaction)));

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
        /// <param name="transaction">一个有效的事务或者是<c>null</c>。</param>
        /// <param name="commandType">命令类型 (存储过程，文本命令，其它)。</param>
        /// <param name="commandText">存储过程名或<c>T-SQL</c>命令。</param>
        /// <param name="commandParameters">和命令相关联的参数数组，如果没有参数为<c>null</c>。</param>
        /// <param name="cancellationToken">传播取消操作通知的<c>Token</c>。</param>
#if NETSTANDARD2_0
        private async ValueTask PrepareCommandAsync(NpgsqlCommand command, NpgsqlConnection connection, NpgsqlTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default)
#else
        private async ValueTask PrepareCommandAsync(NpgsqlCommand command, NpgsqlConnection? connection, NpgsqlTransaction? transaction, CommandType commandType, string commandText, IDataParameter[]? commandParameters, CancellationToken cancellationToken = default)
#endif
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException(nameof(commandText));

            command.CommandType = commandType;
            command.CommandText = commandText;

            if (commandParameters != null) AttachParameters(command, commandParameters);

            if (transaction == null)
            {
                if (connection == null) throw new ArgumentException(String.Format(Properties.LocalizationResources.arguments_X_and_Y_cannot_be_null_at_the_same_time, nameof(connection), nameof(transaction)));
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
        #endregion SqlHelper
    }
}