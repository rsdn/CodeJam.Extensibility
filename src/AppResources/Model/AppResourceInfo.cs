using System;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Info about app resource.
	/// </summary>
	public class AppResourceInfo
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceInfo(string uriTemplate, Type resourceType)
		{
			UriTemplate = uriTemplate;
			ResourceType = resourceType;
		}

		/// <summary>
		/// Resource URI template.
		/// </summary>
		public string UriTemplate { get; }

		/// <summary>
		/// Resource provider type.
		/// </summary>
		public Type ResourceType { get; }
	}
}