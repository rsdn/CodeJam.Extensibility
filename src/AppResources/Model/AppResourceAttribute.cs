using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Marks <see cref="IAppResource"/> implementation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IAppResource))]
	public class AppResourceAttribute : Attribute
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		/// <param name="uriTemplate"></param>
		public AppResourceAttribute(string uriTemplate)
		{
			UriTemplate = uriTemplate;
		}

		/// <summary>
		/// Resource URI template.
		/// </summary>
		public string UriTemplate { get; }
	}
}