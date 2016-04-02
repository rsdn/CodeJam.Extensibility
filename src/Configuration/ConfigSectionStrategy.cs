using System;

using CodeJam.Extensibility.Registration;

using Rsdn.SmartApp;
using Rsdn.SmartApp.Configuration;

namespace CodeJam.Extensibility.Configuration
{
	/// <summary>
	/// Стратегия подключения секций конфигурации.
	/// </summary>
	public class ConfigSectionStrategy
		: RegKeyedElementsStrategy<Type, ConfigSectionInfo, ConfigSectionAttribute>
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ConfigSectionStrategy(IServicePublisher publisher)
			: base(publisher)
		{}

		/// <summary>
		/// Создать элемент.
		/// </summary>
		public override ConfigSectionInfo CreateElement(ExtensionAttachmentContext context, ConfigSectionAttribute attr)
		{
			return
				new ConfigSectionInfo(
					attr.Name,
					attr.Namespace,
					attr.AllowMerge,
					context.Type,
					attr.CreateSerializer(context.Type));
		}
	}
}