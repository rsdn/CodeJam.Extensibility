using System;
using System.Reactive.Disposables;
using System.Threading;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Реализация паттерна IDisposable для <see cref="ReaderWriterLock"/>
	/// </summary>
	public static class ReaderWriterLockHelper
	{
		/// <summary>
		/// Получить лок для читающего.
		/// </summary>
		public static IDisposable GetReaderLock(this ReaderWriterLockSlim readerWriterLock)
		{
			readerWriterLock.EnterReadLock();
			return Disposable.Create(readerWriterLock.ExitReadLock);
		}

		/// <summary>
		/// Получить лок для пишущего.
		/// </summary>
		public static IDisposable GetWriterLock(this ReaderWriterLockSlim readerWriterLock)
		{
			readerWriterLock.EnterWriteLock();
			return Disposable.Create(readerWriterLock.ExitWriteLock);
		}

		/// <summary>
		/// Получить лок для читающего с возможностью апгрейда до пишущего.
		/// </summary>
		public static IDisposable GetUpgradeableReaderLock(this ReaderWriterLockSlim readerWriterLock)
		{
			readerWriterLock.EnterUpgradeableReadLock();
			return Disposable.Create(readerWriterLock.ExitUpgradeableReadLock);
		}
	}
}