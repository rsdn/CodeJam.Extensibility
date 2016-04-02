using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	[UsedImplicitly]
	public class NoDefaultCtor
	{
		public NoDefaultCtor()
		{
		}

		public NoDefaultCtor(string message)
		{
		}
	}
}