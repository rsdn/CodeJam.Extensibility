using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
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