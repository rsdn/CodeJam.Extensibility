using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	[UsedImplicitly]
	public class CustomParamsCtor
	{
		public CustomParamsCtor(IServiceProvider provider, string message)
		{
			Message = message;
		}

		public string Message { get; private set; }
	}
}