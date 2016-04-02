namespace CodeJam.Extensibility.Configuration
{
	[XmlSerializerSection("CodeJam.Extensibility.Configuration.Section2, CodeJam.Extensibility-Tests")]
	public interface ISection2
	{
		int Number { get; }
	}
}