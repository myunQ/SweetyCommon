namespace Sweety.Common
{
    /// <summary>
    /// 封装一个方法，该方法具有两个参数（第一个参数按引用传递值）且不返回值。
    /// </summary>
    /// <typeparam name="T1">此委托封装的方法的第一个参数的类型。这是按引用传递值。</typeparam>
    /// <typeparam name="T2">此委托封装的方法的第二个参数的类型。这是逆变类型参数。 即，可以使用指定的类型，也可以使用派生程度较低的任何类型。</typeparam>
    /// <param name="arg1">此委托封装的方法的第一个参数。</param>
    /// <param name="arg2">此委托封装的方法的第二个参数。</param>
    public delegate void RefT1Action<T1,in T2>(ref T1 arg1, T2 arg2);

    /// <summary>
    /// 封装一个方法，该方法具有三个参数（第一个参数按引用传递值）且不返回值。
    /// </summary>
    /// <typeparam name="T1">此委托封装的方法的第一个参数的类型。这是按引用传递值。</typeparam>
    /// <typeparam name="T2">此委托封装的方法的第二个参数的类型。这是逆变类型参数。 即，可以使用指定的类型，也可以使用派生程度较低的任何类型。</typeparam>
    /// <typeparam name="T3">此委托封装的方法的第三个参数的类型。这是逆变类型参数。 即，可以使用指定的类型，也可以使用派生程度较低的任何类型。</typeparam>
    /// <param name="arg1">此委托封装的方法的第一个参数。</param>
    /// <param name="arg2">此委托封装的方法的第二个参数。</param>
    /// <param name="arg3">此委托封装的方法的第三个参数。</param>
    public delegate void RefT1Action<T1, in T2, in T3>(ref T1 arg1, T2 arg2, T3 arg3);
}
