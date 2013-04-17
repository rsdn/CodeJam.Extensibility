using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Info about app resource.
	/// </summary>
	public class AppResourceInfo
	{
		private readonly string _uriTemplate;
		private readonly Type _resourceType;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceInfo(string uriTemplate, Type resourceType)
		{
			_uriTemplate = uriTemplate;
			_resourceType = resourceType;
		}

		/// <summary>
		/// Resource URI template.
		/// </summary>
		public string UriTemplate
		{
			get { return _uriTemplate; }
		}

		/// <summary>
		/// Resource provider type.
		/// </summary>
		public Type ResourceType
		{
			get { return _resourceType; }
		}
	}
}