using System.Collections.Generic;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// App resource provider.
	/// </summary>
	public interface IAppResource
	{
		/// <summary>
		/// Get resource.
		/// </summary>
		AppResourceResponse GetResource(AppResourceRequest request, IDictionary<string, string> vars);
	}
}