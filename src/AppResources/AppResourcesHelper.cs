using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Helper methods for app resource infrastructure.
	/// </summary>
	public static class AppResourcesHelper
	{
		/// <summary>
		/// Creates app resource providers attachment strategy.
		/// </summary>
		public static IExtensionAttachmentStrategy CreateStrategy(IServicePublisher publisher)
		{
			return
				publisher.CreateStrategy<AppResourceInfo, AppResourceAttribute>(
					(ctx, attr) => new AppResourceInfo(attr.UriTemplate, ctx.Type));
		}

		/// <summary>
		/// Creates app resource server implementation.
		/// </summary>
		public static IAppResourceServer CreateResourceServer(IServiceProvider provider, string schemeName)
		{
			return new AppResourceServer(provider, schemeName);
		}

		/// <summary>
		/// Creates and publish app resource server implementation.
		/// </summary>
		public static IAppResourceServer CreateAndPublishResourceServer(IServicePublisher publisher, string schemeName)
		{
			var srv = CreateResourceServer(publisher, schemeName);
			publisher.Publish(srv);
			return srv;
		}
	}
}