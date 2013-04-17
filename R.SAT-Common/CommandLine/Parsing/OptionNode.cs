namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Node for command line option.
	/// </summary>
	public class OptionNode : CmdLineNodeBase
	{
		private readonly QuotedOrNonquotedValueNode _value;
		private readonly OptionType _type;
		private readonly bool _boolValue;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		private OptionNode(string text, int position, int length, OptionType type)
			: base(text, position, length)
		{
			_type = type;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionNode(
			string text,
			int position,
			int length)
			: this(text, position, length, OptionType.Valueless)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionNode(
			string text,
			int position,
			int length,
			bool boolValue)
			: this(text, position, length, OptionType.Bool)
		{
			_boolValue = boolValue;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionNode(
			string text,
			int position,
			int length,
			QuotedOrNonquotedValueNode value)
			: this(text, position, length, OptionType.Value)
		{
			_value = value;
		}

		/// <summary>
		/// Type of node.
		/// </summary>
		public OptionType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Boolean value.
		/// </summary>
		public bool BoolValue
		{
			get { return _boolValue; }
		}

		/// <summary>
		/// Option value.
		/// </summary>
		public QuotedOrNonquotedValueNode Value
		{
			get { return _value; }
		}
	}
}