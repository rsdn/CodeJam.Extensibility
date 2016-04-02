using JetBrains.Annotations;

namespace CodeJam.Extensibility.Instancing
{
	[UsedImplicitly]
	public class MultipleDefaultCtors
	{
		[DefaultConstructor]
		public MultipleDefaultCtors()
		{
		}

		[DefaultConstructor]
		public MultipleDefaultCtors(string message)
		{
		}
	}
}