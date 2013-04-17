using System;
using System.IO;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// App resource response.
	/// </summary>
	public class AppResourceResponse
	{
		private readonly string _mimeType;
		private readonly long _length;
		private readonly int _status;
		private readonly Func<Stream> _streamCreator;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceResponse(Func<Stream> streamCreator, string mimeType, long length, int status)
		{
			_mimeType = mimeType;
			_length = length;
			_status = status;
			_streamCreator = streamCreator;
		}

		/// <summary>
		/// Resource MIME type
		/// </summary>
		public string MimeType
		{
			get { return _mimeType; }
		}

		/// <summary>
		/// Resource data length
		/// </summary>
		public long Length
		{
			get { return _length; }
		}

		/// <summary>
		/// Responce HTTP status (200 - OK, 404 - not found etc).
		/// </summary>
		public int Status
		{
			get { return _status; }
		}

		/// <summary>
		/// Response data stream creator.
		/// </summary>
		public Func<Stream> StreamCreator
		{
			get { return _streamCreator; }
		}
	}
}