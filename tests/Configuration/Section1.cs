using System.Xml.Serialization;

namespace CodeJam.Extensibility.Configuration
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