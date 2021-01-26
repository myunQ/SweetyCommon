/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      表示不兼容的异常类。
 *      
 * Members Index:
 *      class
 *          IncompatibleException
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;


    /// <summary>
    /// 表示不兼容的异常类。
    /// </summary>
    [Serializable]
    public class IncompatibleException<T1, T2> : Exception
    {
        T1 _a;
        T2 _b;

        /// <summary>
        /// 初始化类的新实例。
        /// </summary>
        /// <param name="a">不兼容的两个对象的其中一个。</param>
        /// <param name="b">不兼容的两个对象的另一个。</param>
        public IncompatibleException(T1 a, T2 b)
            : this(a, b, null, null) { }
        /// <summary>
        /// 使用指定的错误消息初始化 <see cref="IncompatibleException{T1, T2}"/> 类的新实例。
        /// </summary>
        /// <param name="a">不兼容的两个对象的其中一个。</param>
        /// <param name="b">不兼容的两个对象的另一个。</param>
        /// <param name="message">描述错误的消息。</param>
        public IncompatibleException(T1 a, T2 b, string message)
            : this(a, b, message, null) { }
        /// <summary>
        /// 使用两个不兼容的对象和指定错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="IncompatibleException{T1, T2}"/> 类的新实例。
        /// </summary>
        /// <param name="a">不兼容的两个对象的其中一个。</param>
        /// <param name="b">不兼容的两个对象的另一个。</param>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用（在 <c>Visual Basic</c> 中为 <c>Nothing</c>）。</param>
        public IncompatibleException(T1 a, T2 b, string message, Exception innerException)
            : base(message, innerException)
        {
            _a = a;
            _b = b;
        }

        /// <summary>
        /// 不兼容的两个对象的其中一个。
        /// </summary>
        public T1 A
        {
            get
            {
                return _a;
            }
        }
        /// <summary>
        /// 不兼容的两个对象的另一个。
        /// </summary>
        public T2 B
        {
            get
            {
                return _b;
            }
        }
    }
}
