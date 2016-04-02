using System;

using CodeJam.Extensibility.Registration;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Информация о зарегистрированной фабрике стратегий.
	/// </summary>
	public class ExtensionStrategyFactoryInfo : IKeyedElementInfo<Type>
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ExtensionStrategyFactoryInfo(Type type)
		{
			Type = type;
		}

		/// <summary>
		/// Тип фабрики.
		/// </summary>
		public Type Type { get; }

		#region IKeyedElementInfo<Type> Members
		/// <summary>
		/// Ключ.
		/// </summary>
		Type IKeyedElementInfo<Type>.Key => Type;
		#endregion
	}
}