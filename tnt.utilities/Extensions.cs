namespace TNT.Utilities;

/// <summary>
/// Extension methods
/// </summary>
internal static class Extensions
{
  /// <summary>
  /// Calls <paramref name="action"/> with <paramref name="it"/> as the parameter and return <paramref name="it"/>
  /// </summary>
  /// <typeparam name="T">Type of <paramref name="it"/></typeparam>
  /// <param name="it">Object calling extension method</param>
  /// <param name="action"><see cref="Action{T}"/> that takes <paramref name="it"/> as the parameter</param>
  /// <returns><paramref name="it"/></returns>
  public static T Also<T>(this T it, Action<T> action)
  {
    action(it);
    return it;
  }
}