namespace Rsdn.SmartApp.Extensibility
{
	internal class SimpleExtensionStrategy : AttachmentStrategyBase<SimpleExtensionAttribute>
	{
		private static string _lastExtensionTypeName;

		public static string LastExtensionTypeName
		{
			get { return _lastExtensionTypeName; }
		}

		/// <summary>
		/// Подключает расширение.
		/// </summary>
		protected override void Attach(ExtensionAttachmentContext context, SimpleExtensionAttribute attribute)
		{
			_lastExtensionTypeName = context.Type.AssemblyQualifiedName;
		}
	}
}