using System.Xml;
using System.Xml.Serialization;

namespace CodeJam.Extensibility.Configuration.Serialization
{
	/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
	[XmlRoot("config", Namespace = ConfigXmlConstants.XmlNamespace)]
	public class Config
	{
		private string[] _includes = new string[0];
		private ConfigVar[] _vars = new ConfigVar[0];

		/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
		[XmlElement("include", typeof (string))]
		public string[] Includes
		{
			get { return _includes; }
			set { _includes = value ?? new string[0]; }
		}

		/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
		[XmlElement("var", typeof (ConfigVar))]
		public ConfigVar[] Vars
		{
			get { return _vars; }
			set { _vars = value ?? new ConfigVar[0]; }
		}

		/// <include file='../../CommonXmlDocs.xml' path='common-docs/not-use-directly/*'/>
		[XmlAnyElement]
		public XmlElement[] ConfigData { get; set; } = new XmlElement[0];
	}
}