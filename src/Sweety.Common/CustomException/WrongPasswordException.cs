/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      表示输入的密码与设定的密码不匹配导致认证失败的异常。
 *      
 * Members Index:
 *      class
 *          WrongPasswordException
 *          
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common
{
    using System;



    /// <summary>
    /// 密码不匹配的异常。
    /// </summary>
    [Serializable]
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(string message) : base(message) { }

        public WrongPasswordException(string message, Exception innerException) : base(message, innerException) { }

        public WrongPasswordException(string id, string password, string message)
            : this(id, password, message, null) { }

        public WrongPasswordException(string id, string password, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Id = id;
            this.Password = password;
        }

        public WrongPasswordException(string id, string password, byte canAttemptsTimes, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Id = id;
            this.Password = password;
            this.CanAttemptsTimes = canAttemptsTimes;
        }

        /// <summary>
        /// 获取产生此异常的ID（ID可能是登录名、邮箱、手机号等等）。
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// 获取产生此异常的明文密码。
        /// </summary>
        public string Password { get; private set; }
        /// <summary>
        /// 还可尝试次数。
        /// </summary>
        public byte CanAttemptsTimes { get; private set; }
    }
}
