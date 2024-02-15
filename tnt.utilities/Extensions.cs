namespace TNT.Utilities
{
  /// <summary>
  /// Extension methods
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Calls <paramref name="func"/> with <paramref name="it"/> as the parameter and returns the value returned
    /// by <paramref name="func"/>
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="it"/></typeparam>
    /// <typeparam name="R">Type of return value</typeparam>
    /// <param name="it">Object calling extension method</param>
    /// <param name="func"><see cref="Func{T, TResult}"/> that takes <paramref name="it"/> as the parameter
    /// and return value of type <typeparamref name="R"/></param>
    /// <returns>The value returned by <paramref name="func"/></returns>
    [Obsolete("Use TNT.Commons.Let")]
    public static R Let<T, R>(this T it, Func<T, R> func) => func(it);

    /// <summary>
    /// Calls <paramref name="action"/> with <paramref name="it"/> as the parameter and return <paramref name="it"/>
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="it"/></typeparam>
    /// <param name="it">Object calling extension method</param>
    /// <param name="action"><see cref="Action{T}"/> that takes <paramref name="it"/> as the parameter</param>
    /// <returns><paramref name="it"/></returns>
    [Obsolete("Use TNT.Commons.Also")]
    public static T Also<T>(this T it, Action<T> action)
    {
      action(it);
      return it;
    }

    /// <summary>
    /// Calls <paramref name="action"/> when <typeparamref name="T1"/> can be cast to <typeparamref name="T2"/> and 
    /// passes<typeparamref name="T2"/> to <paramref name="action"/>
    /// </summary>
    /// <typeparam name="T1">Base type</typeparam>
    /// <typeparam name="T2">Super type</typeparam>
    /// <param name="obj">Object that needs to be cast</param>
    /// <param name="action">Action to take </param>
    [Obsolete("Use TNT.Commons.whenType")]
    public static void whenType<T1, T2>(this T1 obj, Action<T2> action)
    {
      if (obj is T2 value) action(value);
    }
  }
}