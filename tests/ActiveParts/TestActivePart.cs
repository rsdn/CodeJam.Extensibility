using System;

using Rsdn.SmartApp;

namespace CodeJam.Extensibility.ActiveParts
{
	[ActivePart]
	public class TestActivePart : IActivePart, IDisposable
	{
		public bool Activated { get; private set; }

		public bool Disposed { get; private set; }

		public void Activate()
		{
			Activated = true;
		}

		public void Passivate()
		{
			Activated = false;
		}

		#region Implementation of IDisposable
		public void Dispose()
		{
			Disposed = true;
		}
		#endregion
	}
}