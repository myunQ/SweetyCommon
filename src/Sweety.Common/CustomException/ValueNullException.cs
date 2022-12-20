using System;

namespace Sweety.Common
{
    /// <summary>
    /// 值为<c>null</c>的异常。
    /// </summary>
    public class ValueNullException : Exception
    {
        /// <summary>
        /// 初始化 <see cref="ValueNullException"/> 类的新实例。
        /// </summary>
        public ValueNullException()
        { }
        /// <summary>
        /// 使用指定错误消息初始化 <see cref="ValueNullException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        public ValueNullException(string message)
            : base(message)
        { }
        /// <summary>
        /// 使用指定错误消息和对作为此异常原因的内部异常的引用来初始化 <see cref="ValueNullException"/> 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 <c>null</c> 引用（在 <c>Visual Basic</c> 中为 <c>Nothing</c>）。</param>
        public ValueNullException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
