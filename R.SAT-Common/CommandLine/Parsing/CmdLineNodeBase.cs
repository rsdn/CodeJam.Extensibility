namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Base class for command line AST node
	///</summary>
	public abstract class CmdLineNodeBase
	{
		private readonly string _text;
		private readonly int _position;
		private readonly int _length;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		protected CmdLineNodeBase(string text, int position, int length)
		{
			_text = text;
			_position = position;
			_length = length;
		}

		/// <summary>
		/// Node text.
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		/// <summary>
		/// Node position.
		/// </summary>
		public int Position
		{
			get { return _position; }
		}

		/// <summary>
		/// Node length.
		/// </summary>
		public int Length
		{
			get { return _length; }
		}
	}
}