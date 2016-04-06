using System;

using CodeJam.Services;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Хелперный класс для работы с сервисами.
	/// </summary>
	public static partial class ServicesHelper
	{
		/// <summary>
		/// Присваивает экземпляры сервисов специально размеченным полям.
		/// </summary>
		public static void AssignServices(this object instance, IServiceProvider provider)
		{
			AssignHelper.Assign(instance, provider);
		}

		/// <summary>
		/// Creates service publishing strategy.
		/// </summary>
		public static IExtensionAttachmentStrategy CreateServiceStrategy([NotNull] IServicePublisher publisher)
		{
			if (publisher == null)
				throw new ArgumentNullException(nameof(publisher));
			return new ServicePublishingStrategy(publisher);
		}
	}
}