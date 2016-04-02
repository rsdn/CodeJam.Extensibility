using System;

namespace Rsdn.SmartApp.Instancing
{
	public class CustomParamsCtor
	{
		public CustomParamsCtor(IServiceProvider provider, string message)
		{
			Message = message;
		}

		public string Message { get; private set; }
	}
}