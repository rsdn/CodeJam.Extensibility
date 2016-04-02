using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	[UsedImplicitly]
	public class OnlyProviderParam
	{
		public OnlyProviderParam(IServiceProvider provider)
		{
			Provider = provider;
		}

		public IServiceProvider Provider { get; }
	}
}