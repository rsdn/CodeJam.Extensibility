using System;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Помечает секцию конфигурации с использованием <see cref="XmlSerializer"/>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class XmlSerializerSectionAttribute : ConfigSectionAttribute
	{
		private static readonly ElementsCache<Type, XmlRootAttribute> _rootAttrs =
			new ElementsCache<Type, XmlRootAttribute>(
				type =>
				{
					var xras =
						type
							.GetCustomAttributes<XmlRootAttribute>(true)
							.ToArray();
					return xras.Length == 0 ? null : xras[0];
				});
		private static readonly ElementsCache<SchemaLocation, XmlSchema> _schemas =
			new ElementsCache<SchemaLocation, XmlSchema>(LoadSchema);

		private static XmlSchema LoadSchema(SchemaLocation loc)
		{
			var schemaRes = loc.Assembly.GetManifestResourceStream(loc.ResourceName);
			if (schemaRes == null)
				throw new ArgumentException("Resource '" + loc.ResourceName + "' not found in assembly '"
					+ loc.Assembly.FullName + "'");
			return XmlSchema.Read(schemaRes, null);
		}

		/// <summary>
		/// Инициализирует экземпляр типом, реализующим контракт.
		/// </summary>
		public XmlSerializerSectionAttribute(Type implType)
			: base(GetSectionName(implType), GetSectionNamespace(implType))
		{
			ImplementationType = implType;
		}

		/// <summary>
		/// Инициализирует экземпляр именем типа, реализующего контракт.
		/// </summary>
		public XmlSerializerSectionAttribute(string implType)
			: this(Type.GetType(implType, true))
		{}

		/// <summary>
		/// Тип, реализующий конфигурацию. Должен быть сериализуемым в XML.
		/// </summary>
		public Type ImplementationType { get; private set; }

		/// <summary>
		/// Имя ресурса со схемой.
		/// </summary>
		public string SchemaResource { get; set; }

		/// <summary>
		/// Возвращает схему конфигурации.
		/// </summary>
		protected virtual XmlSchema GetSchema(Type contractType)
		{
			return string.IsNullOrEmpty(SchemaResource)
				? null
				: _schemas.Get(new SchemaLocation(contractType.Assembly, SchemaResource));
		}

		private static string GetSectionName(Type type)
		{
			var xra = _rootAttrs.Get(type);
			return xra == null ? type.Name : xra.ElementName; 
		}

		private static string GetSectionNamespace(Type type)
		{
			var xra = _rootAttrs.Get(type);
			return xra == null || xra.Namespace == null ? "" : xra.Namespace; 
		}

		/// <summary>
		/// Создать сериалайзер.
		/// </summary>
		public override IConfigSectionSerializer CreateSerializer(Type contractType)
		{
			return new XmlSectionSerializer(ImplementationType, GetSchema(contractType));
		}

		#region SchemaLocation struct
		private class SchemaLocation
		{
			public SchemaLocation(Assembly asm, string resName)
			{
				Assembly = asm;
				ResourceName = resName;
			}

			public Assembly Assembly { get; private set; }

			public string ResourceName { get; private set; }

			public override bool Equals(object obj)
			{
				var loc = (SchemaLocation)obj;
				return Assembly == loc.Assembly && ResourceName == loc.ResourceName;
			}

			public override int GetHashCode()
			{
				return Assembly.GetHashCode() ^ ResourceName.GetHashCode();
			}
		}
		#endregion
	}
}
