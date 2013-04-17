using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Утилитный класс для работы с XML.
	/// </summary>
	public static class XmlUtils
	{
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
			return
				elements.SelectMany(element => element.Attributes.OfType<XmlAttribute>());
		}
	}
}
