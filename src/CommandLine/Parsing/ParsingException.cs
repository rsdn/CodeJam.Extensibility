using System;
using System.Runtime.Serialization;

using CodeJam.Strings;

namespace CodeJam.Extensibility.CommandLine.Parsing
{
	///<summary>
	/// Parsing exception.
	///</summary>
	[Serializable]
	public class ParsingException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"/> class.
		/// </summary>
		public ParsingException()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public ParsingException(string message) : base(message)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="position">position, where error occurs</param>
		public ParsingException(string message, int position)
			: base(@"Error at ({1}): {0}.".FormatWith(message, position))
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ApplicationException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected ParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{}
	}
}