using System;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Описание секции конфигурации.
	/// </summary>
	public class ConfigSectionInfo : IKeyedElementInfo<Type>
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ConfigSectionInfo(string name, string ns, bool allowMerge, Type contractType,
			IConfigSectionSerializer serializer)
		{
			Name = name;
			Namespace = ns;
			AllowMerge = allowMerge;
			ContractType = contractType;
			Serializer = serializer;
		}

		/// <summary>
		/// Имя секции.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// XML_пространство имен.
		/// </summary>
		public string Namespace { get; private set; }

		/// <summary>
		/// Разрешено ли слияние секций в разных файлах.
		/// </summary>
		public bool AllowMerge { get; private set; }

		/// <summary>
		/// Тип контракта секции.
		/// </summary>
		public Type ContractType { get; private set; }

		/// <summary>
		/// Сериализатор.
		/// </summary>
		public IConfigSectionSerializer Serializer { get; private set; }

		/// <summary>
		/// Имя.
		/// </summary>
		Type IKeyedElementInfo<Type>.Key
		{
			get { return ContractType; }
		}
	}
}