namespace Rsdn.SmartApp
{
	/// <summary>
	/// Application resources server.
	/// </summary>
	public interface IAppResourceServer
	{
		/// <summary>
		/// Get application resource.
		/// </summary>
		AppResourceResponse GetResource(AppResourceRequest request);
	}
}