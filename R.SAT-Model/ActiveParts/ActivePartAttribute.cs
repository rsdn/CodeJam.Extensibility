using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Помечает active part класс.
	/// </summary>
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IActivePart))]
	public class ActivePartAttribute : Attribute
	{
	}
}