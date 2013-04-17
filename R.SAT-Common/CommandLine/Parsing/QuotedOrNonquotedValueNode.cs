namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Quoted or nonquoted value;
	/// </summary>
	public class QuotedOrNonquotedValueNode : CmdLineNodeBase
	{
		private readonly bool _quoted;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public QuotedOrNonquotedValueNode(
			string text,
			int position,
			int length,
			bool quoted) : base(text, position, length)
		{
			_quoted = quoted;
		}

		/// <summary>
		/// True, if value quoted.
		/// </summary>
		public bool Quoted
		{
			get { return _quoted; }
		}
	}
}