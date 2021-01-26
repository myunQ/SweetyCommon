/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      表示可以输出异常信息到客户端的异常类。用来表示某个异常对象的 Message 属性的值可以输出给客户端。
 *      
 * Members Index:
 *      class
 *          CanOutputException
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;


    /// <summary>
    /// 表示可以输出异常信息到客户端的异常类。
    /// </summary>
    /// <remarks>
    /// 不建议抛出此异常或捕获此异常。
    /// 此异常用于放置在其它异常的 <see cref="Exception.InnerException"/> 属性中，表示异常的 <see cref="Exception.Message"/> 属性的内容可以直接输出给客户端使用。
    /// </remarks>
    /// <example>
    /// <code>
    /// throw new ArgumentException("message content", "param name", CanOutputException.DefaultInstance);
    /// </code>
    /// </example>
    [Serializable]
    public class CanOutputException : Exception
    {
        readonly int _errorCode;

        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly CanOutputException DefaultInstance = new CanOutputException();

        /// <summary>
        /// 初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        public CanOutputException()
            : this(0, String.Empty) { }
        /// <summary>
        /// 使用指定的异常码初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        /// <param name="errorCode">表示异常的代码。</param>
        public CanOutputException(int errorCode)
            : this(errorCode, String.Empty) { }
        /// <summary>
        /// 使用指定的错误消息初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public CanOutputException(string message)
            : this(0, message) { }
        /// <summary>
        /// 使用指定的异常码和错误消息初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        /// <param name="errorCode">表示异常的代码。</param>
        /// <param name="message">描述错误的消息。</param>
        public CanOutputException(int errorCode, string message)
            : base(message ?? String.Empty)
        {
            _errorCode = errorCode;
        }
        /// <summary>
        /// 使用两个不兼容的种类和指定错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用（在 <c>Visual Basic</c> 中为 <c>Nothing</c>）。</param>
        public CanOutputException(string message, Exception innerException)
            : this(0, message ?? String.Empty, innerException) { }
        /// <summary>
        /// 使用指定的异常码和错误消息初始化 <see cref="CanOutputException"/> 类的新实例。
        /// </summary>
        /// <param name="errorCode">表示异常的代码。</param>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用（在 <c>Visual Basic</c> 中为 <c>Nothing</c>）。</param>
        public CanOutputException(int errorCode, string message, Exception innerException)
            : base(message ?? String.Empty, innerException)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// 获取表示异常的代码。
        /// </summary>
        public int ErrorCode
        {
            get { return _errorCode; }
        }

        /// <summary>
        /// 尝试从 <paramref name="exception"/> 或 <c>exception.InnerException</c> 中获取 <see cref="CanOutputException"/> 类的实例。
        /// </summary>
        /// <param name="exception">要从中查找 <see cref="CanOutputException"/> 类实例的异常对象。</param>
        /// <returns><see cref="CanOutputException"/> 对象实例或 <c>null</c>。</returns>
        public static CanOutputException TryGet(Exception exception)
        {
            if (exception != null)
            {
                CanOutputException result = exception as CanOutputException;
                if (result != null) return result;

                while (exception.InnerException != null)
                {
                    result = exception.InnerException as CanOutputException;
                    if (result != null) return result;

                    exception = exception.InnerException;
                }
            }
            return null;
        }

        /// <summary>
        /// 尝试从 <paramref name="exception"/> 或 <c>exception.InnerException</c> 中获取 <see cref="CanOutputException"/> 类的实例。
        /// </summary>
        /// <param name="exception">要从中查找 <see cref="CanOutputException"/> 类实例的异常对象。</param>
        /// <param name="result">当此方法返回时，如果转换成功，则包含此 <paramref name="exception"/> 中的 <see cref="CanOutputException"/> 对象实例，否则为 <c>null</c>。</param>
        /// <returns>获取到 <see cref="CanOutputException"/> 对象实例则返回 <c>true</c> 否则返回 <c>false</c>。</returns>
        public static bool TryGet(Exception exception, out CanOutputException result)
        {
            result = TryGet(exception);
            return result != null;
        }
    }
}
