using System;
using System.Threading;

namespace TNT.Utilities
{
	/// <summary>
	/// Wraps a <see cref="System.Threading.Mutex"/>
	/// </summary>
	public class MutexWrapper
	{
		private Mutex mutex = null;

		/// <summary>
		/// Initialization constructor
		/// </summary>
		/// <param name="nameKnownBySystem">Mutex name</param>
		public MutexWrapper(string nameKnownBySystem)
		{
			this.mutex = new Mutex(false, nameKnownBySystem);
		}

		/// <summary>
		/// Critical section to execute with mutex protection
		/// </summary>
		/// <param name="waitTimeout">Milliseconds to wait to obtain the mutex</param>
		/// <param name="action">Delegate that will be executed within the mutex's protection</param>
		public void CriticalSection(int waitTimeout, Action action)
		{
			var mutexObtained = false;

			try
			{
				try
				{
					if (!mutex.WaitOne(waitTimeout))
					{
						throw new Exception("Unable to obtain mutex");
					}
				}
				catch (AbandonedMutexException)
				{
				}

				mutexObtained = true;
				action();
			}
			finally
			{
				if (mutexObtained)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		/// <summary>
		/// Critical section to execute with mutex protection
		/// </summary>
		/// <param name="action">Delegate that will be executed within the mutex's protection</param>
		public void CriticalSection(Action action)
		{
			CriticalSection(-1, action);
		}
	}
}
