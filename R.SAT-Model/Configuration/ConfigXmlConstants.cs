namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Константы, связанные с XML-представлением конфигурации.
	/// </summary>
	public static class ConfigXmlConstants
	{
		/// <summary>
		/// XML-неймспейс.
		/// </summary>
		public const string XmlNamespace =
			"http://rsdn.ru/R.SAT/ConfigSectionSchema.xsd";

		/// <summary>
		/// Имя ресурса со схемой.
		/// </summary>
		public const string XmlSchemaResource =
			"Rsdn.SmartApp.Configuration.ConfigSectionSchema.xsd";

		/// <summary>
		/// Имя тега с инклюдом.
		/// </summary>
		public const string IncludeTagName = "include";

		/// <summary>
		/// Имя тега с переменной.
		/// </summary>
		public const string VariableTagName = "var";

		/// <summary>
		/// Имя атрибута с именем переменной.
		/// </summary>
		public const string VariableNameAttribute = "name";
	}
}