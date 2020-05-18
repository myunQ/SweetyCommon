/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      表示未找到对象导致程序无法继续往下执行的异常。
 *      
 * Members Index:
 *      class
 *          ObjectNotFoundException<T>
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;



    /// <summary>
    /// 未找到对象导致程序无法继续往下执行的异常。
    /// </summary>
    [Serializable]
    public class ObjectNotFoundException<T> : Exception
    {
        /// <summary>
        /// 使用指定的错误消息初始化 <see cref="ObjectNotFoundException{T}"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public ObjectNotFoundException(string message) : base(message) { }
        /// <summary>
        /// 使用指定对象编号和错误消息初始化 <see cref="ObjectNotFoundException{T}"/> 类的新实例。
        /// </summary>
        /// <param name="id">对象唯一标识符。</param>
        /// <param name="message">描述错误的消息。</param>
        public ObjectNotFoundException(T id, string message)
            : this(id, message, null)
        { }
        /// <summary>
        /// 使用指定对象编号和错误消息还有对作为此异常原因的内部异常的引用初始化 <see cref="ObjectNotFoundException{T}"/> 类的新实例。
        /// </summary>
        /// <param name="id">对象唯一标识符。</param>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用。</param>
        public ObjectNotFoundException(T id, string message, Exception innerException)
            : base(message, innerException)
        {
            ID = id;
        }
        /// <summary>
        /// 对象唯一标识符，对象编号。
        /// </summary>
        public T ID { get; protected set; }
    }
}
