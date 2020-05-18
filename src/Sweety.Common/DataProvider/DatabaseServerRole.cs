/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  表示数据库服务器角色。
 * 
 * Members Index:
 *      enum
 *          DatabaseServerRole
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.DataProvider
{
    /// <summary>
    /// 数据库服务器角色
    /// </summary>
    public enum DatabaseServerRole : byte
    {
        /// <summary>
        /// 主服务器
        /// </summary>
        Master,
        /// <summary>
        /// 从服务器
        /// </summary>
        Slave,
        /// <summary>
        /// 主服务器或从服务器
        /// </summary>
        MasterOrSlave
    }
}
