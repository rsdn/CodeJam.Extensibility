using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Делегат, создающий сервис.
	/// </summary>
	[NotNull]
	public delegate object ServiceCreator(
		[NotNull] Type serviceType,
		[NotNull] IServicePublisher publisher);
}