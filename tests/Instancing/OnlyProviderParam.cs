using System;

namespace Rsdn.SmartApp.Instancing
{
	public class OnlyProviderParam
	{
		private readonly IServiceProvider _provider;

		public OnlyProviderParam(IServiceProvider provider)
		{
			_provider = provider;
		}

		public IServiceProvider Provider
		{
			get { return _provider; }
		}
	}
}