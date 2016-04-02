using System;
using System.IO;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// App resource response.
	/// </summary>
	public class AppResourceResponse
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceResponse(Func<Stream> streamCreator, string mimeType, long length, int status)
		{
			MimeType = mimeType;
			Length = length;
			Status = status;
			StreamCreator = streamCreator;
		}

		/// <summary>
		/// Resource MIME type
		/// </summary>
		public string MimeType { get; }

		/// <summary>
		/// Resource data length
		/// </summary>
		public long Length { get; }

		/// <summary>
		/// Responce HTTP status (200 - OK, 404 - not found etc).
		/// </summary>
		public int Status { get; }

		/// <summary>
		/// Response data stream creator.
		/// </summary>
		public Func<Stream> StreamCreator { get; }
	}
}