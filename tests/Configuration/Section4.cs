using System.Xml.Serialization;

namespace CodeJam.Extensibility.Configuration
{
	[XmlRoot("section4")]
	public class Section4 : ISection4
	{
		#region ISection4 Members
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlText]
		public string Value { get; set; }
		#endregion
	}
}