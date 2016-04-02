namespace CodeJam.Extensibility.CommandLine.Parsing
{
	///<summary>
	/// Base class for command line AST node
	///</summary>
	public abstract class CmdLineNodeBase
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		protected CmdLineNodeBase(string text, int position, int length)
		{
			Text = text;
			Position = position;
			Length = length;
		}

		/// <summary>
		/// Node text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Node position.
		/// </summary>
		public int Position { get; }

		/// <summary>
		/// Node length.
		/// </summary>
		public int Length { get; }
	}
}