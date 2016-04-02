using Rsdn.SmartApp.Configuration;

namespace CodeJam.Extensibility.Configuration
{
	[XmlSerializerSection(typeof (Section1),
		SchemaResource = "Rsdn.SmartApp.Configuration.Section1.xsd")]
	public interface ISection1
	{
		string Text { get; }
	}
}