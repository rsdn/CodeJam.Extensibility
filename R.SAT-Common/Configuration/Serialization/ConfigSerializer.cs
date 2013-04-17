using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Rsdn.SmartApp.Configuration
{
	internal class ConfigSerializer : IDisposable
	{
		/*private static readonly object _settingsCreateFlag = new object();
		private static XmlReaderSettings _readerSettings;*/

		private readonly object _configLoadLockFlag = new object();
		private readonly IConfigDataProvider _dataProvider;

		private readonly HashSet<IConfigDataProvider> _providers =
			new HashSet<IConfigDataProvider>();

		private readonly VarsContainer _rootVars;

		private readonly Regex _varsRegex = new Regex(@"\$\(([A-Za-z][A-Za-z0-9\-_]*)\)",
			RegexOptions.Compiled);

		private Dictionary<SectionName, List<XElement>> _loadedSections;

		public ConfigSerializer(IConfigDataProvider dataProvider,
			IDictionary<string, string> rootVars)
		{
			_dataProvider = dataProvider;
			_rootVars = new VarsContainer(rootVars);
		}

		#region IDisposable Members
		public void Dispose()
		{
			foreach (var provider in _providers)
			{
				var disp = provider as IDisposable;
				if (disp != null)
					disp.Dispose();
			}
			ClearProviders();
		}
		#endregion

		/*private static XmlReader GetReader(XmlReader innerReader)
		{
			if (_readerSettings == null)
				lock (_settingsCreateFlag)
					if (_readerSettings == null)
					{
						_readerSettings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
						_readerSettings.Schemas.Add(
							ConfigXmlConstants.XmlNamespace,
							XmlReader.Create(typeof (ConfigXmlConstants)
								.Assembly.GetManifestResourceStream(
								ConfigXmlConstants.XmlSchemaResource)));
					}
			return XmlReader.Create(innerReader, _readerSettings);
		}*/

		public XElement[] GetSectionContent(SectionName ident)
		{
			if (_loadedSections == null)
				lock (_configLoadLockFlag)
					if (_loadedSections == null)
					{
						_loadedSections = new Dictionary<SectionName, List<XElement>>();
						LoadSections(_dataProvider, _rootVars);
					}
			List<XElement> contents;
			return _loadedSections.TryGetValue(ident, out contents) ? contents.ToArray() : null;
		}

		private void LoadSections(IConfigDataProvider provider, VarsContainer parentVars)
		{
			if (_providers.Contains(provider))
				throw new ApplicationException("Circular reference was found for '" + provider + "'");

			XDocument xDoc;
			using (var reader = provider.ReadData())
				xDoc = XDocument.Load(reader);

			var includes = new List<string>();
			var varsDic = new Dictionary<string, string>();
			var sections = new List<XElement>();
			if (xDoc.Root != null)
				foreach (var elem in xDoc.Root.Elements())
				{
					if (elem.Name.NamespaceName == ConfigXmlConstants.XmlNamespace)
						switch (elem.Name.LocalName)
						{
							case ConfigXmlConstants.IncludeTagName:
								{
									var trimmed = elem.Value;
									if (trimmed == "")
										throw new ConfigurationException(ConfigurationResources.IncludeEmptyMessage);
									includes.Add(trimmed);
									continue;
								}
							case ConfigXmlConstants.VariableTagName:
								{
									var name = elem.Attribute(ConfigXmlConstants.VariableNameAttribute);
									if (name == null || name.Value == "")
										throw new ConfigurationException(ConfigurationResources.VarNameEmptyMessage);
									if (varsDic.ContainsKey(name.Value) || parentVars.IsVarDefined(name.Value))
										throw new ConfigurationException(
											string.Format(ConfigurationResources.VarAlreadyDefined, name));
									varsDic.Add(name.Value, SubstVars(varsDic, elem.Value.Trim()));
									continue;
								}
						}
					sections.Add(elem);
				}

			var vars = new VarsContainer(varsDic, parentVars);

			foreach (var elem in sections)
			{
				SubstVars(vars, elem);
				var secName = SectionName.Create(elem);
				if (_loadedSections.ContainsKey(secName))
					_loadedSections[secName].Add(elem);
				else
					_loadedSections.Add(secName, new List<XElement>(new[] { elem }));
			}

			foreach (var include in includes)
				LoadSections(provider.ResolveInclude(include), vars);

			_providers.Add(provider);
			provider.ConfigChanged += ProviderConfigChanged;
		}

		private void SubstVars(VarsContainer vars, XElement elem)
		{
			var texts =
				elem
					.DescendantNodesAndSelf()
					.OfType<XText>();
			foreach (var text in texts)
				text.Value = SubstVars(vars, text.Value);
			foreach (var attr in elem.DescendantsAndSelf().Attributes())
				attr.Value = SubstVars(vars, attr.Value);
		}

		private string SubstVars(VarsContainer vars, string str)
		{
			return _varsRegex.Replace(str,
				match => vars.GetVar(match.Groups[1].Value));
		}

		private string SubstVars(IDictionary<string, string> vars, string str)
		{
			return _varsRegex.Replace(str,
				match =>
				{
					var name = match.Groups[1].Value;
					string value;
					if (!vars.TryGetValue(name, out value))
						throw new ArgumentException("Variable '" + name + "' is not defined.");
					return value;
				});
		}

		private void ProviderConfigChanged(IConfigDataProvider sender)
		{
			_loadedSections = null;
			ClearProviders();
			OnConfigChanged();
		}

		private void ClearProviders()
		{
			foreach (var provider in _providers)
				provider.ConfigChanged -= ProviderConfigChanged;
			_providers.Clear();
		}

		public event EventHandler<ConfigSerializer> ConfigChanged;

		private void OnConfigChanged()
		{
			if (ConfigChanged != null)
				ConfigChanged(this);
		}
	}
}