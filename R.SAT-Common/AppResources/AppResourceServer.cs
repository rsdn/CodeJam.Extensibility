using System;
using System.Linq;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Default implementation of <see cref="IAppResourceServer"/>
	/// </summary>
	public class AppResourceServer : IAppResourceServer
	{
		private readonly string _schemeName;
		private readonly Lazy<UriTemplateTable> _templateTable;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public AppResourceServer(IServiceProvider svcProvider, string schemeName)
		{
			_schemeName = schemeName;
			_templateTable =
				new Lazy<UriTemplateTable>(
					() =>
						new UriTemplateTable(
							new Uri(schemeName + "://"),
							svcProvider
								.GetRegisteredElements<AppResourceInfo>()
								.Select(
									info =>
										new
										{
											Template = new UriTemplate(info.UriTemplate),
											Resource = info.ResourceType.CreateInstance(svcProvider)
										})
								.ToDictionary(p => p.Template, p => p.Resource)));
		}

		/// <summary>
		/// Get application resource.
		/// </summary>
		public AppResourceResponse GetResource(AppResourceRequest request)
		{
			var table = _templateTable.Value;
			if (request.Uri.Scheme != _schemeName)
				return null;
			// Replace base address in uri because UriTemplateTable.BaseAddress always starts with http://localhost/
			var match =
				table.MatchSingle(
					new Uri(
						table.BaseAddress,
						request.Uri.GetComponents(UriComponents.HostAndPort | UriComponents.PathAndQuery, UriFormat.Unescaped)));
			return
				match == null
					? null
					: ((IAppResource)match.Data).GetResource(
						request,
						match
							.BoundVariables
							.AllKeys
							.ToDictionary(k => k, k => match.BoundVariables[k], StringComparer.OrdinalIgnoreCase));
		}
	}
}