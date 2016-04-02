using System;

namespace CodeJam.Extensibility.Instancing
{
	/// <summary>
	/// Помечает конструктор по умолчанию, если класс содержит несколько публичных конструкторов.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor)]
	public class DefaultConstructorAttribute : Attribute
	{
	}
}