using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	[UsedImplicitly]
	public class MarkedDefaultCtor
	{
		[DefaultConstructor]
		public MarkedDefaultCtor()
		{
			Message = "default";
		}

		public MarkedDefaultCtor(string message)
		{
			Message = message;
		}

		public string Message { get; }
	}
}