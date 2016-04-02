using System.Xml.Serialization;

namespace Rsdn.SmartApp.Configuration
{
	/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
	public class ConfigVar
	{
		/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
		[XmlText]
		public string Value { get; set; }
	}
}