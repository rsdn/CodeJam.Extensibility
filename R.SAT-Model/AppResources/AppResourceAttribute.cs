using System;
using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Marks <see cref="IAppResource"/> implementation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IAppResource))]
	public class AppResourceAttribute : Attribute
	{
		private readonly string _uriTemplate;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		/// <param name="uriTemplate"></param>
		public AppResourceAttribute(string uriTemplate)
		{
			_uriTemplate = uriTemplate;
		}

		/// <summary>
		/// Resource URI template.
		/// </summary>
		public string UriTemplate
		{
			get { return _uriTemplate; }
		}
	}
}