/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 *
 * 因为 Visual Studio 自动生成的 Sweety.Common.Properties.Localization 类型的访问修饰符是 internal 。
 * 为了让外部程序集（例如：Sweety.Common.DataProvider.SqlServer.dll）可以访问 Sweety.Common.dll 程序集内的本地化资源，特地创建此类。
 * 
 * Members Index:
 *      static class
 *          LocalizationResources
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Properties
{
    using System;
    using System.Globalization;


    /// <summary>
    /// 提供外部程序集访问<see cref="Sweety.Common"/>程序集的本地化资源资源的方法。
    /// </summary>
    public static class LocalizationResources
    {
        /// <summary>
        /// 返回指定的字符串资源的值。
        /// </summary>
        /// <param name="name">要检索的资源的名称。</param>
        /// <returns>为调用方的当前 UI 区域性本地化的资源的值，如果在资源集中找不到 <paramref name="name"/> 则返回 <paramref name="name"/>。</returns>
        public static string GetString(string name)
        {
            return Localization.ResourceManager.GetString(name.ToLowerInvariant()) ?? name;
        }

        /// <summary>
        /// 返回为指定区域性本地化的字符串资源的值。
        /// </summary>
        /// <param name="name">要检索的资源的名称。</param>
        /// <param name="culture">一个对象，表示为其本地化资源的区域性。</param>
        /// <returns>为指定区域性本地化的资源的值，如果在资源集中找不到 <paramref name="name"/> 则返回 <paramref name="name"/>。</returns>
        public static string GetString(string name, CultureInfo culture)
        {
            return Localization.ResourceManager.GetString(name.ToLowerInvariant(), culture) ?? name;
        }

        #region 同步 Sweety.Common.Properties.Localization 里自动生成的属性。
        public static string arguments_X_and_Y_cannot_be_null_at_the_same_time => Localization.arguments_X_and_Y_cannot_be_null_at_the_same_time;
        public static string the_database_connection_string_cannot_be_null_or_empty => Localization.the_database_connection_string_cannot_be_null_or_empty;

        public static string the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction => Localization.the_transaction_was_rollbacked_or_commited__please_provide_an_open_transaction;

        public static string the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance => Localization.the_database_connection_provided_and_the_database_connection_used_by_the_transaction_must_be_the_same_instance;

        public static string this_instance_only_accepts_database_connection_objects_of_type_XXX => Localization.this_instance_only_accepts_database_connection_objects_of_type_XXX;

        public static string this_instance_only_accepts_transaction_objects_of_type_XXX => Localization.this_instance_only_accepts_transaction_objects_of_type_XXX;

        public static string the_value_ranges_from_XXX_to_XXX => Localization.the_value_ranges_from_XXX_to_XXX;

        public static string the_value_ranges_from_XXX_to_XXX_milliseconds => Localization.the_value_ranges_from_XXX_to_XXX_milliseconds;
        #endregion 同步 Sweety.Common.Properties.Localization 里自动生成的属性。
    }
}
