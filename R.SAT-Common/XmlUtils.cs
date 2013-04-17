using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Утилитный класс для работы с XML.
	/// </summary>
	public static class XmlUtils
	{
		#region Old XmlDOM support
		/// <summary>
		/// Перечисляет все потомки узла рекурсивно и сам узел.
		/// </summary>
		public static IEnumerable<XmlNode> DescendantNodesAndSelf(this XmlNode node)
		{
			foreach (var child in node.ChildNodes.OfType<XmlNode>())
				foreach (var subChild in child.DescendantNodesAndSelf())
					yield return subChild;
			yield return node;
		}

		/// <summary>
		/// Перечисляет все потомки элемента рекурсивно и сам узел.
		/// </summary>
		public static IEnumerable<XmlElement> DescendantElementsAndSelf(this XmlElement elem)
		{
			foreach (var child in elem.ChildNodes.OfType<XmlElement>())
				foreach (var subChild in child.DescendantElementsAndSelf())
					yield return subChild;
			yield return elem;
		}

		/// <summary>
		/// Перечисляет все атрибуты всех элементов.
		/// </summary>
		public static IEnumerable<XmlAttribute> Attributes(this IEnumerable<XmlElement> elements)
		{
			return elements.SelectMany(element => element.Attributes.OfType<XmlAttribute>());
		}
		#endregion

		/// <summary>
		/// Возвращает обязательный root документа.
		/// </summary>
		[NotNull]
		public static XElement RequiredRoot([NotNull] this XDocument document)
		{
			if (document == null)
				throw new ArgumentNullException("document");
			if (document.Root == null)
				throw new ArgumentException("Document is empty");
			return document.Root;
		}

		/// <summary>
		/// Возвращает обязательный root документа.
		/// </summary>
		[NotNull]
		public static XElement RequiredRoot([NotNull] this XDocument document, [NotNull] XName name)
		{
			if (name == null) throw new ArgumentNullException("name");
			var root = RequiredRoot(document);
			if (root.Name != name)
				throw new ArgumentException("Root '{0}' expected, but {1} found".FormatStr(name, root.Name));
			return root;
		}

		/// <summary>
		/// Возвращает обязательный элемент.
		/// </summary>
		[NotNull]
		public static XElement RequiredElement([NotNull] this XElement element, [NotNull] XName name)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (name == null)
				throw new ArgumentNullException("name");
			var res = element.Element(name);
			if (res == null)
				throw new ArgumentException("Element contains no child with specified name: '{0}'".FormatStr(name));
			return res;
		}

		/// <summary>
		/// Возвращает обязательный элемент.
		/// </summary>
		[NotNull]
		public static XElement RequiredElementAlt([NotNull] this XElement element, params XName[] names)
		{
			if (element == null) throw new ArgumentNullException("element");
			var res =
				names
					.Select(n => element.Element(n))
					.FirstOrDefault(e => e != null);
			if (res == null)
				throw new ArgumentException(
					"No one of the alternative elements specified: "
					+ names.Select(n => "'" + n.ToString() + "'").Join(", "));
			return res;
		}

		/// <summary>
		/// Возвращает обязательный атрибут.
		/// </summary>
		[NotNull]
		public static XAttribute RequiredAttribute([NotNull] this XElement element, [NotNull] XName attrName)
		{
			if (element == null)
				throw new ArgumentNullException("element");
			if (attrName == null)
				throw new ArgumentNullException("attrName");
			var attr = element.Attribute(attrName);
			if (attr == null)
				throw new ArgumentException("Element contains no attribute with specified name");
			return attr;
		}

		/// <summary>
		/// Returns optional attribute value, or default.
		/// </summary>
		public static T OptionalAttributeValue<T>(
			[NotNull] this XElement element,
			[NotNull] XName attrName,
			[NotNull] Func<string, T> valueParser,
			T defaultValue)
		{
			if (element == null) throw new ArgumentNullException("element");
			if (attrName == null) throw new ArgumentNullException("attrName");
			if (valueParser == null) throw new ArgumentNullException("valueParser");

			var attr = element.Attribute(attrName);
			return attr != null ? valueParser(attr.Value) : defaultValue;
		}

		/// <summary>
		/// Returns optional attribute value, or default.
		/// </summary>
		public static string OptionalAttributeValue(
			[NotNull] this XElement element,
			[NotNull] XName attrName,
			string defaultValue)
		{
			if (element == null) throw new ArgumentNullException("element");
			if (attrName == null) throw new ArgumentNullException("attrName");

			var attr = element.Attribute(attrName);
			return attr != null ? attr.Value : defaultValue;
		}

		/// <summary>
		/// Returns optional attribute value, or "".
		/// </summary>
		public static string OptionalAttributeValue(
			[NotNull] this XElement element,
			[NotNull] XName attrName)
		{
			return OptionalAttributeValue(element, attrName, "");
		}

		/// <summary>
		/// Возвращает опциональное значение элемента.
		/// </summary>
		public static T OptionalElementAltValue<T>(
			[NotNull] this XElement parent,
			[NotNull] Func<XElement, T> selector,
			T defaultValue,
			params XName[] names)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			if (selector == null) throw new ArgumentNullException("selector");
			if (names == null) throw new ArgumentNullException("names");

			var elem = names.Select(n => parent.Element(n)).FirstOrDefault(e => e != null);
			if (elem == null)
				return defaultValue;
			return selector(elem);
		}

		/// <summary>
		/// Возвращает опциональное значение элемента.
		/// </summary>
		public static T OptionalElementValue<T>(
			[NotNull] this XElement parent,
			[NotNull] Func<XElement, T> selector,
			T defaultValue,
			[NotNull] XName name)
		{
			if (name == null) throw new ArgumentNullException("name");
			return OptionalElementAltValue(parent, selector, defaultValue, name);
		}

		/// <summary>
		/// Возвращает элементы с именем, совпадающим с одним из элементов списка.
		/// </summary>
		public static IEnumerable<XElement> ElementsAlt([NotNull] this XElement parent, params XName[] names)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			var namesHash = new HashSet<XName>(names);
			return parent.Elements().Where(e => namesHash.Contains(e.Name));
		}

		/// <summary>
		/// Пропускает уровень, если он присутствует.
		/// </summary>
		public static XElement AltLevel(this XElement parent, XName name)
		{
			var levelElem = parent.Elements(name).FirstOrDefault();
			return levelElem ?? parent;
		}
	}
}
