/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  定义关系型数据库的常用操作方法。
 * 
 * Members Index:
 *      interface IRelationalDBUtility
 *          
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.DataProvider
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;



    /// <summary>
    /// 关系型数据库常用操作接口。
    /// </summary>
    public interface IRelationalDBUtility
    {
        /// <summary>
        /// 表示遍历参数集合时，当遇到某个元素为 <c>null</c> 时的处理行为。true：停止遍历；false：跳过当前元素继续遍历。
        /// </summary>
        bool BreakWhenParametersElementIsNull { get; set; }
        /// <summary>
        /// 目标数据库服务器角色。
        /// 表示要在哪个角色的数据库服务器上执行操作。
        /// 默认值：<see cref="DatabaseServerRole.Master"/>，表示在数据库主服务器上执行操作。
        /// </summary>
        DatabaseServerRole TargetRole { get; set; }
        /// <summary>
        /// 获取当前使用的数据库服务器索引。注意：<see cref="TargetRole"/>将影响此属性。
        /// </summary>
        int ServerIndex { get; }

        /// <summary>
        /// 使用指定模型对象实例接收<see cref="GetSingle{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法读取到的数据并返回此实例。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetSingle{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="model">用于接收数据的对象实例。</param>
        void UseModelInstance<T>(T model) where T : class;
        /// <summary>
        /// 使用指定的<see cref="IList{T}"/>对象实例作为<see cref="GetList{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法所返回的对象实例。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetList{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="list">用作为<see cref="GetList{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法的返回值的实例。</param>
        void UseListInstance<T>(IList<T> list);
        /// <summary>
        /// 使用指定的<see cref="ISet{T}"/>对象实例作为<see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法所返回的对象实例。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="set">用作为<see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法的返回值的实例。</param>
        void UseSetInstance<T>(ISet<T> set);
        /// <summary>
        /// 使用指定的<see cref="ICollection{T}"/>对象实例作为<see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法所返回的对象实例。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="collection">用作为<see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>方法及其重载方法的返回值的实例。</param>
        void UseCollectionInstance<T>(ICollection<T> collection);
        /// <summary>
        /// 使用指定的<see cref="IDictionary{TKey, TValue}"/>对象实例作为<see cref="GetDictionary{TKey, TValue}(string, CommandType, IDataParameter[])"/>方法及其重载方法所返回的对象实例。
        /// </summary>
        /// <typeparam name="TKey">键的类型。</typeparam>
        /// <typeparam name="TValue">值的类型。</typeparam>
        /// <param name="dict">用作为<see cref="GetDictionary{TKey, TValue}(string, CommandType, IDataParameter[])"/>方法及其重载方法的返回值的实例。</param>
        void UseDictionaryInstance<TKey, TValue>(IDictionary<TKey, TValue> dict);

        /// <summary>
        /// 指定用作构造指定模型对象实例的方法。
        /// 
        /// 如果未使用<see cref="UseModelInstance{T}(T)"/>方法指定对象实例，则使用此方法指定的<paramref name="fun"/>方法构造模型，否则使用<see cref="UseModelInstance{T}(T)"/>指定的模型对象实例。
        /// 
        /// 如果指定了构造模型对象实体的方法，在调用
        /// <see cref="GetList{T}(string, CommandType, IDataParameter[])"/>、
        /// <see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>、
        /// <see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>
        /// 这几个方法或其重载方法时，使用此方法指定的<paramref name="fun"/>方法构造模型。
        /// </summary>
        /// <typeparam name="T">
        /// 模型类型，此类型必须与
        /// <see cref="GetSingle{T}(string, CommandType, IDataParameter[])"/>、
        /// <see cref="GetList{T}(string, CommandType, IDataParameter[])"/>、
        /// <see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>、
        /// <see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>
        /// 方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="fun">用于构造指定模型对象实体的方法。</param>
        void BuildUseModelInstance<T>(Func<T> fun) where T : class;
        /// <summary>
        /// 指定用作构造<see cref="IList{T}"/>对象实例的方法。
        ///
        /// 如果未使用<see cref="UseListInstance{T}(IList{T})"/>方法指定对象实例，则使用此方法指定的<paramref name="fun"/>方法构造对象实例，否则使用<see cref="UseListInstance{T}(IList{T})"/>指定的对象实例。
        ///
        /// 如果指定了构造<see cref="IList{T}"/>对象实例的方法，在调用<see cref="GetList{T}(string, CommandType, IDataParameter[])"/>方法或其重载方法时，使用此方法指定的<paramref name="fun"/>方法构造模型。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetList{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="fun">用于构造指定模型<see cref="IList{T}"/>对象实体的方法。</param>
        void BuildUseListInstance<T>(Func<IList<T>> fun);
        /// <summary>
        /// 指定用作构造<see cref="ISet{T}"/>对象实例的方法。
        ///
        /// 如果未使用<see cref="UseSetInstance{T}(ISet{T})"/>方法指定对象实例，则使用此方法指定的<paramref name="fun"/>方法构造对象实例，否则使用<see cref="UseSetInstance{T}(ISet{T})"/>指定的对象实例。
        ///
        /// 如果指定了构造<see cref="ISet{T}"/>对象实例的方法，在调用<see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>方法或其重载方法时，使用此方法指定的<paramref name="fun"/>方法构造模型。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetSet{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="fun">用于构造指定模型<see cref="ISet{T}"/>对象实体的方法。</param>
        void BuildUseSetInstance<T>(Func<ISet<T>> fun);
        /// <summary>
        /// 指定用作构造<see cref="ICollection{T}"/>对象实例的方法。
        ///
        /// 如果未使用<see cref="UseCollectionInstance{T}(ICollection{T})"/>方法指定对象实例，则使用此方法指定的<paramref name="fun"/>方法构造对象实例，否则使用<see cref="UseCollectionInstance{T}(ICollection{T})"/>指定的对象实例。
        ///
        /// 如果指定了构造<see cref="ICollection{T}"/>对象实例的方法，在调用<see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>方法或其重载方法时，使用此方法指定的<paramref name="fun"/>方法构造模型。
        /// </summary>
        /// <typeparam name="T">模型类型，此类型必须与<see cref="GetCollection{T}(string, CommandType, IDataParameter[])"/>方法中的泛型类型一致或是它的父类型。</typeparam>
        /// <param name="fun">用于构造指定模型<see cref="ICollection{T}"/>对象实体的方法。</param>
        void BuildUseCollectionInstance<T>(Func<ICollection<T>> fun);
        /// <summary>
        /// 指定用作构造<see cref="IDictionary{TKey, TValue}"/>对象实例的方法。
        ///
        /// 如果未使用<see cref="UseDictionaryInstance{TKey, TValue}(IDictionary{TKey, TValue})"/>方法指定对象实例，则使用此方法指定的<paramref name="fun"/>方法构造对象实例，否则使用<see cref="UseDictionaryInstance{TKey, TValue}(IDictionary{TKey, TValue})"/>指定的对象实例。
        ///
        /// 如果指定了构造<see cref="IDictionary{TKey, TValue}"/>对象实例的方法，在调用<see cref="GetDictionary{TKey, TValue}(string, CommandType, IDataParameter[])"/>方法或其重载方法时，使用此方法指定的<paramref name="fun"/>方法构造模型。
        /// </summary>
        /// <typeparam name="TKey">键的类型。</typeparam>
        /// <typeparam name="TValue">值的类型。</typeparam>
        /// <param name="fun">用于构造<see cref="IDictionary{TKey, TValue}"/>对象实体的方法。</param>
        void BuildUseDictionaryInstance<TKey, TValue>(Func<IDictionary<TKey, TValue>> fun);


        /// <summary>
        /// 将通配符转换为普通符号。
        /// </summary>
        /// <param name="value">放在 LIKE 中的值。</param>
        /// <returns>转义后的值。</returns>
        string WildcardEscape(string value);


        /// <summary>
        /// 将<paramref name="value"/>中的字符进行安全处理，防止拼接出诸如<c>SQL</c>注入的攻击性<c>SQL</c>语句。
        /// </summary>
        /// <param name="value"><c>SQL</c>语句/字句或要拼接成<c>SQL</c>语句的字符串。</param>
        /// <returns>处理后的字符串值</returns>
        string SafeHandler(string value);

        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(byte bitFields);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(byte bitFields, out int parameterCount);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="values">位域的可用值。方法返回的所有值都是这里出现过的值。如果为 <c>null</c> 则默认使用 <see cref="byte"/> 的 8 个独立位域值。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
#if NETSTANDARD2_0
        string GetSqlInExpressions(byte bitFields, byte[] values, out int parameterCount);
#else
        string GetSqlInExpressions(byte bitFields, byte[]? values, out int parameterCount);
#endif
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(ushort bitFields);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(ushort bitFields, out int parameterCount);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="values">位域的可用值。方法返回的所有值都是这里出现过的值。如果为 <c>null</c> 则默认使用 <see cref="ushort"/> 的 16 个独立位域值。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
#if NETSTANDARD2_0
        string GetSqlInExpressions(ushort bitFields, ushort[] values, out int parameterCount);
#else
        string GetSqlInExpressions(ushort bitFields, ushort[]? values, out int parameterCount);
#endif
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(uint bitFields);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(uint bitFields, out int parameterCount);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="values">位域的可用值。方法返回的所有值都是这里出现过的值。如果为 <c>null</c> 则默认使用 <see cref="uint"/> 的 32 个独立位域值。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
#if NETSTANDARD2_0
        string GetSqlInExpressions(uint bitFields, uint[] values, out int parameterCount);
#else
        string GetSqlInExpressions(uint bitFields, uint[]? values, out int parameterCount);
#endif
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(ulong bitFields);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
        string GetSqlInExpressions(ulong bitFields, out int parameterCount);
        /// <summary>
        /// 将位域转换成SQL条件的IN里的表达式。
        /// </summary>
        /// <param name="bitFields">位域。</param>
        /// <param name="values">位域的可用值。方法返回的所有值都是这里出现过的值。如果为 <c>null</c> 则默认使用 <see cref="ulong"/> 的 64 个独立位域值。</param>
        /// <param name="parameterCount">返回参数总数。也就是返回了 <paramref name="bitFields"/> 中的几个 <c>bit</c>。</param>
        /// <returns>SQL条件IN的表达式，例如：<c>1,2,4,8,16,32</c>。</returns>
#if NETSTANDARD2_0
        string GetSqlInExpressions(ulong bitFields, ulong[] values, out int parameterCount);
#else
        string GetSqlInExpressions(ulong bitFields, ulong[]? values, out int parameterCount);
#endif

        /// <summary>
        /// 将参数集合转换成多组SQL条件IN里的表达式，同时会对每个元素进行安全处理。
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns>SQL条件IN的表达式数组，每个元素不包括任何SQL关键字及括号。</returns>
        string[] GetSqlInExpressions(IEnumerable<string> parameters);
        /// <summary>
        /// 将参数集合转换成多组SQL条件IN里的表达式，同时会对每个元素进行安全处理。
        /// </summary>
        /// <param name="parameters">参数集合。</param>
        /// <param name="parameterCount">返回参数总数。</param>
        /// <returns>SQL条件IN的表达式数组，每个元素不包括任何SQL关键字及括号。</returns>
        string[] GetSqlInExpressions(IEnumerable<string> parameters, out int parameterCount);
        /// <summary>
        /// 将参数集合转换成多组SQL条件IN里的表达式。
        /// </summary>
        /// <param name="parameters">参数集合。</param>
        /// <returns>SQL条件IN的表达式数组，每个元素不包括任何SQL关键字及括号。</returns>
        string[] GetSqlInExpressions<T>(IEnumerable<T> parameters) where T : struct;
        /// <summary>
        /// 将参数集合转换成多组SQL条件IN里的表达式。
        /// </summary>
        /// <param name="parameters">参数集合。</param>
        /// <param name="parameterCount">返回参数总数。</param>
        /// <returns>SQL条件IN的表达式数组，每个元素不包括任何SQL关键字及括号。</returns>
        string[] GetSqlInExpressions<T>(IEnumerable<T> parameters, out int parameterCount) where T : struct;

        /// <summary>
        /// 将指定数据库服务器设定为当前实例使用的数据库服务器。
        /// </summary>
        /// <param name="serverRole">服务器角色。</param>
        /// <param name="index">连接字符串索引</param>
        void Appoint(DatabaseServerRole serverRole, int index);

        /// <summary>
        /// 将当前实例执行 T-SQL 命令的数据库服务器设置位下一台数据库服务器。
        /// </summary>
        void NextServer();

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例。
        /// </summary>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口的对象实例。</returns>
        IDbConnection BuildConnection();
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口的对象实例。</returns>
        IDbConnection BuildConnection(string connectionString);

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        IDbConnection BuildConnectionAndOpen();
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        IDbConnection BuildConnectionAndOpen(string connectionString);

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        Task<IDbConnection> BuildConnectionAndOpenAsync();
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString);

        /// <summary>
        /// 使用默认的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        Task<IDbConnection> BuildConnectionAndOpenAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 使用指定的数据库链接字符串创建一个数据库链接对象实例，并将链接以异步方式打开。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>返回一个实现了<see cref="IDbConnection"/>接口并且以开启的对象实例。</returns>
        Task<IDbConnection> BuildConnectionAndOpenAsync(string connectionString, CancellationToken cancellationToken = default);


        /// <summary>
        /// 使用默认的数据库链接创建一个实现了<see cref="System.Data.IDbCommand"/>接口的对象实例。
        /// </summary>
        /// <returns>返回一个实现了<see cref="System.Data.IDbCommand"/>接口的对象实例。</returns>
        IDbCommand BuildCommand();
        /// <summary>
        /// 使用指定的数据库链接创建一个实现了<see cref="System.Data.IDbCommand"/>接口的对象实例。
        /// </summary>
        /// <param name="conn">实现了<see cref="IDbConnection"/>接口的数据库链接对象实例。</param>
        /// <returns>返回一个实现了<see cref="System.Data.IDbCommand"/>接口的对象实例。</returns>
        IDbCommand BuildCommand(IDbConnection conn);


        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <returns>数据库事务对象实例。</returns>
        IDbTransaction BuildTransaction();
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <returns>数据库事务对象实例。</returns>
        IDbTransaction BuildTransaction(IsolationLevel level);

        /// <summary>
        /// 使用默认数据库链接对象创建数据库事务对象实例。
        /// </summary>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        Task<IDbTransaction> BuildTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 使用默认数据库链接对象和指定的事务隔离级别创建数据库事务对象实例。
        /// </summary>
        /// <param name="level">事务隔离级别。</param>
        /// <param name="cancellationToken">表示异步任务是否取消的令牌。</param>
        /// <returns>数据库事务对象实例。</returns>
        Task<IDbTransaction> BuildTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default);


        /// <summary>
        /// 创建一个参数对象实例。
        /// </summary>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        IDbDataParameter BuildParameter();
        /// <summary>
        /// 使用指定参数名和参数值创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数的值。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        IDbDataParameter BuildParameter(string parameterName, object value);
#else
        IDbDataParameter BuildParameter(string parameterName, object? value);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 使用指定参数名、类型和大小创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        IDbDataParameter BuildParameter(string parameterName, int parameterType, int size);  //如果给 size 赋默认值就会导致 参数值为 int 类型时把值当作参数类型。
        /// <summary>
        /// 使用指定参数名、类型、大小和方向创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="direction">参数方向。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
        IDbDataParameter BuildParameter(string parameterName, ParameterDirection direction, int parameterType, int size = default);
        /// <summary>
        /// 使用指定参数名、参数值、类型和大小创建一个参数对象实例。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <param name="value">参数值。</param>
        /// <param name="parameterType">参数类型。取值范围为数据库客户端类库提供的参数类型枚举。</param>
        /// <param name="size">参数大小。</param>
        /// <returns>返回一个用于<see cref="IDbCommand"/>的参数对象实例。</returns>
#if NETSTANDARD2_0
        IDbDataParameter BuildParameter(string parameterName, object value, int parameterType, int size = default);
#else
        IDbDataParameter BuildParameter(string parameterName, object? value, int parameterType, int size = default);
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
        IDbDataParameter BuildParameter(string parameterName, object value, ParameterDirection direction, int parameterType, int size = default);
#else
        IDbDataParameter BuildParameter(string parameterName, object? value, ParameterDirection direction, int parameterType, int size = default);
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
#if NETSTANDARD2_0
        void ResetParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#else
        void ResetParameter(IDbDataParameter parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#endif //NETSTANDARD2_0

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
        IDbDataParameter ResetOrBuildParameter(IDbDataParameter parameter, string parameterName, object value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#else
        IDbDataParameter ResetOrBuildParameter(IDbDataParameter? parameter, string parameterName, object? value, int parameterType, int size = default, ParameterDirection direction = ParameterDirection.Input);
#endif //NETSTANDARD2_0


        /// <summary>
        /// 执行T-SQL命令
        /// </summary>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
        int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行T-SQL命令
        /// </summary>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
        int ExecuteNonQuery(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行T-SQL命令
        /// </summary>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
        int ExecuteNonQuery(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);

        /// <summary>
        /// 异步执行T-SQL命令
        /// </summary>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
#if NETSTANDARD2_0
        Task<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        ValueTask<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif
        /// <summary>
        /// 异步执行T-SQL命令
        /// </summary>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
#if NETSTANDARD2_0
        Task<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        ValueTask<int> ExecuteNonQueryAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif
        /// <summary>
        /// 异步执行T-SQL命令
        /// </summary>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回受影响的记录数</returns>
#if NETSTANDARD2_0
        Task<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        ValueTask<int> ExecuteNonQueryAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif


        /// <summary>
        /// 执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        IDataReader GetReader(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        IDataReader GetReader(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        IDataReader GetReader(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);


        /// <summary>
        /// 异步执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <remarks>如果在方法返回的<see cref="IDataReader"/>关闭之后才会对<see cref="IDbCommand.Parameters"/>关联的输出参数赋值的话，那么此方法不会对存储过程的输出参数赋值。因为在方法返回的<see cref="IDataReader"/>关闭之前就已将<see cref="IDbCommand.Parameters"/>集合清空。</remarks>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        Task<IDataReader> GetReaderAsync(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
        /// <summary>
        /// 异步执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <remarks>如果在方法返回的<see cref="IDataReader"/>关闭之后才会对<see cref="IDbCommand.Parameters"/>关联的输出参数赋值的话，那么此方法不会对存储过程的输出参数赋值。因为在方法返回的<see cref="IDataReader"/>关闭之前就已将<see cref="IDbCommand.Parameters"/>集合清空。</remarks>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        Task<IDataReader> GetReaderAsync(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
        /// <summary>
        /// 异步执行T-SQL命令，并生成<see cref="System.Data.IDataReader"/>。
        /// </summary>
        /// <remarks>如果在方法返回的<see cref="IDataReader"/>关闭之后才会对<see cref="IDbCommand.Parameters"/>关联的输出参数赋值的话，那么此方法不会对存储过程的输出参数赋值。因为在方法返回的<see cref="IDataReader"/>关闭之前就已将<see cref="IDbCommand.Parameters"/>集合清空。</remarks>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">T-SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">SQL参数对象数组</param>
        /// <returns>返回<see cref="System.Data.IDataReader"/>对象实例</returns>
        Task<IDataReader> GetReaderAsync(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);



        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        T GetScalar<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="tran">SQL事务</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        T GetScalar<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        T GetScalar<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);

        /// <summary>
        /// 异步执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        Task<T> GetScalarAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
        /// <summary>
        /// 异步执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="tran">SQL事务</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        Task<T> GetScalarAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
        /// <summary>
        /// 异步执行查询，并返回查询所返回的结果集中第一行的第一列。忽略其他列或行。
        /// </summary>
        /// <typeparam name="T">预返回类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>结果集中第一行的第一列；如果结果集为空，则为预返回类型的默认值。</returns>
        Task<T> GetScalarAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);


        /// <summary>
        /// 获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#else
        IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="tran">SQL事务</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#else
        IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        IDictionary<TKey, TValue> GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        IDictionary<TKey, TValue>? GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#else
        IDictionary<TKey, TValue?>? GetDictionary<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#else
        Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="tran">SQL事务</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#else
        Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取键值对集合
        /// </summary>
        /// <typeparam name="TKey">键的数据类型</typeparam>
        /// <typeparam name="TValue">值的数据类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回键值对集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IDictionary<TKey, TValue>> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#elif NETSTANDARD2_1
        Task<IDictionary<TKey, TValue>?> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#else
        Task<IDictionary<TKey, TValue?>?> GetDictionaryAsync<TKey, TValue>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where TKey : notnull;
#endif //NETSTANDARD2_0


        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        T GetSingle<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#else
        T? GetSingle<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        T GetSingle<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#else
        T? GetSingle<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        T GetSingle<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#else
        T? GetSingle<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<T> GetSingleAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#else
        Task<T?> GetSingleAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<T> GetSingleAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#else
        Task<T?> GetSingleAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回单个实体，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<T> GetSingleAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#else
        Task<T?> GetSingleAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters) where T : class;
#endif //NETSTANDARD2_0

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回<paramref name="structure"/>，没有数据返回null</returns>
        ref T? GetSingle<T>(ref T? structure, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回<paramref name="structure"/>，没有数据返回null</returns>
        ref T? GetSingle<T>(ref T? structure, IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回<paramref name="structure"/>，没有数据返回null</returns>
        ref T? GetSingle<T>(ref T? structure, IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>读取到数据返回<c>true</c>，否则返回<c>false</c>。</returns>
        bool TryGetSingle<T>(ref T? structure, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>读取到数据返回<c>true</c>，否则返回<c>false</c>。</returns>
        bool TryGetSingle<T>(ref T? structure, IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="structure">接收数据的结构体</param>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>读取到数据返回<c>true</c>，否则返回<c>false</c>。</returns>
        bool TryGetSingle<T>(ref T? structure, IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters) where T : struct;



        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        IList<T> GetList<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        IList<T>? GetList<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        IList<T> GetList<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        IList<T>? GetList<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        IList<T> GetList<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        IList<T>? GetList<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IList<T>> GetListAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<IList<T>?> GetListAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IList<T>> GetListAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<IList<T>?> GetListAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回据列表，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<IList<T>> GetListAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<IList<T>?> GetListAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0


        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        ISet<T> GetSet<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ISet<T>? GetSet<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        ISet<T> GetSet<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ISet<T>? GetSet<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        ISet<T> GetSet<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ISet<T>? GetSet<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ISet<T>> GetSetAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ISet<T>?> GetSetAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ISet<T>> GetSetAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ISet<T>?> GetSetAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ISet<T>> GetSetAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ISet<T>?> GetSetAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0




        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        ICollection<T> GetCollection<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ICollection<T>? GetCollection<T>(string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        ICollection<T> GetCollection<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ICollection<T>? GetCollection<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        ICollection<T> GetCollection<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#else
        ICollection<T>? GetCollection<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0

        /// <summary>
        /// 异步获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ICollection<T>> GetCollectionAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ICollection<T>?> GetCollectionAsync<T>(string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="tran">数据库事务对象实例。</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ICollection<T>> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ICollection<T>?> GetCollectionAsync<T>(IDbTransaction tran, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
        /// <summary>
        /// 异步获取实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cancellationToken">通知任务取消的令牌。</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回数据集合，没有数据返回null</returns>
#if NETSTANDARD2_0
        Task<ICollection<T>> GetCollectionAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#else
        Task<ICollection<T>?> GetCollectionAsync<T>(IDbConnection conn, string cmdText, CommandType cmdType = CommandType.Text, CancellationToken cancellationToken = default, params IDataParameter[] parameters);
#endif //NETSTANDARD2_0
    }
}
