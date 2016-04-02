using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.StratFactories
{
	/// <summary>
	/// »нтерфейс класса, предоставл€ющего специфические стратегии.
	/// </summary>
	public interface IExtensionStrategyFactory
	{
		/// <summary>
		/// —оздает стратегии.
		/// </summary>
		[NotNull]
		IExtensionAttachmentStrategy[] CreateStrategies([NotNull] IServiceProvider provider);
	}
}