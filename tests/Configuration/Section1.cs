using System.Xml.Serialization;

namespace Rsdn.SmartApp.Configuration
{
	[XmlRoot("section1", Namespace = "Section1.xsd")]
	public class Section1 : ISection1
	{
		#region ISection1 Members
		[XmlText]
		public string Text { get; set; }
		#endregion
	}
}