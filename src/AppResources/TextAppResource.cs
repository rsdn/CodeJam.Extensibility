using System;
using System.Collections.Generic;
using System.IO;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Text app resource.
	/// </summary>
	public abstract class TextAppResource : IAppResource
	{
		private IDictionary<string, string> _vars;
		private AppResourceRequest _request;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected TextAppResource(IServiceProvider svcProvider)
		{
			SvcProvider = svcProvider;
		}

		/// <summary>
		/// Resource MIME type.
		/// </summary>
		protected virtual string MimeType => "text/html";

		/// <summary>
		/// Resource length in bytes.
		/// </summary>
		protected virtual long Length => -1;

		/// <summary>
		/// Write text to output.
		/// </summary>
		protected abstract void WriteText(AppResourceRequest request, TextWriter writer);

		/// <summary>
		/// Request variables.
		/// </summary>
		public IDictionary<string, string> Vars => _vars;

		/// <summary>
		/// Request.
		/// </summary>
		protected AppResourceRequest Request => _request;

		/// <summary>
		/// Service provider.
		/// </summary>
		protected IServiceProvider SvcProvider { get; }

		AppResourceResponse IAppResource.GetResource(AppResourceRequest request, IDictionary<string, string> vars)
		{
			_request = request;
			_vars = vars;
			try
			{
				var ms = new MemoryStream();
				var writer = new StreamWriter(ms);
				WriteText(request, writer);
				writer.Flush();
				ms.Seek(0, SeekOrigin.Begin);
				return new AppResourceResponse(() => ms, MimeType, ms.Length, 200);
			}
			finally
			{
				_request = null;
				_vars = null;
			}
		}
	}
}