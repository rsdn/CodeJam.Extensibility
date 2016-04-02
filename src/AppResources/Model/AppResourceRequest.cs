using System;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// App resource request.
	/// </summary>
	public class AppResourceRequest
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceRequest(Uri uri)
		{
			Uri = uri;
		}

		/// <summary>
		/// Request URI.
		/// </summary>
		public Uri Uri { get; }
	}
}