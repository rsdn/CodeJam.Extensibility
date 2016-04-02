using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

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