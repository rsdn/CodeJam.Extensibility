using System;
using System.Collections.Generic;
using System.Linq;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Стандартная реализация <see cref="IConfigService"/>
	/// </summary>
	public class ConfigService : IConfigService, IDisposable
	{
		private readonly Dictionary<Type, ConfigSectionInfo> _sectionInfos =
			new Dictionary<Type, ConfigSectionInfo>();
		private readonly IConfigDataProvider _dataProvider;
		private readonly ElementsCache<ConfigSectionInfo, object> _sections;
		private readonly ConfigSerializer _serializer;

		/// <summary>
		/// Инициализирует экземпляр поставщиком данных конфигурации.
		/// </summary>
		public ConfigService(
			IEnumerable<ConfigSectionInfo> sectionInfos,
			IConfigDataProvider dataProvider,
			IDictionary<string, string> externalVars)
		{
			if (sectionInfos == null)
				throw new ArgumentNullException("sectionInfos");

			_sectionInfos = sectionInfos.ToDictionary(info => info.ContractType);
			_dataProvider = dataProvider;
			_serializer = new ConfigSerializer(_dataProvider, externalVars);
			_sections = new ElementsCache<ConfigSectionInfo, object>(DeserializeSection);
			_serializer.ConfigChanged += SerializerConfigChanged;
		}

		/// <summary>
		/// Инициализирует экземпляр путем к файлу конфигурации.
		/// </summary>
		public ConfigService(
			IEnumerable<ConfigSectionInfo> sectionInfos,
			string configFile,
			IDictionary<string, string> externalVars)
			: this(sectionInfos, new FileConfigDataProvider(configFile), externalVars)
		{
		}

		#region IConfigService Members
		/// <summary>
		/// Вызывается при изменении конфигурации.
		/// </summary>
		public event EventHandler<IConfigService> ConfigChanged;

		/// <summary>
		/// Получить содержимое секции.
		/// </summary>
		public T GetSection<T>()
		{
			var contract = typeof (T);
			if (!_sectionInfos.ContainsKey(contract))
				throw new ArgumentException("Section of type '" + contract.FullName + "' not exists");
			var secInfo = _sectionInfos[contract];
			return (T) _sections.Get(secInfo);
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// See <see cref="IDisposable.Dispose"/>
		/// </summary>
		public void Dispose()
		{
			_serializer.Dispose();
		}
		#endregion

		/// <summary>
		/// Вызывает обработчики события <see cref="ConfigChanged"/>
		/// </summary>
		protected virtual void OnConfigChanged()
		{
			if (ConfigChanged != null)
				ConfigChanged(this);
		}

		private object DeserializeSection(ConfigSectionInfo sectionInfo)
		{
			var elems = _serializer.GetSectionContent(SectionName.Create(sectionInfo));
			if (elems.Length <= 0)
				return sectionInfo.Serializer.CreateDefaultSection();
			var elem = elems[0];
			if (elems.Length > 1)
				if (!sectionInfo.AllowMerge)
					throw new ApplicationException(
						"Section '{0}' does not allow merge".FormatStr(sectionInfo.Name));
				else
					for (var i = 1; i < elems.Length; i++)
						foreach (var sibChild in elems[i].Nodes())
							elem.Add(sibChild, true);
			return sectionInfo.Serializer.Deserialize(elem.CreateReader());
		}

		private void SerializerConfigChanged(ConfigSerializer sender)
		{
			_sections.Reset();
			OnConfigChanged();
		}
	}
}