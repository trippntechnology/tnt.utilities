using System;

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
		public static R Let<T, R>(this T it, Func<T, R> func) => func(it);

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
}
