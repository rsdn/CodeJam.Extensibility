using System.Xml.Serialization;

namespace Rsdn.SmartApp.Configuration
{
	[XmlRoot("section2")]
	public class Section2 : ISection2
	{
		#region ISection2 Members
		[XmlElement("number")]
		public int Number { get; set; }
		#endregion
	}
}