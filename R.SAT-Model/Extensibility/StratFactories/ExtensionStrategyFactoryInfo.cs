using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Информация о зарегистрированной фабрике стратегий.
	/// </summary>
	public class ExtensionStrategyFactoryInfo : IKeyedElementInfo<Type>
	{
		private readonly Type _type;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ExtensionStrategyFactoryInfo(Type type)
		{
			_type = type;
		}

		/// <summary>
		/// Тип фабрики.
		/// </summary>
		public Type Type
		{
			get { return _type; }
		}

		#region IKeyedElementInfo<Type> Members
		/// <summary>
		/// Ключ.
		/// </summary>
		Type IKeyedElementInfo<Type>.Key
		{
			get { return Type; }
		}
		#endregion
	}
}