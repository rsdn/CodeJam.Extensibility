using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Делегат, создающий сервис.
	/// </summary>
	[NotNull]
	public delegate object ServiceCreator(
		[NotNull] Type serviceType,
		[NotNull] IServicePublisher publisher);
}