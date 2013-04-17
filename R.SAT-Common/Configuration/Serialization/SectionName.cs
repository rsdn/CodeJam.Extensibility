using System;
using System.Xml;
using System.Xml.Linq;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Имя секции.
	/// </summary>
	internal class SectionName : IEquatable<SectionName>
	{
		/// <summary>
		/// Создает экземпляр по <see cref="XmlElement"/>.
		/// </summary>
		public static SectionName Create(XElement elem)
		{
			return new SectionName(elem.Name.LocalName, elem.Name.NamespaceName);
		}

		/// <summary>
		/// Создает экземпляр по <see cref="ConfigSectionInfo"/>.
		/// </summary>
		public static SectionName Create(ConfigSectionInfo info)
		{
			return new SectionName(info.Name, info.Namespace);
		}

		private SectionName(string localName, string ns)
		{
			if (ns == null)
				throw new ArgumentNullException("ns");
			if (localName.IsNullOrEmpty())
				throw new ArgumentNullException("localName");

			LocalName = localName;
			Namespace = ns;
		}

		private string LocalName { get; set; }
		private string Namespace { get; set; }

		/// <summary>
		/// See <see cref="object.Equals(object)"/>
		/// </summary>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(obj, this))
				return true;
			var other = obj as IEquatable<SectionName>;
			return other != null && other.Equals(this);
		}

		/// <summary>
		/// See <see cref="object.GetHashCode"/>.
		/// </summary>
		public override int GetHashCode()
		{
			return LocalName.GetHashCode() + Namespace.GetHashCode();
		}

		#region IEquatable<SectionName> Members
		///<summary>
		///Indicates whether the current object is equal to another object of the same type.
		///</summary>
		public bool Equals(SectionName other)
		{
			return LocalName == other.LocalName && Namespace == other.Namespace;
		}
		#endregion
	}
}
