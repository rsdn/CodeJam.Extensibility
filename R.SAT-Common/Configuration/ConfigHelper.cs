using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.SmartApp.Configuration
{
	using SectionRegSvc = IRegKeyedElementsService<Type, ConfigSectionInfo>;

	/// <summary>
	/// Вспомогательный класс для работы с <see cref="ConfigService"/>.
	/// </summary>
	public static class ConfigHelper
	{
		/// <summary>
		/// Создать и инициализировать сервис указанным файлом.
		/// </summary>
		public static IConfigService CreateConfigService(
			this IServiceProvider provider,
			string fileName,
			IDictionary<string, string> externalVars)
		{
			return CreateConfigService(
				provider,
				new FileConfigDataProvider(fileName),
				externalVars);
		}

		/// <summary>
		/// Создать и инициализировать сервис указанным <see cref="IConfigDataProvider"/>.
		/// </summary>
		public static ConfigService CreateConfigService(
			this IServiceProvider provider,
			IConfigDataProvider dataProvider,
			IDictionary<string, string> externalVars)
		{
			var secSvc = provider.GetService<SectionRegSvc>();
			return new ConfigService(
				secSvc == null
					? EmptyArray<ConfigSectionInfo>.Value
					: secSvc.GetRegisteredElements(),
				dataProvider,
				externalVars);
		}

		/// <summary>
		/// Возвращает описатели секций по типам контрактов.
		/// </summary>
		public static IEnumerable<ConfigSectionInfo> GetSectionInfos(
			this IEnumerable<Type> sectionContracts)
		{
			return
				from contract in sectionContracts
				let attr =
					contract
						.GetCustomAttributes<ConfigSectionAttribute>(false)
						.FirstOrDefault() where attr != null
				select
					new ConfigSectionInfo(
						attr.Name,
						attr.Namespace,
						attr.AllowMerge,
						contract,
						attr.CreateSerializer(contract));
		}
	}
}
