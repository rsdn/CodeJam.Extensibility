namespace Rsdn.SmartApp.Instancing
{
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