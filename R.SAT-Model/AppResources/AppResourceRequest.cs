using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// App resource request.
	/// </summary>
	public class AppResourceRequest
	{
		private readonly Uri _uri;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceRequest(Uri uri)
		{
			_uri = uri;
		}

		/// <summary>
		/// Request URI.
		/// </summary>
		public Uri Uri
		{
			get { return _uri; }
		}
	}
}