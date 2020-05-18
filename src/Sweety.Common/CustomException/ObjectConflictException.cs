/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      表示因对象冲突导致程序无法继续往下执行的异常。
 *      
 * Members Index:
 *      class
 *          ObjectConflictException
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;
    using System.Security;
    using System.Runtime.Serialization;



    /// <summary>
    /// 对象冲突异常。
    /// </summary>
    /// <remarks>
    /// 场景：
    /// 1、某个集合中的元素必须唯一，往该集合里添加一个已有元素则可以抛出此异常，并将集合里已存在的那个元素赋值给 <see cref="ObjectConflictException.ConflictObject"/> 属性。
    /// </remarks>
    [Serializable]
    public class ObjectConflictException : Exception
    {
        /// <summary>
        /// 初始化 <see cref="ObjectConflictException"/> 类的新实例。
        /// </summary>
        public ObjectConflictException() { }
        /// <summary>
        /// 使用指定错误消息初始化 <see cref="ObjectConflictException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public ObjectConflictException(string message) : base(message) { }
        /// <summary>
        /// 使用指定错误消息和发生冲突的对象初始化 <see cref="ObjectConflictException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="conflictObject">发生冲突的对象。</param>
        public ObjectConflictException(string message, object conflictObject) : this(message, conflictObject, null) { }
        /// <summary>
        /// 使用指定错误消息和发生冲突的对象还有对作为此异常原因的内部异常的引用初始化 <see cref="ObjectConflictException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="conflictObject">发生冲突的对象。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用。</param>
        public ObjectConflictException(string message, object conflictObject, Exception innerException) : base(message, innerException)
        {
            if (conflictObject != null) this.ConflictObject = conflictObject;
        }
        ///// <summary>
        ///// 使用指定错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="ObjectConflictException"/> 类的新实例。
        ///// </summary>
        ///// <param name="message">描述错误的消息。</param>
        ///// <param name="innerException">导致当前异常的异常。 如果 <paramref name="innerException"/> 参数不为空引用，则在处理内部异常的 <c>catch</c> 块中引发当前异常。</param>
        //public ObjectConflictException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>
        /// 用序列化数据初始化 <see cref="ObjectConflictException"/> 类的新实例。
        /// </summary>
        /// <param name="info">保存序列化对象数据的对象。</param>
        /// <param name="context">有关源或目标的上下文信息。</param>
        [SecuritySafeCritical]
        protected ObjectConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }


        /// <summary>
        /// 获取已之发生冲突的对象。
        /// </summary>
        public object ConflictObject { get; private set; }
    }
}
