using System.Xml.Serialization;

namespace CodeJam.Extensibility.Configuration
{
	[XmlRoot("section3")]
	public class Section3 : ISection3
	{
		#region ISection3 Members
		[XmlElement("value", typeof (string))]
		public string[] Values { get; set; }
		#endregion
	}
}