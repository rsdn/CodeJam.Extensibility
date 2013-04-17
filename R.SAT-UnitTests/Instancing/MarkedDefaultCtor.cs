namespace Rsdn.SmartApp.Instancing
{
	public class MarkedDefaultCtor
	{
		private readonly string _message;

		[DefaultConstructor]
		public MarkedDefaultCtor()
		{
			_message = "default";
		}

		public MarkedDefaultCtor(string message)
		{
			_message = message;
		}

		public string Message
		{
			get { return _message; }
		}
	}
}