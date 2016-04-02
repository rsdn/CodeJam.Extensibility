using System;
using System.Globalization;
using System.IO;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Base class for layouts.
	/// </summary>
	public abstract class RazorLayout
	{
		private TextWriter _writer;
		private Func<string> _bodyWriter;
		private Func<string, string> _sectionWriter;

		/// <summary>
		/// Write text to output.
		/// </summary>
		public void WriteText(TextWriter writer, Func<string> bodyWriter, Func<string, string> sectionWriter)
		{
			try
			{
				_writer = writer;
				_bodyWriter = bodyWriter;
				_sectionWriter = sectionWriter;

				Execute();
			}
			finally
			{
				_writer = null;
				_bodyWriter = null;
				_sectionWriter = null;
			}
		}

		/// <summary>
		/// Renders body of page.
		/// </summary>
		public string RenderBody()
		{
			return _bodyWriter();
		}

		/// <summary>
		/// Renders specified section.
		/// </summary>
		public string RenderSection(string name)
		{
			return _sectionWriter(name);
		}

		/// <summary>
		/// Executes template.
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// Write string literal to output.
		/// </summary>
		protected void WriteLiteral(string literal)
		{
			_writer.Write(literal);
		}

		/// <summary>
		/// Write object to output.
		/// </summary>
		protected void Write(object value)
		{
			if ((value == null))
				return;

			WriteLiteral(Convert.ToString(value, CultureInfo.InvariantCulture));
		}
	}
}