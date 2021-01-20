/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  关系型数据库的常用操作基类。
 * 
 * Members Index:
 *      abstract class RelationalDBUtilityBase
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.DataProvider
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text;

    using Sweety.Common.Converter;


    /// <summary>
    /// 关系型数据库操作对象基类
    /// </summary>
    public abstract class RelationalDBUtilityBase : IRelationalDBUtility
    {
        /// <summary> 
        /// 枚举,标识数据库连接是由SqlHelper提供还是由调用者提供 
        /// </summary> 
        protected enum SqlConnectionOwnership
        {
            /// <summary>由SqlHelper提供连接</summary> 
            Internal,
            /// <summary>由调用者提供连接</summary> 
            External
        }

        /// <summary>
        /// 存放主库连接字符串和从库连接字符串，主库连接字符串放在前。
        /// </summary>
        protected static string[] __allConnStr;
        /// <summary>
        /// 存放主库连接字符串。
        /// </summary>
        protected static string[] __masterConnStr;
        /// <summary>
        /// 存放从库连接字符串。
        /// </summary>
        protected static string[] __slaveConnStr;
        /*
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected static bool[] __usableAllConnStr;
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected static bool[] __usableMasterConnStr;
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected static bool[] __usableSlaveConnStr;
        */


        static int __allConnStrIndex = 0;
        static int __masterConnStrIndex = 0;
        static int __slaveConnStrIndex = 0;

        /// <summary>
        /// 所有数据库链接（主数据库和从数据库）的连接字符串集合的索引。
        /// </summary>
        protected int _allConnStrIndex = -1;
        /// <summary>
        /// 主数据库链接字符串集合的索引。
        /// </summary>
        protected int _masterConnStrIndex = 0;
        /// <summary>
        /// 从数据库链接字符串集合的索引。
        /// </summary>
        protected int _slaveConnStrIndex = -1;


        protected object _modelInstance = null;              //model object.
        protected object _listInstance = null;               //IList<T>
        protected object _setInstance = null;                //ISet<T>
        protected object _collectionInstance = null;         //ICollection<T>
        protected object _dictionaryInstance = null;         //IDictionary<TKey, TValue>

        protected object _funBuildModelInstance = null;      //Func<T>
        protected object _funBuildListInstance = null;       //Func<T>
        protected object _funBuildSetInstance = null;        //Func<T>
        protected object _funBuildCollectionInstance = null; //Func<T>
        protected object _funBuildDictionaryInstance = null; //Func<T>

        DatabaseServerRole _targetRole = DatabaseServerRole.Master;


        /// <summary>
        /// 设置数据库连接字符串集合。
        /// </summary>
        /// <remarks>
        /// 在调用构造函数构造对象实例前，必须先调用此方法进行初始化。
        /// </remarks>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="masterConnStr"/>为<c>null</c>时引发此异常。</exception>
        /// <exception cref="ArgumentException">当<paramref name="masterConnStr"/>不包含任何元素或<paramref name="masterConnStr"/>、<paramref name="slaveConnStr"/>包含<c>null</c>、Unicode空白字符时引发此异常。</exception>
        public static void Initialize(string[] masterConnStr, string[] slaveConnStr = null)
        {
            if (masterConnStr == null) throw new ArgumentNullException(nameof(masterConnStr));
            if (masterConnStr.Length == 0) throw new ArgumentException(Properties.Localization.please_specify_the_main_database_connection_string, nameof(masterConnStr));

            if (slaveConnStr == null || slaveConnStr.Length == 0)
            {
                __allConnStr = new string[masterConnStr.Length];
                __masterConnStr = new string[masterConnStr.Length];
                Array.Copy(masterConnStr, __masterConnStr, masterConnStr.Length);
                Array.Copy(masterConnStr, __allConnStr, masterConnStr.Length);
                __slaveConnStr = null;
            }
            else
            {
                __allConnStr = new string[masterConnStr.Length + slaveConnStr.Length];
                __masterConnStr = new string[masterConnStr.Length];
                __slaveConnStr = new string[slaveConnStr.Length];
                Array.Copy(masterConnStr, __masterConnStr, masterConnStr.Length);
                Array.Copy(slaveConnStr, __slaveConnStr, slaveConnStr.Length);

                Array.Copy(masterConnStr, __allConnStr, masterConnStr.Length);
                Array.Copy(slaveConnStr, 0, __allConnStr, masterConnStr.Length, slaveConnStr.Length);
            }

            for (int i = 0; i < __allConnStr.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(__allConnStr[i]))
                {
                    if (i < masterConnStr.Length)
                    {
                        throw new ArgumentException(String.Format(Properties.Localization.the_element_of_the_master_database_connection_string_index_XXX_is_a_null_or_empty_string, i), nameof(masterConnStr));
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(Properties.Localization.the_element_of_the_slave_database_connection_string_index_XXX_is_a_null_or_empty_string, i - masterConnStr.Length), nameof(slaveConnStr));
                    }
                }
            }
        }


        /// <summary>
        /// 构造函数，创建数据库操作对象实例。
        /// </summary>
        public RelationalDBUtilityBase()
        {
            if (__masterConnStr == null) throw new InvalidOperationException(String.Format(Properties.Localization.please_call_the_static_method_XXX_to_initialize_before_constructing_the_instance, nameof(Initialize)));

            NextServer();
        }



        /// <summary>
        /// 一条SQL语句中IN运算符中最多放置多少个值。值的多数量取决于是否能使用索引和整体SQL语句有没有超过最大长度。
        /// </summary>
        protected virtual int SqlInParameterMaximumLength => 50;



        #region IRelationalDBUtility interface implementation.
        public virtual DatabaseServerRole TargetRole
        {
            get => _targetRole;
            set
            {
                if (Enum.IsDefined(typeof(DatabaseServerRole), value))
                {
                    _targetRole = value;

                    if ((_targetRole == DatabaseServerRole.MasterOrSlave && _allConnStrIndex == -1)
                        || (_targetRole == DatabaseServerRole.Slave && _slaveConnStrIndex == -1))
                    {
                        NextServer();
                    }
                }
            }
        }

        public int ServerIndex
        {
            get
            {
                if (TargetRole == DatabaseServerRole.Master) return _masterConnStrIndex;
                if (TargetRole == DatabaseServerRole.Slave) return _slaveConnStrIndex;
                if (TargetRole == DatabaseServerRole.MasterOrSlave) return _allConnStrIndex;
                throw new NotSupportedException(String.Format(Properties.Localization.the_value_of_the_property_XXX_is_invalid, nameof(TargetRole)));
            }
            set
            {
                if (TargetRole == DatabaseServerRole.Master)
                {
                    if (value < __masterConnStr.Length)
                    {
                        _masterConnStrIndex = value;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
                else if (TargetRole == DatabaseServerRole.Slave)
                {
                    if (__slaveConnStr != null && value < __slaveConnStr.Length)
                    {
                        _slaveConnStrIndex = value;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
                else if (TargetRole == DatabaseServerRole.MasterOrSlave)
                {
                    if (value < __allConnStr.Length)
                    {
                        _allConnStrIndex = value;
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }


        public void UseModelInstance<T>(T model) where T : class => _modelInstance = model;

        public void UseListInstance<T>(IList<T> list) => _listInstance = list;

        public void UseSetInstance<T>(ISet<T> set) => _setInstance = set;

        public void UseCollectionInstance<T>(ICollection<T> collection) => _collectionInstance = collection;

        public void UseDictionaryInstance<TKey, TValue>(IDictionary<TKey, TValue> dict) => _dictionaryInstance = dict;



        public void BuildUseModelInstance<T>(Func<T> fun) where T : class => _funBuildModelInstance = fun;

        public void BuildUseListInstance<T>(Func<IList<T>> fun) => _funBuildListInstance = fun;

        public void BuildUseSetInstance<T>(Func<ISet<T>> fun) => _funBuildSetInstance = fun;

        public void BuildUseCollectionInstance<T>(Func<ICollection<T>> fun) => _funBuildCollectionInstance = fun;

        public void BuildUseDictionaryInstance<TKey, TValue>(Func<IDictionary<TKey, TValue>> fun) => _funBuildDictionaryInstance = fun;




        public abstract string SafeHandler(string value);



        public abstract string WildcardEscape(string value);



        public virtual string[] GetSqlInExpressions(IEnumerable<string> parameters)
        {
            return GetSqlInExpressions(parameters, out _);
        }

        public virtual string[] GetSqlInExpressions(IEnumerable<string> parameters, out int parameterCount)
        {
            parameterCount = parameters.Count();

            int arrLength = parameterCount / SqlInParameterMaximumLength;

            if (parameterCount % SqlInParameterMaximumLength != 0)
            {
                arrLength++;
            }

            int i = 1;
            int arrIndex = 0;
            string[] result = new string[arrLength];
            StringBuilder strBuilder = new StringBuilder(1024);

            foreach (var p in parameters)
            {
                strBuilder.Append('\'');
                strBuilder.Append(SafeHandler(p));
                strBuilder.Append('\'');

                if (i >= SqlInParameterMaximumLength)
                {
                    result[arrIndex++] = strBuilder.ToString();
                    strBuilder.Clear();
                    i = 1;
                    continue;
                }

                i++;
                strBuilder.Append(',');
            }

            if (i > 1)
            {
                strBuilder.Length -= 1;
                result[arrIndex] = strBuilder.ToString();
                strBuilder.Clear();
            }

            return result;
        }


        public virtual string[] GetSqlInExpressions<T>(IEnumerable<T> parameters) where T : struct
        {
            return GetSqlInExpressions<T>(parameters, out _);
        }

        public virtual string[] GetSqlInExpressions<T>(IEnumerable<T> parameters, out int parameterCount) where T : struct
        {
            parameterCount = parameters.Count();

            int arrLength = parameterCount / SqlInParameterMaximumLength;

            if (parameterCount % SqlInParameterMaximumLength != 0)
            {
                arrLength++;
            }

            int i = 1;
            int arrIndex = 0;
            string[] result = new string[arrLength];
            StringBuilder strBuilder = new StringBuilder(1024);
            Type tType = typeof(T);
            bool needQuotes = tType == typeof(Guid) || tType == typeof(DateTime);

            foreach (var p in parameters)
            {
                if (needQuotes)
                {
                    strBuilder.Append('\'');
                    strBuilder.Append(p.ToString());
                    strBuilder.Append('\'');
                }
                else
                {
                    if (tType.IsEnum)
                    {
                        strBuilder.Append(Convert.ChangeType(p, tType.GetEnumUnderlyingType()));
                    }
                    else
                    {
                        strBuilder.Append(p.ToString());
                    }

                }

                if (i >= SqlInParameterMaximumLength)
                {
                    result[arrIndex++] = strBuilder.ToString();
                    strBuilder.Clear();
                    i = 1;
                    continue;
                }

                i++;
                strBuilder.Append(',');
            }

            if (i > 1)
            {
                strBuilder.Length -= 1;
                result[arrIndex] = strBuilder.ToString();
                strBuilder.Clear();
            }

            return result;
        }


        public virtual void Appoint(DatabaseServerRole serverRole, int index)
        {
            TargetRole = serverRole;
            ServerIndex = index;
        }


        /// <summary>
        /// 根据<see cref="TargetRole"/>属性的值将<see cref="_allConnStrIndex"/>或<see cref="_masterConnStrIndex"/>或<see cref="_slaveConnStrIndex"/>的值设置成下一个数据库连接字符串的索引位置。
        /// </summary>
        public virtual void NextServer()
        {
            if (TargetRole == DatabaseServerRole.Master)
            {
                if (__masterConnStr.Length == 1)
                {
                    if (_masterConnStrIndex != 0) _masterConnStrIndex = 0;
                }
                else
                {
                    _ = Interlocked.Increment(ref __masterConnStrIndex);

                    _masterConnStrIndex = Interlocked.CompareExchange(ref __masterConnStrIndex, 0, __masterConnStr.Length - 1);
                }
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
                if (__slaveConnStr == null || __slaveConnStr.Length == 0) throw new InvalidOperationException(Properties.Localization.the_connection_string_of_the_slave_database_is_not_specified);

                if (__slaveConnStr.Length == 1)
                {
                    if (_slaveConnStrIndex != 0) _slaveConnStrIndex = 0;
                }
                else
                {
                    _ = Interlocked.Increment(ref __slaveConnStrIndex);
                    _slaveConnStrIndex = Interlocked.CompareExchange(ref __slaveConnStrIndex, 0, __slaveConnStr.Length - 1);
                }
            }
            else
            {
                if (__allConnStr.Length == 1)
                {
                    if (_allConnStrIndex != 0) _allConnStrIndex = 0;
                }
                else
                {
                    _ = Interlocked.Increment(ref __allConnStrIndex);
                    _allConnStrIndex = Interlocked.CompareExchange(ref __allConnStrIndex, 0, __allConnStr.Length - 1);
                }
            }
        }


        public abstract IDbConnection BuildConnection();


        public abstract IDbCommand BuildCommand();

        public abstract IDbCommand BuildCommand(IDbConnection conn);



        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns>数据库事务对象实例。</returns>
        public abstract IDbTransaction BuildTransaction();
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns>数据库事务对象实例。</returns>
        public abstract IDbTransaction BuildTransaction(IsolationLevel level);

#if !NETSTANDARD2_0
        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public abstract Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        public abstract Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default);
#endif //!NETSTANDARD2_0



        public abstract IDbDataParameter BuildParameter();

        public abstract IDbDataParameter BuildParameter(string parameterName, object value);



        public virtual int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(BuildConnection(), null, cmdType, cmdText, parameters);
        }

        public virtual int ExecuteNonQuery(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(tran.Connection, tran, cmdType, cmdText, parameters);
        }

        public virtual int ExecuteNonQuery(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteNonQuery(conn, null, cmdType, cmdText, parameters);
        }

#if NETSTANDARD2_0
        public Task<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken);
        }

#if NETSTANDARD2_0
        public Task<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(null, tran, cmdType, cmdText, parameters, cancellationToken);
        }

#if NETSTANDARD2_0
        public Task<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(conn, null, cmdType, cmdText, parameters, cancellationToken);
        }



        public virtual IDataReader GetReader(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteReader(BuildConnection(), null, cmdType, cmdText, parameters, SqlConnectionOwnership.Internal);
        }

        public virtual IDataReader GetReader(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteReader(tran.Connection, tran, cmdType, cmdText, parameters, SqlConnectionOwnership.External);
        }

        public virtual IDataReader GetReader(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ExecuteReader(conn, null, cmdType, cmdText, parameters, SqlConnectionOwnership.External);
        }

        public virtual Task<IDataReader> GetReaderAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(BuildConnection(), null, cmdType, cmdText, parameters, SqlConnectionOwnership.External, cancellationToken);
        }

        public virtual Task<IDataReader> GetReaderAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(conn, null, cmdType, cmdText, parameters, SqlConnectionOwnership.External, cancellationToken);
        }

        public virtual Task<IDataReader> GetReaderAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(null, tran, cmdType, cmdText, parameters, SqlConnectionOwnership.External, cancellationToken);
        }



        public virtual T GetScalar<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(ExecuteScalar(BuildConnection(), null, cmdType, cmdText, parameters));
        }

        public virtual T GetScalar<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(ExecuteScalar(tran.Connection, tran, cmdType, cmdText, parameters));
        }

        public virtual T GetScalar<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(ExecuteScalar(conn, null, cmdType, cmdText, parameters));
        }

        public virtual async Task<T> GetScalarAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken));
        }

        public virtual async Task<T> GetScalarAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken));
        }

        public virtual async Task<T> GetScalarAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken));
        }



        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(cmdText, cmdType, parameters))
            {
                return ReadToDictionary<TKey, TValue>(reader);
            }
        }

        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(tran, cmdText, cmdType, parameters))
            {
                return ReadToDictionary<TKey, TValue>(reader);
            }
        }

        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(conn, cmdText, cmdType, parameters))
            {
                return ReadToDictionary<TKey, TValue>(reader);
            }
        }

        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToDictionaryAsync<TKey, TValue>(reader, cancellationToken);
            }
        }

        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(tran, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToDictionaryAsync<TKey, TValue>(reader, cancellationToken);
            }
        }

        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(conn, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToDictionaryAsync<TKey, TValue>(reader, cancellationToken);
            }
        }



        public virtual T GetSingle<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
        {
            using (var reader = GetReader(cmdText, cmdType, parameters))
            {
                return ReadToModel<T>(reader);
            }
        }

        public virtual T GetSingle<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
        {
            using (var reader = GetReader(tran, cmdText, cmdType, parameters))
            {
                return ReadToModel<T>(reader);
            }
        }

        public virtual T GetSingle<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
        {
            using (var reader = GetReader(conn, cmdText, cmdType, parameters))
            {
                return ReadToModel<T>(reader);
            }
        }

        public virtual async Task<T> GetSingleAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters) where T : class
        {
            using (var reader = (DbDataReader)await GetReaderAsync(cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToModelAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<T> GetSingleAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters) where T : class
        {
            using (var reader = (DbDataReader)await GetReaderAsync(tran, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToModelAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<T> GetSingleAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters) where T : class
        {
            using (var reader = (DbDataReader)await GetReaderAsync(conn, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToModelAsync<T>(reader, cancellationToken);
            }
        }

        public virtual ref T? GetSingle<T>(ref T? structure, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }

        public virtual ref T? GetSingle<T>(ref T? structure, IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }

        public virtual ref T? GetSingle<T>(ref T? structure, IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }



        public virtual bool TryGetSingle<T>(ref T structure, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            throw new NotImplementedException();
        }



        public virtual IList<T> GetList<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(cmdText, cmdType, parameters))
            {
                return ReadToList<T>(reader);
            }
        }

        public virtual IList<T> GetList<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(tran, cmdText, cmdType, parameters))
            {
                return ReadToList<T>(reader);
            }
        }

        public virtual IList<T> GetList<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(conn, cmdText, cmdType, parameters))
            {
                return ReadToList<T>(reader);
            }
        }

        public virtual async Task<IList<T>> GetListAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToListAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<IList<T>> GetListAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(tran, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToListAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<IList<T>> GetListAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(conn, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToListAsync<T>(reader, cancellationToken);
            }
        }



        public virtual ISet<T> GetSet<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(cmdText, cmdType, parameters))
            {
                return ReadToSet<T>(reader);
            }
        }

        public virtual ISet<T> GetSet<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(tran, cmdText, cmdType, parameters))
            {
                return ReadToSet<T>(reader);
            }
        }

        public virtual ISet<T> GetSet<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(conn, cmdText, cmdType, parameters))
            {
                return ReadToSet<T>(reader);
            }
        }

        public virtual async Task<ISet<T>> GetSetAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToSetAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(tran, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToSetAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(conn, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToSetAsync<T>(reader, cancellationToken);
            }
        }



        public virtual ICollection<T> GetCollection<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(cmdText, cmdType, parameters))
            {
                return ReadToCollection<T>(reader);
            }
        }

        public virtual ICollection<T> GetCollection<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(tran, cmdText, cmdType, parameters))
            {
                return ReadToCollection<T>(reader);
            }
        }

        public virtual ICollection<T> GetCollection<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            using (var reader = GetReader(conn, cmdText, cmdType, parameters))
            {
                return ReadToCollection<T>(reader);
            }
        }

        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToCollectionAsync<T>(reader, cancellationToken);
            }
        }


        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(tran, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToCollectionAsync<T>(reader, cancellationToken);
            }
        }

        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken? cancellationToken = null, params IDataParameter[] parameters)
        {
            using (var reader = (DbDataReader)await GetReaderAsync(conn, cmdText, cmdType, cancellationToken, parameters))
            {
                return await ReadToCollectionAsync<T>(reader, cancellationToken);
            }
        }
        #endregion IRelationalDBUtility interface implementation.




        /// <summary>
        /// 执行指定 T-SQL 命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param> 
        /// <returns>返回受影响的记录数</returns>
        protected abstract int ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
        /// <summary>
        /// 异步执行指定 T-SQL 命令。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回受影响的记录数</returns>
#if NETSTANDARD2_0
        protected abstract Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null);
#else
        protected abstract ValueTask<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null);
#endif

        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param> 
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
        protected abstract object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集中的第一行第一列的数据。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回结果集中的第一行第一列的数据。</returns>
        protected abstract Task<object> ExecuteScalarAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken? cancellationToken = null);

        /// <summary> 
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param> 
        /// <returns>返回包含结果集的数据读取器</returns> 
        protected abstract IDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership);
        /// <summary> 
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="transaction">一个有效的事务,或者为<c>null</c></param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param> 
        /// <param name="connectionOwnership">标识数据库连接对象由调用者提供还是有此类或直接或间接子类提供。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns> 
        protected abstract Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, SqlConnectionOwnership connectionOwnership, CancellationToken? cancellationToken = null);





        /// <summary>
        /// 将<see cref="SqlCommand.ExecuteScalar"/>方法的返回的<see cref="Object"/>类型的值转换成<typeparamref name="T"/>类型的值。
        /// </summary>
        /// <typeparam name="T">要转成什么类型。</typeparam>
        /// <param name="obj"><see cref="SqlCommand.ExecuteScalar"/>方法返回的值。</param>
        /// <returns>返回转换成目标类型的值或默认值。</returns>
        protected virtual T ObjectToScalar<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value) return default;

            return (T)InternalConvertUtility.HackType(obj, typeof(T));
        }


        /// <summary>
        /// 将<paramref name="reader"/>[0]作为键、将<paramref name="reader"/>[1]作为值，读取到键值对集合实例里。
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>如果读取器里有数据则返回这些数据的键值对集合。如果没有数据则返回<c>null</c>。</returns>
        protected virtual IDictionary<TKey, TValue> ReadToDictionary<TKey, TValue>(IDataReader reader)
        {
            IDictionary<TKey, TValue> result = null;

            if (reader.Read())
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue>();
                }

                do
                {
                    result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)reader[1]);
                }
                while (reader.Read());
            }
            return result;
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>[0]作为键、将<paramref name="reader"/>[1]作为值，读取到键值对集合实例里。
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则返回这些数据的键值对集合。如果没有数据则返回<c>null</c>。</returns>
        protected virtual async Task<IDictionary<TKey, TValue>> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken? cancellationToken = null)
        {
            IDictionary<TKey, TValue> result = null;

            bool hasCancellationToken = cancellationToken.HasValue;

            if (await (hasCancellationToken ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync()))
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue>();
                }

                do
                {
                    if (hasCancellationToken)
                    {
                        result.Add((TKey)reader[0], await reader.IsDBNullAsync(1, cancellationToken.Value) ? default : (TValue)reader[1]);
                    }
                    else
                    {
                        result.Add((TKey)reader[0], await reader.IsDBNullAsync(1) ? default : (TValue)reader[1]);
                    }

                }
                while (await (hasCancellationToken ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync()));
            }
            return result;
        }


        /// <summary>
        /// 将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
        protected virtual T ReadToModel<T>(IDataReader reader) where T : class
        {
            if (!reader.Read()) return null;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model);
                return model;
            }

            return reader.ToModel<T>();
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
        protected virtual async Task<T> ReadToModelAsync<T>(DbDataReader reader, CancellationToken? cancellationToken) where T : class
        {
            if (!await (cancellationToken.HasValue ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync())) return null;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model);
                return model;
            }

            return reader.ToModel<T>();
        }



        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
        private void FillToCollection<T>(IDataReader reader, ICollection<T> collection)
        {
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == ValueTypeConstants.GuidType)
            {
                T defaultValue = default;
                do
                {
                    do
                    {
                        if (reader.IsDBNull(0))
                        {
                            collection.Add(defaultValue);
                        }
                        else
                        {
                            collection.Add((T)reader[0]);
                        }
                    } while (reader.Read());
                }
                while (reader.NextResult() && reader.Read());
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        reader.ToCollection<T>(collection);
                    }
                    while (reader.NextResult() && reader.Read());
                }
                else
                {
                    Func<T> fun = (Func<T>)_funBuildModelInstance;
                    _funBuildModelInstance = null;
                    do
                    {
                        reader.ToCollection<T>(collection, fun);
                    }
                    while (reader.NextResult() && reader.Read());
                }
            }
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
#if NETSTANDARD2_0
        private async Task FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection)
#else
        private async ValueTask FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection)
#endif
        {
            Task<bool> nextResult;
            Task<bool> readResult;
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == typeof(Guid))
            {
                T defaultValue = default;
                do
                {
                    do
                    {
                        if (await reader.IsDBNullAsync(0))
                        {
                            collection.Add(defaultValue);
                        }
                        else
                        {
                            collection.Add((T)reader[0]);
                        }
                    } while (await reader.ReadAsync());

                    nextResult = reader.NextResultAsync();
                    readResult = reader.ReadAsync();
                }
                while (await nextResult && await readResult);
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection);

                        nextResult = reader.NextResultAsync();
                        readResult = reader.ReadAsync();
                    }
                    while (await nextResult && await readResult);
                }
                else
                {
                    Func<T> fun = (Func<T>)_funBuildModelInstance;
                    _funBuildModelInstance = null;
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection, (CancellationToken?)null, fun);

                        nextResult = reader.NextResultAsync();
                        readResult = reader.ReadAsync();
                    }
                    while (await nextResult && await readResult);
                }
            }
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
#if NETSTANDARD2_0
        private async Task FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection, CancellationToken cancellationToken)
#else
        private async ValueTask FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection, CancellationToken cancellationToken)
#endif
        {
            Task<bool> nextResult;
            Task<bool> readResult;
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == typeof(Guid))
            {
                T defaultValue = default;
                do
                {
                    do
                    {
                        if (await reader.IsDBNullAsync(0, cancellationToken))
                        {
                            collection.Add(defaultValue);
                        }
                        else
                        {
                            collection.Add((T)reader[0]);
                        }
                    } while (await reader.ReadAsync(cancellationToken));

                    nextResult = reader.NextResultAsync(cancellationToken);
                    readResult = reader.ReadAsync(cancellationToken);
                }
                while (await nextResult && await readResult);
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection, cancellationToken);

                        nextResult = reader.NextResultAsync(cancellationToken);
                        readResult = reader.ReadAsync(cancellationToken);
                    }
                    while (await nextResult && await readResult);
                }
                else
                {
                    Func<T> fun = (Func<T>)_funBuildModelInstance;
                    _funBuildModelInstance = null;
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection, cancellationToken, fun);

                        nextResult = reader.NextResultAsync(cancellationToken);
                        readResult = reader.ReadAsync(cancellationToken);
                    }
                    while (await nextResult && await readResult);
                }
            }
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
        protected virtual IList<T> ReadToList<T>(IDataReader reader)
        {
            IList<T> result = null;

            if (reader.Read())
            {
                if (_listInstance != null)
                {
                    result = (IList<T>)_listInstance;
                    _listInstance = null;
                }
                else if (_funBuildListInstance != null)
                {
                    result = ((Func<IList<T>>)_funBuildListInstance)();
                    _funBuildListInstance = null;
                }
                else
                {
                    result = new List<T>();
                }

                FillToCollection<T>(reader, result);
            }
            return result;
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
        protected virtual async Task<IList<T>> ReadToListAsync<T>(DbDataReader reader, CancellationToken? cancellationToken = null)
        {
            IList<T> result = null;

            if (await (cancellationToken.HasValue ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync()))
            {
                if (_listInstance != null)
                {
                    result = (IList<T>)_listInstance;
                    _listInstance = null;
                }
                else if (_funBuildListInstance != null)
                {
                    result = ((Func<IList<T>>)_funBuildListInstance)();
                    _funBuildListInstance = null;
                }
                else
                {
                    result = new List<T>();
                }

                await (cancellationToken.HasValue
                    ? FillToCollectionAsync<T>(reader, result, cancellationToken.Value)
                    : FillToCollectionAsync<T>(reader, result));

            }
            return result;
        }


        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
        protected virtual ISet<T> ReadToSet<T>(IDataReader reader)
        {
            ISet<T> result = null;

            if (reader.Read())
            {
                if (_setInstance != null)
                {
                    result = (ISet<T>)_setInstance;
                    _setInstance = null;
                }
                else if (_funBuildSetInstance != null)
                {
                    result = ((Func<ISet<T>>)_funBuildSetInstance)();
                    _funBuildSetInstance = null;
                }
                else
                {
                    result = new HashSet<T>();
                }

                FillToCollection<T>(reader, result);
            }
            return result;
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
        protected virtual async Task<ISet<T>> ReadToSetAsync<T>(DbDataReader reader, CancellationToken? cancellationToken)
        {
            ISet<T> result = null;

            if (await (cancellationToken.HasValue ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync()))
            {
                if (_setInstance != null)
                {
                    result = (ISet<T>)_setInstance;
                    _setInstance = null;
                }
                else if (_funBuildSetInstance != null)
                {
                    result = ((Func<ISet<T>>)_funBuildSetInstance)();
                    _funBuildSetInstance = null;
                }
                else
                {
                    result = new HashSet<T>();
                }

                await (cancellationToken.HasValue
                    ? FillToCollectionAsync<T>(reader, result, cancellationToken.Value)
                    : FillToCollectionAsync<T>(reader, result));
            }
            return result;
        }


        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
        protected virtual ICollection<T> ReadToCollection<T>(IDataReader reader)
        {
            ICollection<T> result = null;

            if (reader.Read())
            {
                if (_collectionInstance != null)
                {
                    result = (ICollection<T>)_collectionInstance;
                    _collectionInstance = null;
                }
                else if (_funBuildCollectionInstance != null)
                {
                    result = ((Func<ICollection<T>>)_funBuildCollectionInstance)();
                    _funBuildCollectionInstance = null;
                }
                else
                {
                    result = new List<T>();
                }

                FillToCollection<T>(reader, result);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
        protected virtual async Task<ICollection<T>> ReadToCollectionAsync<T>(DbDataReader reader, CancellationToken? cancellationToken)
        {
            ICollection<T> result = null;

            if (await (cancellationToken.HasValue ? reader.ReadAsync(cancellationToken.Value) : reader.ReadAsync()))
            {
                if (_collectionInstance != null)
                {
                    result = (ICollection<T>)_collectionInstance;
                    _collectionInstance = null;
                }
                else if (_funBuildCollectionInstance != null)
                {
                    result = ((Func<ICollection<T>>)_funBuildCollectionInstance)();
                    _funBuildCollectionInstance = null;
                }
                else
                {
                    result = new List<T>();
                }

                await (cancellationToken.HasValue
                    ? FillToCollectionAsync<T>(reader, result, cancellationToken.Value)
                    : FillToCollectionAsync<T>(reader, result));
            }
            return result;
        }
    }
}
