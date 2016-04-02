using System;
using System.Collections.Generic;
using System.IO;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Text app resource.
	/// </summary>
	public abstract class TextAppResource : IAppResource
	{
		private readonly IServiceProvider _svcProvider;
		private IDictionary<string, string> _vars;
		private AppResourceRequest _request;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected TextAppResource(IServiceProvider svcProvider)
		{
			_svcProvider = svcProvider;
		}

		/// <summary>
		/// Resource MIME type.
		/// </summary>
		protected virtual string MimeType { get { return "text/html"; } }

		/// <summary>
		/// Resource length in bytes.
		/// </summary>
		protected virtual long Length { get { return -1; } }

		/// <summary>
		/// Write text to output.
		/// </summary>
		protected abstract void WriteText(AppResourceRequest request, TextWriter writer);

		/// <summary>
		/// Request variables.
		/// </summary>
		public IDictionary<string, string> Vars
		{
			get { return _vars; }
		}

		/// <summary>
		/// Request.
		/// </summary>
		protected AppResourceRequest Request
		{
			get { return _request; }
		}

		/// <summary>
		/// Service provider.
		/// </summary>
		protected IServiceProvider SvcProvider
		{
			get { return _svcProvider; }
		}

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