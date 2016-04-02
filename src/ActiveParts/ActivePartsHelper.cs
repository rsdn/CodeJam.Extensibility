using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
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
				throw new ArgumentNullException(nameof(servicePublisher));

			var mgr = new ActivePartManager(servicePublisher);
			servicePublisher.Publish<IActivePartManager>(mgr);
			return mgr;
		}
	}
}