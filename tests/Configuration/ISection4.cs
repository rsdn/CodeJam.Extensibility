namespace Rsdn.SmartApp.Configuration
{
	[XmlSerializerSection(typeof (Section4))]
	public interface ISection4
	{
		string Name { get; }
		string Value { get; }
	}
}