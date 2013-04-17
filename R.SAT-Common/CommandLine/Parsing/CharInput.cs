using System;

namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Character input.
	///</summary>
	internal class CharInput : ICharInput
	{
		///<summary>
		/// End of file char.
		///</summary>
		public const char EOF = '\0';

		private readonly string _source;
		private readonly int _position;
		private readonly char _current;

		///<summary>
		/// Initialize instance.
		///</summary>
		public CharInput(string source) : this(source, 0)
		{}

		private CharInput(string source, int position)
		{
			_source = source;
			_position = position;
			_current = position < source.Length ? source[position] : EOF;
		}

		#region Implementation of ICharInput
		public char Current
		{
			get { return _current; }
		}

		public int Position
		{
			get { return _position; }
		}

		public ICharInput GetNext()
		{
			if (_current == EOF)
				throw new InvalidOperationException(@"Atempt to read beyond end of file");
			return new CharInput(_source, _position + 1);
		}
		#endregion
	}
}