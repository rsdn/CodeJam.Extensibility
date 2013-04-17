namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Parse result.
	///</summary>
	public class ParseResult<T>
	{
		private readonly T _result;
		private readonly ICharInput _inputRest;

		///<summary>
		/// Initialize instance with result.
		///</summary>
		public ParseResult(T result, ICharInput inputRest)
		{
			_result = result;
			_inputRest = inputRest;
		}

		/// <summary>
		/// Parsing result.
		/// </summary>
		public T Result
		{
			get { return _result; }
		}

		/// <summary>
		/// Input rest.
		/// </summary>
		public ICharInput InputRest
		{
			get { return _inputRest; }
		}
	}
}