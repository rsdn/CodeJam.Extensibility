using System;

namespace CodeJam.Extensibility.Services
{
	internal class DisposableService : IDisposableService, IDisposable
	{
		#region Implementation of IDisposableService
		public bool Disposed { get; private set; }
		#endregion

		#region Implementation of IDisposable
		public void Dispose()
		{
			Disposed = true;
		}
		#endregion
	}
}