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
    using System.Collections;
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
        /// 辅助数据库服务器之间实现轮询的类。
        /// 用于记录当前使用的数据库连接字符串。
        /// </summary>
        /// <remarks>
        /// 对同一个数据库的所有数据库操作对象实例都应该共享此对象的同一个实例，这样才是实现轮询的效果。
        /// </remarks>
        public class ServersPolling
        {
            /// <summary>
            /// 所有数据库链接（主数据库和从数据库）的连接字符串集合的索引。
            /// </summary>
            public int _allConnStrIndex = -1;
            /// <summary>
            /// 主数据库链接字符串集合的索引。
            /// </summary>
            public int _masterConnStrIndex = -1;
            /// <summary>
            /// 从数据库链接字符串集合的索引。
            /// </summary>
            public int _slaveConnStrIndex = -1;
        }

        /// <summary>
        /// 存放主库连接字符串和从库连接字符串，主库连接字符串放在前。
        /// </summary>
        protected string[] _allConnStr;
        /// <summary>
        /// 存放主库连接字符串。
        /// </summary>
        protected string[] _masterConnStr;
        /// <summary>
        /// 存放从库连接字符串。
        /// </summary>
#if NETSTANDARD2_0
        protected string[] _slaveConnStr;
#else
        protected string[]? _slaveConnStr;
#endif //NETSTANDARD2_0
        /*
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected bool[] _usableAllConnStr;
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected bool[] _usableMasterConnStr;
        /// <summary>
        /// 表示数据库连接字符串指向的数据库服务器现在是否可用。
        /// </summary>
        protected bool[] _usableSlaveConnStr;
        */

#if NETSTANDARD2_0
        readonly ServersPolling _polling;
#else
        readonly ServersPolling? _polling;
#endif
        /// <summary>
        /// 所有数据库链接（主数据库和从数据库）的连接字符串集合的索引。
        /// </summary>
        protected int _allConnStrIndex = 0;
        /// <summary>
        /// 主数据库链接字符串集合的索引。
        /// </summary>
        protected int _masterConnStrIndex = 0;
        /// <summary>
        /// 从数据库链接字符串集合的索引。
        /// </summary>
        protected int _slaveConnStrIndex = 0;

#if NETSTANDARD2_0
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
#else
        protected object? _modelInstance = null;              //model object.
        protected object? _listInstance = null;               //IList<T>
        protected object? _setInstance = null;                //ISet<T>
        protected object? _collectionInstance = null;         //ICollection<T>
        protected object? _dictionaryInstance = null;         //IDictionary<TKey, TValue>

        protected object? _funBuildModelInstance = null;      //Func<T>
        protected object? _funBuildListInstance = null;       //Func<T>
        protected object? _funBuildSetInstance = null;        //Func<T>
        protected object? _funBuildCollectionInstance = null; //Func<T>
        protected object? _funBuildDictionaryInstance = null; //Func<T>
#endif //NETSTANDARD2_0

        DatabaseServerRole _targetRole;

        /// <summary>
        /// 创建数据库操作对象实例。
        /// </summary>
        /// <param name="connStr">数据库链接字符串。</param>
        public RelationalDBUtilityBase(string connStr)
        {
            if (String.IsNullOrWhiteSpace(connStr)) throw new ArgumentNullException(nameof(connStr));

            _polling = null;
            _slaveConnStr = null;
            _targetRole = DatabaseServerRole.Master;
            _masterConnStr = _allConnStr = new string[] { connStr };
        }

        /// <summary>
        /// 创建数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="masterConnStr"/>为<c>null</c>时引发此异常。</exception>
        /// <exception cref="ArgumentException">当<paramref name="masterConnStr"/>不包含任何元素或<paramref name="masterConnStr"/>、<paramref name="slaveConnStr"/>包含<c>null</c>、Unicode空白字符时引发此异常。</exception>
#if NETSTANDARD2_0
        public RelationalDBUtilityBase(ServersPolling polling, string[] masterConnStr, string[] slaveConnStr)
#else
        public RelationalDBUtilityBase(ServersPolling polling, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
            : this(polling, DatabaseServerRole.Master, masterConnStr, slaveConnStr)
        {

        }

        /// <summary>
        /// 创建数据库操作对象实例。
        /// </summary>
        /// <param name="polling">表示应使用的数据库连接字符串索引，辅助实现数据库服务器轮询。</param>
        /// <param name="role">要使用什么角色的数据库服务器。</param>
        /// <param name="masterConnStr">主数据库服务器链接字符串集合。</param>
        /// <param name="slaveConnStr">从数据库服务器链接字符串集合。</param>
        /// <exception cref="ArgumentNullException">当<paramref name="masterConnStr"/>为<c>null</c>时引发此异常。</exception>
        /// <exception cref="ArgumentException">当<paramref name="masterConnStr"/>不包含任何元素或<paramref name="masterConnStr"/>、<paramref name="slaveConnStr"/>包含<c>null</c>、Unicode空白字符时引发此异常。</exception>
#if NETSTANDARD2_0
        public RelationalDBUtilityBase(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[] slaveConnStr)
#else
        public RelationalDBUtilityBase(ServersPolling polling, DatabaseServerRole role, string[] masterConnStr, string[]? slaveConnStr)
#endif //NETSTANDARD2_0
        {
            _polling = polling ?? throw new ArgumentNullException(nameof(polling));
            if (masterConnStr == null) throw new ArgumentNullException(nameof(masterConnStr));
            if (masterConnStr.Length == 0) throw new ArgumentException(Properties.Localization.please_specify_the_main_database_connection_string, nameof(masterConnStr));

            if (slaveConnStr == null || slaveConnStr.Length == 0)
            {
                _masterConnStr = new string[masterConnStr.Length];
                Array.Copy(masterConnStr, _masterConnStr, masterConnStr.Length);
                _allConnStr = _masterConnStr;
                _slaveConnStr = null;
            }
            else
            {
                _allConnStr = new string[masterConnStr.Length + slaveConnStr.Length];
                _masterConnStr = new string[masterConnStr.Length];
                _slaveConnStr = new string[slaveConnStr.Length];
                Array.Copy(masterConnStr, _masterConnStr, masterConnStr.Length);
                Array.Copy(slaveConnStr, _slaveConnStr, slaveConnStr.Length);

                Array.Copy(masterConnStr, _allConnStr, masterConnStr.Length);
                Array.Copy(slaveConnStr, 0, _allConnStr, masterConnStr.Length, slaveConnStr.Length);
            }

            for (int i = 0; i < _allConnStr.Length; i++)
            {
                if (String.IsNullOrWhiteSpace(_allConnStr[i]))
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

            TargetRole = role;
        }

        /// <summary>
        /// 一条SQL语句中IN运算符中最多放置多少个值。值的多数量取决于是否能使用索引和整体SQL语句有没有超过最大长度。
        /// </summary>
        public virtual int SqlInParameterMaximumLength => 50;



        #region IRelationalDBUtility interface implementation.
        /// <summary>
        /// 表示遍历参数集合时，当遇到某个元素为 <c>null</c>时的处理行为。true：停止遍历；false：跳过当前元素继续遍历。默认值：true。
        /// </summary>
        public bool BreakWhenParametersElementIsNull { get; set; } = true;

        public virtual DatabaseServerRole TargetRole
        {
            get => _targetRole;
            set
            {
                if (!Enum.IsDefined(typeof(DatabaseServerRole), value)) throw new ArgumentException(String.Format(Properties.Localization.invalid_value_XXX, value));

                if (_targetRole != value)
                {
                    _targetRole = value;

                    NextServer();
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
            protected set
            {
                if (TargetRole == DatabaseServerRole.Master)
                {
                    if (value > -1 && value < _masterConnStr.Length)
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
                    if (_slaveConnStr != null && value > -1 && value < _slaveConnStr.Length)
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
                    if (value > -1 && value < _allConnStr.Length)
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


        public virtual string GetSqlInExpressions(byte bitFields)
        {
            return GetSqlInExpressions(bitFields, null, out _);
        }

        public virtual string GetSqlInExpressions(byte bitFields, out int parameterCount)
        {
            return GetSqlInExpressions(bitFields, null, out parameterCount);
        }

#if NETSTANDARD2_0
        public virtual string GetSqlInExpressions(byte bitFields, byte[] values, out int parameterCount)
#else
        public virtual string GetSqlInExpressions(byte bitFields, byte[]? values, out int parameterCount)
#endif
        {
            StringBuilder strBuilder = new StringBuilder(21); //"1,2,4,8,16,32,64,128,".Length = 21
            parameterCount = 0;

            if (values == null || values.Length == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    int value = 1 << i;
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }
            else
            {
                foreach (var value in values)
                {
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }

            if (parameterCount > 0)
            {
                return strBuilder.ToString(0, strBuilder.Length - 1);
            }

            return String.Empty;
        }

        public virtual string GetSqlInExpressions(ushort bitFields)
        {
            return GetSqlInExpressions(bitFields, null, out _);
        }

        public virtual string GetSqlInExpressions(ushort bitFields, out int parameterCount)
        {
            return GetSqlInExpressions(bitFields, null, out parameterCount);
        }

#if NETSTANDARD2_0
        public virtual string GetSqlInExpressions(ushort bitFields, ushort[] values, out int parameterCount)
#else
        public virtual string GetSqlInExpressions(ushort bitFields, ushort[]? values, out int parameterCount)
#endif
        {
            StringBuilder strBuilder = new StringBuilder(61); //"1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768," = 61
            parameterCount = 0;

            if (values == null || values.Length == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    int value = 1 << i;
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }
            else
            {
                foreach (var value in values)
                {
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }

            if (parameterCount > 0)
            {
                return strBuilder.ToString(0, strBuilder.Length - 1);
            }

            return String.Empty;
        }

        public virtual string GetSqlInExpressions(uint bitFields)
        {
            return GetSqlInExpressions(bitFields, null, out _);
        }

        public virtual string GetSqlInExpressions(uint bitFields, out int parameterCount)
        {
            return GetSqlInExpressions(bitFields, null, out parameterCount);
        }

#if NETSTANDARD2_0
        public virtual string GetSqlInExpressions(uint bitFields, uint[] values, out int parameterCount)
#else
        public virtual string GetSqlInExpressions(uint bitFields, uint[]? values, out int parameterCount)
#endif
        {
            StringBuilder strBuilder = new StringBuilder(199); //"1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768,65536,131072,262144,524288,1048576,2097152,4194304,8388608,16777216,33554432,67108864,134217728,268435456,536870912,1073741824,2147483648," = 199
            parameterCount = 0;

            if (values == null || values.Length == 0)
            {
                for (int i = 0; i < 32; i++)
                {
                    uint value = 1U << i;
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }
            else
            {
                foreach (var value in values)
                {
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }

            if (parameterCount > 0)
            {
                return strBuilder.ToString(0, strBuilder.Length - 1);
            }

            return String.Empty;
        }

        public virtual string GetSqlInExpressions(ulong bitFields)
        {
            return GetSqlInExpressions(bitFields, null, out _);
        }

        public virtual string GetSqlInExpressions(ulong bitFields, out int parameterCount)
        {
            return GetSqlInExpressions(bitFields, null, out parameterCount);
        }

#if NETSTANDARD2_0
        public virtual string GetSqlInExpressions(ulong bitFields, ulong[] values, out int parameterCount)
#else
        public virtual string GetSqlInExpressions(ulong bitFields, ulong[]? values, out int parameterCount)
#endif
        {
            StringBuilder strBuilder = new StringBuilder(704); //"1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768,65536,131072,262144,524288,1048576,2097152,4194304,8388608,16777216,33554432,67108864,134217728,268435456,536870912,1073741824,2147483648,4294967296,8589934592,17179869184,34359738368,68719476736,137438953472,274877906944,549755813888,1099511627776,2199023255552,4398046511104,8796093022208,17592186044416,35184372088832,70368744177664,140737488355328,281474976710656,562949953421312,1125899906842624,2251799813685248,4503599627370496,9007199254740992,18014398509481984,36028797018963968,72057594037927936,144115188075855872,288230376151711744,576460752303423488,1152921504606846976,2305843009213693952,4611686018427387904,9223372036854775808," = 704
            parameterCount = 0;

            if (values == null || values.Length == 0)
            {
                for (int i = 0; i < 64; i++)
                {
                    ulong value = 1UL << i;
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }
            else
            {
                foreach (var value in values)
                {
                    if ((bitFields & value) == value)
                    {
                        parameterCount++;
                        strBuilder.Append(value);
                        strBuilder.Append(',');
                    }
                }
            }

            if (parameterCount > 0)
            {
                return strBuilder.ToString(0, strBuilder.Length - 1);
            }

            return String.Empty;
        }


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
                if (_masterConnStr.Length == 1)
                {
                    if (_masterConnStrIndex != 0) _masterConnStrIndex = 0;
                }
                else
                {
#if NETSTANDARD2_0
                    _masterConnStrIndex = Interlocked.Increment(ref _polling._masterConnStrIndex);
#else
                    _masterConnStrIndex = Interlocked.Increment(ref _polling!._masterConnStrIndex);
#endif
                    _ = Interlocked.CompareExchange(ref _polling._masterConnStrIndex, -1, _masterConnStr.Length - 1);
                }
            }
            else if (TargetRole == DatabaseServerRole.Slave)
            {
                if (_slaveConnStr == null || _slaveConnStr.Length == 0) throw new InvalidOperationException(Properties.Localization.the_connection_string_of_the_slave_database_is_not_specified);

                if (_slaveConnStr.Length == 1)
                {
                    if (_slaveConnStrIndex != 0) _slaveConnStrIndex = 0;
                }
                else
                {
#if NETSTANDARD2_0
                    _slaveConnStrIndex = Interlocked.Increment(ref _polling._slaveConnStrIndex);
#else
                    _slaveConnStrIndex = Interlocked.Increment(ref _polling!._slaveConnStrIndex);
#endif
                    _ = Interlocked.CompareExchange(ref _polling._slaveConnStrIndex, -1, _slaveConnStr.Length - 1);
                }
            }
            else
            {
                if (_allConnStr.Length == 1)
                {
                    if (_allConnStrIndex != 0) _allConnStrIndex = 0;
                }
                else
                {
#if NETSTANDARD2_0
                    _allConnStrIndex = Interlocked.Increment(ref _polling._allConnStrIndex);
#else
                    _allConnStrIndex = Interlocked.Increment(ref _polling!._allConnStrIndex);
#endif
                    _ = Interlocked.CompareExchange(ref _polling._allConnStrIndex, -1, _allConnStr.Length - 1);
                }
            }
        }


        public abstract IDbConnection BuildConnection();

        public abstract IDbConnection BuildConnection(string connectionString);

        public abstract IDbConnection BuildConnectionAndOpen();

        public abstract IDbConnection BuildConnectionAndOpen(string connectionString);

        public abstract Task<IDbConnection> BuildConnectionAndOpenAsync();

        public abstract Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString);

        public abstract Task<IDbConnection> BuildConnectionAndOpenAsync(CancellationToken cancellationToken = default);

        public abstract Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default);


        public abstract IDbCommand BuildCommand();

        public abstract IDbCommand BuildCommand(IDbConnection conn);



        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public abstract IDbTransaction BuildTransaction();
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public abstract IDbTransaction BuildTransaction(IsolationLevel level);

        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public abstract Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        [Obsolete("这个方法即将在新的版本中被移除。因为使用词方法极为容易导致数据库链接未关闭的情况。除非您主动从事务对象中获取链接对象并确保在不使用时关闭。")]
        public abstract Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default);


        /// <summary>
        /// 创建一个参数对象实例。
        /// </summary>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        public abstract IDbDataParameter BuildParameter();
        /// <summary>
        /// 使用指定参数名和参数值创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数的值。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public abstract IDbDataParameter BuildParameter(string parameterName, object value);
#else
        public abstract IDbDataParameter BuildParameter(string parameterName, object? value);
#endif
        /// <summary>
        /// 使用指定参数名、类型和大小创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        public abstract IDbDataParameter BuildParameter(string parameterName, int parameterType, int size);  //如果给 size 赋默认值就会导致 参数值为 int 类型时把值当作参数类型。
        /// <summary>
        /// 使用指定参数名、类型、大小和方向创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        public abstract IDbDataParameter BuildParameter(string parameterName, ParameterDirection direction, int parameterType, int size = default);
        /// <summary>
        /// 使用指定参数名、参数值、类型和大小创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public abstract IDbDataParameter BuildParameter(string parameterName, object value, int parameterType, int size = default);
#else
        public abstract IDbDataParameter BuildParameter(string parameterName, object? value, int parameterType, int size = default);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 使用指定参数名、参数值、类型、大小和方向创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public abstract IDbDataParameter BuildParameter(string parameterName, object value, ParameterDirection direction, int parameterType, int size = default);
#else
        public abstract IDbDataParameter BuildParameter(string parameterName, object? value, ParameterDirection direction, int parameterType, int size = default);
#endif //NETSTANDARD2_0



        /// <summary>
        /// 将参数对象的参数名、参数值和方向重置为指定的值，类型和大小随着<paramref name="value"/>的类型默认重置。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
#if NETSTANDARD2_0
        public abstract void ResetParameter(IDbDataParameter parameter, string parameterName, object value, ParameterDirection direction = ParameterDirection.Input);
#else
        public abstract void ResetParameter(IDbDataParameter parameter, string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input);
#endif //NETSTANDARD2_0

        /// <summary>
        /// 将参数对象的参数名、参数值、类型、大小和方向重置为指定的值。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <param name="direction">参数方向。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public abstract void ResetParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#else
        public abstract void ResetParameter(IDbDataParameter parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#endif //NETSTANDARD2_0

        /// <summary>
        /// 如果<paramref name="parameter"/>不为<c>null</c>则将参数对象的参数名、参数值和方向重置为指定的值（类型和大小随着<paramref name="value"/>的类型默认重置），否则用这些参数创建一个新的参数对象。
        /// </summary>
        /// <param name="parameter">参数对象。</param>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="direction">参数方向。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        public virtual IDbDataParameter ResetOrBuildParameter(IDbDataParameter parameter, string parameterName, object value, ParameterDirection direction = ParameterDirection.Input)
#else
        public virtual IDbDataParameter ResetOrBuildParameter(IDbDataParameter? parameter, string parameterName, object? value, ParameterDirection direction = ParameterDirection.Input)
#endif //NETSTANDARD2_0
        {
            if (parameter is null)
            {
                parameter = BuildParameter(parameterName, value);

                if (parameter.Direction != direction) parameter.Direction = direction;
            }
            else
            {
                ResetParameter(parameter, parameterName, value, direction);
            }

            return parameter;
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
        public abstract IDbDataParameter ResetOrBuildParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#else
        public abstract IDbDataParameter ResetOrBuildParameter(IDbDataParameter? parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#endif //NETSTANDARD2_0



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
        public Task<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken);
        }

#if NETSTANDARD2_0
        public Task<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(null, tran, cmdType, cmdText, parameters, cancellationToken);
        }

#if NETSTANDARD2_0
        public Task<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public ValueTask<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif
        {
            return ExecuteNonQueryAsync(conn, null, cmdType, cmdText, parameters, cancellationToken);
        }



        public virtual IDataReader GetReader(string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, params IDataParameter[] parameters)
        {
            return ExecuteReader(BuildConnection(), commandBehavior | CommandBehavior.CloseConnection, cmdType, cmdText, parameters);
        }

        public virtual IDataReader GetReader(string cmdText, CommandType cmdType, CommandBehavior commandBehavior, out IDbCommand command, params IDataParameter[] parameters)
        {
            return ExecuteReader(BuildConnection(), commandBehavior | CommandBehavior.CloseConnection, cmdType, cmdText, parameters, out command);
        }

        public virtual IDataReader GetReader(IDbCommand command, CommandBehavior commandBehavior = CommandBehavior.Default, params IDataParameter[] parameters)
        {
            return ExecuteReader(command, parameters, commandBehavior);
        }



        public virtual IDataReader GetReader(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, params IDataParameter[] parameters)
        {
            return ExecuteReader(conn, commandBehavior, cmdType, cmdText, parameters);
        }

        public virtual IDataReader GetReader(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, params IDataParameter[] parameters)
        {
            return ExecuteReader(tran, commandBehavior, cmdType, cmdText, parameters);
        }

        public virtual IDataReader GetReader(IDbConnection conn, string cmdText, CommandType cmdType, CommandBehavior commandBehavior, out IDbCommand command, params IDataParameter[] parameters)
        {
            return ExecuteReader(conn, commandBehavior, cmdType, cmdText, parameters, out command);
        }

        public virtual IDataReader GetReader(IDbTransaction tran, string cmdText, CommandType cmdType, CommandBehavior commandBehavior, out IDbCommand command, params IDataParameter[] parameters)
        {
            return ExecuteReader(tran, commandBehavior, cmdType, cmdText, parameters, out command);
        }

        public virtual Task<IDataReader> GetReaderAsync(string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(BuildConnection(), commandBehavior | CommandBehavior.CloseConnection, cmdType, cmdText, parameters, cancellationToken);
        }

        public virtual Task<IDataReader> GetReaderAsync(IDbCommand command, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(command, parameters, commandBehavior, cancellationToken);
        }

        public virtual Task<IDataReader> GetReaderAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(conn, commandBehavior, cmdType, cmdText, parameters, cancellationToken);
        }

        public virtual Task<IDataReader> GetReaderAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAsync(tran, commandBehavior, cmdType, cmdText, parameters, cancellationToken);
        }

        public virtual Task<(IDataReader, IDbCommand)> GetReaderAndOutParametersAsync(string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAndGetCommandAsync(BuildConnection(), commandBehavior | CommandBehavior.CloseConnection, cmdType, cmdText, parameters, cancellationToken);
        }

        public virtual Task<(IDataReader, IDbCommand)> GetReaderAndOutParametersAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAndGetCommandAsync(conn, commandBehavior, cmdType, cmdText, parameters, cancellationToken);
        }

        public virtual Task<(IDataReader, IDbCommand)> GetReaderAndOutParametersAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ExecuteReaderAndGetCommandAsync(tran, commandBehavior, cmdType, cmdText, parameters, cancellationToken);
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

        public virtual async Task<T> GetScalarAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(BuildConnection(), null, cmdType, cmdText, parameters, cancellationToken));
        }

        public virtual async Task<T> GetScalarAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(tran.Connection, tran, cmdType, cmdText, parameters, cancellationToken));
        }

        public virtual async Task<T> GetScalarAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            return ObjectToScalar<T>(await ExecuteScalarAsync(conn, null, cmdType, cmdText, parameters, cancellationToken));
        }



#if NETSTANDARD2_0
        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToDictionary<TKey, TValue>(reader);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToDictionary<TKey, TValue>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        public virtual IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToDictionary<TKey, TValue>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual async Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual async Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToDictionaryAsync<TKey, TValue>(reader, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual async Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual async Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                    
                return await ReadToDictionaryAsync<TKey, TValue>((DbDataReader)reader, cancellationToken);
                
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="conn"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        public virtual async Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#elif NETSTANDARD2_1
        public virtual async Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#else
        public virtual async Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
            where TKey : notnull
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                    
                return await ReadToDictionaryAsync<TKey, TValue>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual T GetSingle<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    return ReadToModel<T>(reader);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual T GetSingle<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    return ReadToModel<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual T GetSingle<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    return ReadToModel<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual T GetSingle<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#else
        public virtual T? GetSingle<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToModel<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToModelAsync<T>(reader, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);
                    
                return await ReadToModelAsync<T>((DbDataReader)reader, cancellationToken);
                
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);    
                return await ReadToModelAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }




#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToModelAsync<T>(reader, ignoreFieldName, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToModelAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);

            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);
                return await ReadToModelAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }




#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToModelAsync<T>(reader, ignoreFieldNames, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
#endif //NETSTANDARD2_0
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToModelAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);

            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<T> GetSingleAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<T?> GetSingleAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, cancellationToken);
                return await ReadToModelAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }


        public virtual bool TryGetSingle<T>(ref T structure, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



        public virtual bool TryGetSingle<T>(ref T structure, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldName, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldName, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldName, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



        public virtual bool TryGetSingle<T>(ref T structure, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldNames, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldNames, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

        public virtual bool TryGetSingle<T>(ref T structure, IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct
        {
#if NETSTANDARD2_0
            IDbCommand cmd = null;
#else
            IDbCommand? cmd = null;
#endif
            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleRow, cmdType, cmdText, parameters, out cmd))
                {
                    if (reader.Read())
                    {
                        reader.ToModel(ref structure, ignoreFieldNames, customAssignMethod);
                        return true;
                    }
                    return false;
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#else
        public virtual IList<T>? GetList<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#endif //NETSTANDARD2_0
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToList<T>(reader);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#else
        public virtual IList<T>? GetList<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#endif //NETSTANDARD2_0
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToList<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#else
        public virtual IList<T>? GetList<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
#endif //NETSTANDARD2_0
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToList<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual IList<T> GetList<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual IList<T>? GetList<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToList<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToListAsync<T>(reader, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                return await ReadToListAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;

#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                return await ReadToListAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToListAsync<T>(reader, ignoreFieldName, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToListAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;

#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToListAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToListAsync<T>(reader, ignoreFieldNames, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToListAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<IList<T>> GetListAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<IList<T>?> GetListAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;

#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToListAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ISet<T>? GetSet<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToSet<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }



#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ISet<T>? GetSet<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToSet<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }




#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ISet<T>? GetSet<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToSet<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ISet<T> GetSet<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ISet<T>? GetSet<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToSet<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToSetAsync<T>(reader, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                    
                return await ReadToSetAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                return await ReadToSetAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToSetAsync<T>(reader, ignoreFieldName, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToSetAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToSetAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToSetAsync<T>(reader, ignoreFieldNames, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToSetAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ISet<T>> GetSetAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ISet<T>?> GetSetAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToSetAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }



#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ICollection<T>? GetCollection<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToCollection<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }




#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ICollection<T>? GetCollection<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToCollection<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader, ignoreFieldName, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }




#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#else
        public virtual ICollection<T>? GetCollection<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            try
            {
                using (var reader = ExecuteReader(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection))
                {
                    return ReadToCollection<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual ICollection<T> GetCollection<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
#else
        public virtual ICollection<T>? GetCollection<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
#endif //NETSTANDARD2_0

            try
            {
                using (var reader = ExecuteReader(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, out cmd))
                {
                    return ReadToCollection<T>(reader, ignoreFieldNames, customAssignMethod);
                }
            }
            finally
            {
                cmd?.Parameters.Clear();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToCollectionAsync<T>(reader, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                return await ReadToCollectionAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);
                
                return await ReadToCollectionAsync<T>((DbDataReader)reader, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToCollectionAsync<T>(reader, ignoreFieldName, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToCollectionAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbConnection conn, string cmdText, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToCollectionAsync<T>((DbDataReader)reader, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
#endif //NETSTANDARD2_0
        {
            IDbCommand cmd = BuildCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdText;

            try
            {
                using (var reader = (DbDataReader)await ExecuteReaderAsync(cmd, parameters, CommandBehavior.SingleResult | CommandBehavior.CloseConnection, cancellationToken))
                {
                    return await ReadToCollectionAsync<T>(reader, ignoreFieldNames, customAssignMethod, cancellationToken);
                }
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }


#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(tran, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToCollectionAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
            }
        }

#if NETSTANDARD2_0
        public virtual async Task<ICollection<T>> GetCollectionAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand cmd = null;
            IDataReader reader = null;
#else
        public virtual async Task<ICollection<T>?> GetCollectionAsync<T>(IDbConnection conn, string cmdText, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters)
        {
            IDbCommand? cmd = null;
            IDataReader? reader = null;
#endif //NETSTANDARD2_0

            try
            {
                (reader, cmd) = await ExecuteReaderAndGetCommandAsync(conn, CommandBehavior.SingleResult, cmdType, cmdText, parameters, cancellationToken);

                return await ReadToCollectionAsync<T>((DbDataReader)reader, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            finally
            {
                cmd?.Parameters.Clear();
                reader?.Dispose();
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
#if NETSTANDARD2_0
        protected abstract int ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
#else
        protected abstract int ExecuteNonQuery(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
#endif //NETSTANDARD2_0
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
        protected abstract Task<int> ExecuteNonQueryAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);
#else
        protected abstract ValueTask<int> ExecuteNonQueryAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);
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
#if NETSTANDARD2_0
        protected abstract object ExecuteScalar(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
#else
        protected abstract object? ExecuteScalar(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters);
#endif //NETSTANDARD2_0
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
#if NETSTANDARD2_0
        protected abstract Task<object> ExecuteScalarAsync(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);
#else
        protected abstract Task<object?> ExecuteScalarAsync(IDbConnection? connection, IDbTransaction? transaction, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);
#endif //NETSTANDARD2_0


        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="command">一个有效的用于执行数据库命令的对象。</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
#if NETSTANDARD2_0
        protected abstract IDataReader ExecuteReader(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior);
#else
        protected abstract IDataReader ExecuteReader(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用 <see cref="ExecuteReader(IDbCommand, IDataParameter[], CommandBehavior)"/>或 <see cref="ExecuteReader(IDbConnection, CommandBehavior, CommandType, string, IDataParameter[], out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters);
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用 <see cref="ExecuteReader(IDbCommand, IDataParameter[], CommandBehavior)"/>或 <see cref="ExecuteReader(IDbTransaction, CommandBehavior, CommandType, string, IDataParameter[], out IDbCommand)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters);
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭 <see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract IDataReader ExecuteReader(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, out IDbCommand command);
        /// <summary>
        /// 执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="transaction">一个有效的事务。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="command">返回执行命令的对象。用于在存储过程使用输出变量时，关闭 <see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract IDataReader ExecuteReader(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, out IDbCommand command);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="command">一个有效的用于执行数据库命令的对象。</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract Task<IDataReader> ExecuteReaderAsync(IDbCommand command, IDataParameter[] commandParameters, CommandBehavior commandBehavior, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用 <see cref="ExecuteReaderAsync(IDbCommand, IDataParameter[], CommandBehavior, CancellationToken)"/>或 <see cref="ExecuteReaderAndGetCommandAsync(IDbConnection, CommandBehavior, CommandType, string, IDataParameter[], CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract Task<IDataReader> ExecuteReaderAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <remarks>
        /// 这个方法是不能获取输出参数，如果要获取输出参数的值请使用 <see cref="ExecuteReaderAsync(IDbCommand, IDataParameter[], CommandBehavior, CancellationToken)"/>或 <see cref="ExecuteReaderAndGetCommandAsync(IDbTransaction, CommandBehavior, CommandType, string, IDataParameter[], CancellationToken)"/>方法。
        /// </remarks>
        /// <param name="transaction">一个有效的事务。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器</returns>
        protected abstract Task<IDataReader> ExecuteReaderAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="connection">一个有效的数据库连接对象。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器和返回执行命令的对象。用于在存储过程使用输出变量时，关闭 <see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</returns>
        protected abstract Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbConnection connection, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken);
        /// <summary>
        /// 异步执行指定 T-SQL 命令，返回结果集的数据读取器。
        /// </summary>
        /// <param name="transaction">一个有效的事务。</param>
        /// <param name="commandBehavior">传入<see cref="IDbCommand.ExecuteReader(CommandBehavior)"/>方法的参数。</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>
        /// <param name="commandText">存储过程名或T-SQL语句</param>
        /// <param name="commandParameters">参数数组,如果没有参数则为<c>null</c></param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>返回包含结果集的数据读取器和返回执行命令的对象。用于在存储过程使用输出变量时，关闭 <see cref="IDataReader"/>对象，输出参数被赋值后清除参数，达到参数对象重复使用的目的。</returns>
        protected abstract Task<(IDataReader, IDbCommand)> ExecuteReaderAndGetCommandAsync(IDbTransaction transaction, CommandBehavior commandBehavior, CommandType commandType, string commandText, IDataParameter[] commandParameters, CancellationToken cancellationToken);






        /// <summary>
        /// 将<see cref="IDbCommand.ExecuteScalar"/>方法的返回的<see cref="Object"/>类型的值转换成<typeparamref name="T"/>类型的值。
        /// </summary>
        /// <typeparam name="T">要转成什么类型。</typeparam>
        /// <param name="obj"><see cref="IDbCommand.ExecuteScalar"/>方法返回的值。</param>
        /// <returns>返回转换成目标类型的值或默认值。</returns>
#if NET5_0_OR_GREATER
        protected virtual T? ObjectToScalar<T>(object obj)
#else
        protected virtual T ObjectToScalar<T>(object obj)
#endif //NET5_0_OR_GREATER
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
#if NETSTANDARD2_0
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
                    result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (reader.Read());
            }
            return result;
        }
#elif NETSTANDARD2_1
        protected virtual IDictionary<TKey, TValue>? ReadToDictionary<TKey, TValue>(IDataReader reader)
            where TKey : notnull
        {
            IDictionary<TKey, TValue>? result = null;

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
                    result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (reader.Read());
            }
            return result;
        }
#else
        protected virtual IDictionary<TKey, TValue?>? ReadToDictionary<TKey, TValue>(IDataReader reader)
            where TKey : notnull
        {
            IDictionary<TKey, TValue?>? result = null;
            
            if (reader.Read())
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue?>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue?>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue?>();
                }

                do
                {
                    result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (reader.Read());
            }
            return result;
        }
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步将<paramref name="reader"/>[0]作为键、将<paramref name="reader"/>[1]作为值，读取到键值对集合实例里。
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则返回这些数据的键值对集合。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<IDictionary<TKey, TValue>> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default)
        {
            IDictionary<TKey, TValue> result = null;

            if (await reader.ReadAsync(cancellationToken))
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
                    result.Add((TKey)reader[0], await reader.IsDBNullAsync(1, cancellationToken) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            return result;
        }
#elif NETSTANDARD2_1
        protected virtual async Task<IDictionary<TKey, TValue>?> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            IDictionary<TKey, TValue>? result = null;

            if (await reader.ReadAsync(cancellationToken))
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
                    result.Add((TKey)reader[0], await reader.IsDBNullAsync(1, cancellationToken) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            return result;
        }
#else
        protected virtual async Task<IDictionary<TKey, TValue?>?> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default)
            where TKey : notnull
        {
            IDictionary<TKey, TValue?>? result = null;

            if (await reader.ReadAsync(cancellationToken))
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue?>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue?>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue?>();
                }

                do
                {
                    result.Add((TKey)reader[0], await reader.IsDBNullAsync(1, cancellationToken) ? default : (TValue)InternalConvertUtility.HackType(reader[1], typeof(TValue)));
                }
                while (await reader.ReadAsync(cancellationToken));
            }
            return result;
        }
#endif //NETSTANDARD2_0
        /*
        /// <summary>
        /// 将<paramref name="reader"/>[0]作为键、将<paramref name="reader"/>[1]作为值，读取到键值对集合实例里。
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>如果读取器里有数据则返回这些数据的键值对集合。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual IDictionary<TKey, TValue> ReadToDictionary<TKey, TValue>(IDataReader reader)
            where TValue : new()
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

                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (reader.Read());
                }
                else
                {
                    Type conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (reader.Read());
                }
            }
            return result;
        }
#elif NETSTANDARD2_1
        protected virtual IDictionary<TKey, TValue>? ReadToDictionary<TKey, TValue>(IDataReader reader)
            where TKey : notnull
            where TValue : new()
        {
            IDictionary<TKey, TValue>? result = null;

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

                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName!) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (reader.Read());
                }
                else
                {
                    Type? conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value!;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (reader.Read());
                }
            }
            return result;
        }
#else
        protected virtual IDictionary<TKey, TValue?>? ReadToDictionary<TKey, TValue>(IDataReader reader)
            where TKey : notnull
            where TValue : new()
        {
            IDictionary<TKey, TValue?>? result = null;

            if (reader.Read())
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue?>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue?>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue?>();
                }

                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName!) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (reader.Read());
                }
                else
                {
                    Type? conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value!;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (reader.Read());
                }
                
            }
            return result;
        }
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步将<paramref name="reader"/>[0]作为键、将<paramref name="reader"/>[1]作为值，读取到键值对集合实例里。
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则返回这些数据的键值对集合。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<IDictionary<TKey, TValue>> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default) where TValue : new()
        {
            IDictionary<TKey, TValue> result = null;

            if (await reader.ReadAsync(cancellationToken))
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

                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
                else
                {
                    Type conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
            }
            return result;
        }
#elif NETSTANDARD2_1
        protected virtual async Task<IDictionary<TKey, TValue>?> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default)
            where TKey : notnull
            where TValue : new()
        {
            IDictionary<TKey, TValue>? result = null;

            if (await reader.ReadAsync(cancellationToken))
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

                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName!) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
                else
                {
                    Type? conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value!;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
            }
            return result;
        }
#else
        protected virtual async Task<IDictionary<TKey, TValue?>?> ReadToDictionaryAsync<TKey, TValue>(DbDataReader reader, CancellationToken cancellationToken = default)
            where TKey : notnull
            where TValue : new()
        {
            IDictionary<TKey, TValue?>? result = null;
            
            if (await reader.ReadAsync(cancellationToken))
            {
                if (_dictionaryInstance != null)
                {
                    result = (IDictionary<TKey, TValue?>)_dictionaryInstance;
                    _dictionaryInstance = null;
                }
                else if (_funBuildDictionaryInstance != null)
                {
                    result = ((Func<IDictionary<TKey, TValue?>>)_funBuildDictionaryInstance)();
                    _funBuildDictionaryInstance = null;
                }
                else
                {
                    result = new Dictionary<TKey, TValue?>();
                }

                
                Type tTalueType = typeof(TValue);
                if (tTalueType.GetInterface(typeof(IList).FullName!) == null)
                {
                    do
                    {
                        result.Add((TKey)reader[0], reader.IsDBNull(1) ? default : (TValue)InternalConvertUtility.HackType(reader[1], tTalueType));
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
                else
                {
                    Type? conversionType;
                    if (tTalueType.IsGenericType)
                    {
                        conversionType = tTalueType.GenericTypeArguments[0];
                    }
                    else
                    {
                        conversionType = default;
                    }

                    do
                    {
                        IList list;
                        TKey key = (TKey)reader[0];
                        if (result.TryGetValue(key, out var value))
                        {
                            list = (IList)value!;
                        }
                        else
                        {
                            value = new TValue();
                            list = (IList)value;
                            result.Add(key, value);
                        }

                        if (conversionType == default)
                        {
                            list.Add(reader.IsDBNull(1) ? default : InternalConvertUtility.HackType(reader[1], tTalueType));
                        }
                        else
                        {
                            list.Add(reader.IsDBNull(1) ? default : reader[1]);
                        }
                    }
                    while (await reader.ReadAsync(cancellationToken));
                }
            }
            return result;
        }
#endif //NETSTANDARD2_0
        */

        /// <summary>
        /// 将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual T ReadToModel<T>(IDataReader reader) where T : class
#else
        protected virtual T? ReadToModel<T>(IDataReader reader) where T : class
#endif //NETSTANDARD2_0
        {
            if (!reader.Read()) return default;

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
        /// 将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual T ReadToModel<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod) where T : class
#else
        protected virtual T? ReadToModel<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod) where T : class
#endif //NETSTANDARD2_0
        {
            if (!reader.Read()) return default;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldName, customAssignMethod);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldName, customAssignMethod);
                return model;
            }

            return reader.ToModel<T>(ignoreFieldName, customAssignMethod);
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual T ReadToModel<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod) where T : class
#else
        protected virtual T? ReadToModel<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod) where T : class
#endif //NETSTANDARD2_0
        {
            if (!reader.Read()) return default;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldNames, customAssignMethod);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldNames, customAssignMethod);
                return model;
            }

            return reader.ToModel<T>(ignoreFieldNames, customAssignMethod);
        }

        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<T> ReadToModelAsync<T>(DbDataReader reader, CancellationToken cancellationToken) where T : class
#else
        protected virtual async Task<T?> ReadToModelAsync<T>(DbDataReader reader, CancellationToken cancellationToken) where T : class
#endif //NETSTANDARD2_0
        {
            if (!await reader.ReadAsync(cancellationToken)) return default;

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
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<T> ReadToModelAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken) where T : class
#else
        protected virtual async Task<T?> ReadToModelAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken) where T : class
#endif //NETSTANDARD2_0
        {
            if (!await reader.ReadAsync(cancellationToken)) return default;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldName, customAssignMethod);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldName, customAssignMethod);
                return model;
            }

            return reader.ToModel<T>(ignoreFieldName, customAssignMethod);
        }


        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据按照列明与属性名对应，读取到类型<typeparamref name="T"/>的对象实例的各个属性。
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>如果读取器里有数据则将这些数据写入类型<typeparamref name="T"/>的实例并返回此实例。如果没有数据则返回<c>null</c>。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<T> ReadToModelAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken) where T : class
#else
        protected virtual async Task<T?> ReadToModelAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken) where T : class
#endif //NETSTANDARD2_0
        {
            if (!await reader.ReadAsync(cancellationToken)) return default;

            if (_modelInstance != null)
            {
                T model = (T)_modelInstance;
                _modelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldNames, customAssignMethod);
                return model;
            }
            else if (_funBuildModelInstance != null)
            {
                T model = ((Func<T>)_funBuildModelInstance)();
                _funBuildModelInstance = null;
                reader.ToModel<T>(ref model, ignoreFieldNames, customAssignMethod);
                return model;
            }

            return reader.ToModel<T>(ignoreFieldNames, customAssignMethod);
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
                //T defaultValue = default;
                do
                {
                    do
                    {
                        if (reader.IsDBNull(0))
                        {
                            //collection.Add(defaultValue);
                        }
                        else
                        {
                            collection.Add((T)InternalConvertUtility.HackType(reader[0], typeof(T)));
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
        /// 将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <exception cref="NotSupportedException">此方法不支持<typeparamref name="T"/>为简单的值类型。</exception>
        private void FillToCollection<T>(IDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == ValueTypeConstants.GuidType)
            {
                //此方法不支持简单的值类型。
                throw new NotSupportedException();
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        reader.ToCollection<T>(collection, ignoreFieldName, customAssignMethod);
                    }
                    while (reader.NextResult() && reader.Read());
                }
                else
                {
                    Func<T> fun = (Func<T>)_funBuildModelInstance;
                    _funBuildModelInstance = null;
                    do
                    {
                        reader.ToCollection<T>(collection, ignoreFieldName, customAssignMethod, fun);
                    }
                    while (reader.NextResult() && reader.Read());
                }
            }
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <exception cref="NotSupportedException">此方法不支持<typeparamref name="T"/>为简单的值类型。</exception>
        private void FillToCollection<T>(IDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == ValueTypeConstants.GuidType)
            {
                //此方法不支持简单的值类型。
                throw new NotSupportedException();
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        reader.ToCollection<T>(collection, ignoreFieldNames, customAssignMethod);
                    }
                    while (reader.NextResult() && reader.Read());
                }
                else
                {
                    Func<T> fun = (Func<T>)_funBuildModelInstance;
                    _funBuildModelInstance = null;
                    do
                    {
                        reader.ToCollection<T>(collection, ignoreFieldNames, customAssignMethod, fun);
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
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
#if NETSTANDARD2_0
        private async Task FillToCollectionAsync<T>(DbDataReader reader, ICollection<T>collection, CancellationToken cancellationToken)
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
                //T defaultValue = default;
                do
                {
                    do
                    {
                        if (await reader.IsDBNullAsync(0, cancellationToken))
                        {
                            //collection.Add(defaultValue);
                        }
                        else
                        {
                            collection.Add((T)InternalConvertUtility.HackType(reader[0], typeof(T)));
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
        /// 异步将<paramref name="reader"/>里的数据读取到<paramref name="collection"/>对象实例中。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="collection">存储数据的集合。</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <exception cref="NotSupportedException">此方法不支持<typeparamref name="T"/>为简单的值类型。</exception>
#if NETSTANDARD2_0
        private async Task FillToCollectionAsync<T>(DbDataReader reader, ICollection<T>collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
#else
        private async ValueTask FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
#endif
        {
            Task<bool> nextResult;
            Task<bool> readResult;
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == typeof(Guid))
            {
                //此方法不支持简单的值类型。
                throw new NotSupportedException();
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection, ignoreFieldName, customAssignMethod, cancellationToken);

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
                        await reader.ToCollectionAsync<T>(collection, ignoreFieldName, customAssignMethod, cancellationToken, fun);

                        nextResult = reader.NextResultAsync(cancellationToken);
                        readResult = reader.ReadAsync(cancellationToken);
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
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <exception cref="NotSupportedException">此方法不支持<typeparamref name="T"/>为简单的值类型。</exception>
#if NETSTANDARD2_0
        private async Task FillToCollectionAsync<T>(DbDataReader reader, ICollection<T>collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
#else
        private async ValueTask FillToCollectionAsync<T>(DbDataReader reader, ICollection<T> collection, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
#endif
        {
            Task<bool> nextResult;
            Task<bool> readResult;
            Type t = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(t);
            //struct 类型的 t.IsValueType 等于 true，typeCode 等于 TypeCode.Object。
            if ((t.IsValueType && typeCode != TypeCode.Object) || typeCode == TypeCode.String || t == typeof(Guid))
            {
                //此方法不支持简单的值类型。
                throw new NotSupportedException();
            }
            else
            {
                if (_funBuildModelInstance == null)
                {
                    do
                    {
                        await reader.ToCollectionAsync<T>(collection, ignoreFieldNames, customAssignMethod, cancellationToken);

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
                        await reader.ToCollectionAsync<T>(collection, ignoreFieldNames, customAssignMethod, cancellationToken, fun);

                        nextResult = reader.NextResultAsync(cancellationToken);
                        readResult = reader.ReadAsync(cancellationToken);
                    }
                    while (await nextResult && await readResult);
                }
            }
        }

        /// <summary>
        /// 构建一个<typeparamref name="T1"/>类型的集合实例。
        /// 优先使用<paramref name="instance"/>、<paramref name="funBuildListInstance"/>进行构造，如果两个变量都为<c>null</c>则构造一个<typeparamref name="T2"/>类型的默认实例。
        /// </summary>
        /// <typeparam name="T1">要构造的集合类型。可以是接口、基类、具体类型。</typeparam>
        /// <typeparam name="T2">默认实例的类型。如果<paramref name="instance"/>、<paramref name="funBuildListInstance"/>均为<c>null</c>则使用该类型的无参构造函数来构造实例并返回。</typeparam>
        /// <param name="instance">优先返回的实例。该实例必须是<typeparamref name="T1"/>类型或可以转换成该类型的实例。</param>
        /// <param name="funBuildListInstance">如果<paramref name="instance"/>为<c>null</c>，则使用该委托构造实例并返回。该委托必须返回<typeparamref name="T1"/>类型或可以转换成该类型的实例。</param>
        /// <returns></returns>
#if NETSTANDARD2_0
        private T1 GetOrBuildCollectionInstance<T1, T2>(ref object instance, ref object funBuildListInstance)
#else
        private T1 GetOrBuildCollectionInstance<T1, T2>(ref object? instance, ref object? funBuildListInstance)
#endif
            where T2 : T1, new()
        {
            T1 result;
            
            if (instance != null)
            {
                result = (T1)instance;
                instance = null;
            }
            else if (funBuildListInstance != null)
            {
                result = ((Func<T1>)funBuildListInstance)();
                funBuildListInstance = null;
            }
            else
            {
                result = new T2();
            }

            return result;
        }

        /// <summary>
        /// 构建列表实例。优先使用<see cref="_listInstance"/>、<see cref="_funBuildListInstance"/>进行构造，如果两个变量都为<c>null</c>则构造一个默认实例。
        /// </summary>
        /// <typeparam name="T">列表里的元素数据类型。</typeparam>
        /// <returns>列表实例。</returns>
        private IList<T> GetOrBuildListInstance<T>() => GetOrBuildCollectionInstance<IList<T>, List<T>>(ref _listInstance, ref _funBuildListInstance);
        
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
#if NETSTANDARD2_0
        protected virtual IList<T> ReadToList<T>(IDataReader reader)
        {
            IList<T> result = null;
#else
        protected virtual IList<T>? ReadToList<T>(IDataReader reader)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildListInstance<T>();

                FillToCollection<T>(reader, result);
            }
            return result;
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
#if NETSTANDARD2_0
        protected virtual IList<T> ReadToList<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            IList<T> result = null;
#else
        protected virtual IList<T>? ReadToList<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildListInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldName, customAssignMethod);
            }
            return result;
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
#if NETSTANDARD2_0
        protected virtual IList<T> ReadToList<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            IList<T> result = null;
#else
        protected virtual IList<T>? ReadToList<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildListInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldNames, customAssignMethod);
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
#if NETSTANDARD2_0
        protected virtual async Task<IList<T>> ReadToListAsync<T>(DbDataReader reader, CancellationToken cancellationToken = default)
        {
            IList<T> result = null;
#else
        protected virtual async Task<IList<T>?> ReadToListAsync<T>(DbDataReader reader, CancellationToken cancellationToken = default)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildListInstance<T>();

                await FillToCollectionAsync<T>(reader, result, cancellationToken);

            }
            return result;
        }

        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<IList<T>> ReadToListAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken = default)
        {
            IList<T> result = null;
#else
        protected virtual async Task<IList<T>?> ReadToListAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken = default)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildListInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldName, customAssignMethod, cancellationToken);

            }
            return result;
        }

        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到列表对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象列表。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<IList<T>> ReadToListAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken = default)
        {
            IList<T> result = null;
#else
        protected virtual async Task<IList<T>?> ReadToListAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken = default)
        {
            IList<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildListInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldNames, customAssignMethod, cancellationToken);

            }
            return result;
        }


        /// <summary>
        /// 构建集实例。优先使用<see cref="_setInstance"/>、<see cref="_funBuildSetInstance"/>进行构造，如果两个变量都为<c>null</c>则构造一个默认实例。
        /// </summary>
        /// <typeparam name="T">集的元素数据类型。</typeparam>
        /// <returns>集实例。</returns>
        private ISet<T> GetOrBuildSetInstance<T>() => GetOrBuildCollectionInstance<ISet<T>, HashSet<T>>(ref _setInstance, ref _funBuildSetInstance);

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
#if NETSTANDARD2_0
        protected virtual ISet<T> ReadToSet<T>(IDataReader reader)
        {
            ISet<T> result = null;
#else
        protected virtual ISet<T>? ReadToSet<T>(IDataReader reader)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildSetInstance<T>();

                FillToCollection<T>(reader, result);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
#if NETSTANDARD2_0
        protected virtual ISet<T> ReadToSet<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            ISet<T> result = null;
#else
        protected virtual ISet<T>? ReadToSet<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildSetInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldName, customAssignMethod);
            }
            return result;
        }

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
#if NETSTANDARD2_0
        protected virtual ISet<T> ReadToSet<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            ISet<T> result = null;
#else
        protected virtual ISet<T>? ReadToSet<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildSetInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldNames, customAssignMethod);
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
#if NETSTANDARD2_0
        protected virtual async Task<ISet<T>> ReadToSetAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
        {
            ISet<T> result = null;
#else
        protected virtual async Task<ISet<T>?> ReadToSetAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildSetInstance<T>();

                await FillToCollectionAsync<T>(reader, result, cancellationToken);
            }
            return result;
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<ISet<T>> ReadToSetAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
        {
            ISet<T> result = null;
#else
        protected virtual async Task<ISet<T>?> ReadToSetAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildSetInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            return result;
        }
        /// <summary>
        /// 异步将<paramref name="reader"/>里的数据读取到数据集对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<ISet<T>> ReadToSetAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
        {
            ISet<T> result = null;
#else
        protected virtual async Task<ISet<T>?> ReadToSetAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
        {
            ISet<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildSetInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            return result;
        }


        /// <summary>
        /// 构建集合实例。优先使用<see cref="_collectionInstance"/>、<see cref="_funBuildCollectionInstance"/>进行构造，如果两个变量都为<c>null</c>则构造一个默认实例。
        /// </summary>
        /// <typeparam name="T">集合元素数据类型。</typeparam>
        /// <returns>集合实例。</returns>
        private ICollection<T> GetOrBuildCollectionInstance<T>() => GetOrBuildCollectionInstance<ICollection<T>, List<T>>(ref _collectionInstance, ref _funBuildCollectionInstance);

        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
#if NETSTANDARD2_0
        protected virtual ICollection<T> ReadToCollection<T>(IDataReader reader)
        {
            ICollection<T> result = null;
#else
        protected virtual ICollection<T>? ReadToCollection<T>(IDataReader reader)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildCollectionInstance<T>();

                FillToCollection<T>(reader, result);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
#if NETSTANDARD2_0
        protected virtual ICollection<T> ReadToCollection<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            ICollection<T> result = null;
#else
        protected virtual ICollection<T>? ReadToCollection<T>(IDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildCollectionInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldName, customAssignMethod);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
#if NETSTANDARD2_0
        protected virtual ICollection<T> ReadToCollection<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            ICollection<T> result = null;
#else
        protected virtual ICollection<T>? ReadToCollection<T>(IDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (reader.Read())
            {
                result = GetOrBuildCollectionInstance<T>();

                FillToCollection<T>(reader, result, ignoreFieldNames, customAssignMethod);
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
#if NETSTANDARD2_0
        protected virtual async Task<ICollection<T>> ReadToCollectionAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
        {
            ICollection<T> result = null;
#else
        protected virtual async Task<ICollection<T>?> ReadToCollectionAsync<T>(DbDataReader reader, CancellationToken cancellationToken)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildCollectionInstance<T>();

                await FillToCollectionAsync<T>(reader, result, cancellationToken);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldName">要忽略赋值的列名，因为该列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<ICollection<T>> ReadToCollectionAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
        {
            ICollection<T> result = null;
#else
        protected virtual async Task<ICollection<T>?> ReadToCollectionAsync<T>(DbDataReader reader, string ignoreFieldName, RefT1Action<T, IDataReader, int> customAssignMethod, CancellationToken cancellationToken)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildCollectionInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldName, customAssignMethod, cancellationToken);
            }
            return result;
        }
        /// <summary>
        /// 将<paramref name="reader"/>里的数据读取到集合对象实例里。
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="reader">数据读取器</param>
        /// <param name="ignoreFieldNames">要忽略赋值的列名，因为这些列不需要赋值或在<paramref name="customAssignMethod"/>里进行赋值。</param>
        /// <param name="customAssignMethod">键为<typeparamref name="T"/>成员属性名，值为特殊的赋值方法。</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <returns>承载<paramref name="reader"/>数据的对象集合。</returns>
#if NETSTANDARD2_0
        protected virtual async Task<ICollection<T>> ReadToCollectionAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
        {
            ICollection<T> result = null;
#else
        protected virtual async Task<ICollection<T>?> ReadToCollectionAsync<T>(DbDataReader reader, HashSet<string> ignoreFieldNames, RefT1Action<T, IDataReader> customAssignMethod, CancellationToken cancellationToken)
        {
            ICollection<T>? result = null;
#endif //NETSTANDARD2_0

            if (await reader.ReadAsync(cancellationToken))
            {
                result = GetOrBuildCollectionInstance<T>();

                await FillToCollectionAsync<T>(reader, result, ignoreFieldNames, customAssignMethod, cancellationToken);
            }
            return result;
        }
    }
}
