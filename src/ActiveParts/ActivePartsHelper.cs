using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Telper methods for active parts.
	/// </summary>
	public static class ActivePartsHelper
	{
		/// <summary>
		/// Create and publish active parts manager.
		/// </summary>
		public static ActivePartManager CreateAndPublishManager([NotNull] IServicePublisher servicePublisher)
		{
			if (servicePublisher == null)
				throw new ArgumentNullException("servicePublisher");

			var mgr = new ActivePartManager(servicePublisher);
			servicePublisher.Publish<IActivePartManager>(mgr);
			return mgr;
		}
	}
}