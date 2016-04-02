using System;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Атрибут, поменчающий секцию конфигурации.
	/// </summary>
	public abstract class ConfigSectionAttribute : Attribute
	{
		/// <summary>
		/// Инициализирует экземпляр. 
		/// </summary>
		protected ConfigSectionAttribute(string name, string ns)
		{
			Name = name;
			Namespace = ns;
			AllowMerge = false;
		}

		/// <summary>
		/// Имя секции.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Пространство имен.
		/// </summary>
		public string Namespace { get; private set; }

		/// <summary>
		/// Признак допустимости слияния секций.
		/// </summary>
		public bool AllowMerge { get; set; }

		/// <summary>
		/// Создать сериалайзер.
		/// </summary>
		public abstract IConfigSectionSerializer CreateSerializer(Type contractType);
	}
}