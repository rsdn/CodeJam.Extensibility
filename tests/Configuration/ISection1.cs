namespace CodeJam.Extensibility.Configuration
{
	[XmlSerializerSection(typeof (Section1),
		SchemaResource = "CodeJam.Extensibility.Configuration.Section1.xsd")]
	public interface ISection1
	{
		string Text { get; }
	}
}