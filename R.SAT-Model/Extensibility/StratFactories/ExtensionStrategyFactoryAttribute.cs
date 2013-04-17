using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Атрибут, помечающий реализацию <see cref="IExtensionStrategyFactory"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IExtensionStrategyFactory))]
	public class ExtensionStrategyFactoryAttribute : Attribute
	{
	}
}