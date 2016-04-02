using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Сериализует секцию при помощи <see cref="XmlSerializer"/>
	/// </summary>
	public class XmlSectionSerializer : IConfigSectionSerializer
	{
		private static readonly ElementsCache<Type, XmlSerializer> _xmlSerializers =
			new ElementsCache<Type, XmlSerializer>(type => new XmlSerializer(type));

		private readonly Type _contractType;
		private readonly XmlSchema _schema;

		/// <summary>
		/// Инициализирует экземпляр типом контракта и схемой.
		/// </summary>
		public XmlSectionSerializer(Type contractType, XmlSchema schema)
		{
			_contractType = contractType;
			_schema = schema;
		}

		/// <summary>
		/// Инициализирует экземпляр типом контракта.
		/// </summary>
		public XmlSectionSerializer(Type contractType) : this(contractType, null)
		{}

		#region IConfigSectionSerializer Members
		/// <summary>
		/// Десериализовать секцию.
		/// </summary>
		public object Deserialize(XmlReader reader)
		{
			var xs = _xmlSerializers.Get(_contractType);
			if (_schema != null)
			{
				var rdrSettings = new XmlReaderSettings();
				rdrSettings.Schemas.Add(_schema);
				rdrSettings.ValidationType = ValidationType.Schema;
				reader = XmlReader.Create(reader, rdrSettings);
			}
			return xs.Deserialize(reader);
		}

		/// <summary>
		/// Создать секцию по умолчанию.
		/// </summary>
		public object CreateDefaultSection()
		{
			return Activator.CreateInstance(_contractType);
		}
		#endregion
	}
}