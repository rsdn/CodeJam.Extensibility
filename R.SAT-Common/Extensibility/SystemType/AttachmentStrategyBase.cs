using System;
using System.Linq;
using System.Reflection;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация стратегии для <see cref="Type"/>.
	/// </summary>
	public abstract class AttachmentStrategyBase : IExtensionAttachmentStrategy
	{
		private readonly string[] _supportedAttrNames;

		/// <summary>
		/// Initialize instance by attribute types.
		/// </summary>
		protected AttachmentStrategyBase(params Type[] supportedAttrTypes)
			: this(supportedAttrTypes.Select(t => t.AssemblyQualifiedName).ToArray())
		{}

		/// <summary>
		/// Initialize instance by attribute type names.
		/// </summary>
		protected AttachmentStrategyBase(params string[] supportedAttrNames)
		{
			_supportedAttrNames = supportedAttrNames;
		}

		/// <summary>
		/// Поддерживаемые типы.
		/// </summary>
		public string[] SupportedAttrNames
		{
			get { return _supportedAttrNames; }
		}

		/// <summary>
		/// Attach specified type.
		/// </summary>
		protected abstract void Attach(ExtensionAttachmentContext context, CustomAttributeData attribute);

		/// <summary>
		/// Возвращает true, если атрибут соответствует стратегии.
		/// </summary>
		protected virtual bool IsSuitable(CustomAttributeData attributeData)
		{
			if (attributeData.Constructor.DeclaringType == null)
				return false;
			var attrName = attributeData.Constructor.DeclaringType.AssemblyQualifiedName;
			return _supportedAttrNames.Any(name => name == attrName);
		}

		void IExtensionAttachmentStrategy.Attach(ExtensionAttachmentContext context, CustomAttributeData attribute)
		{
			if (IsSuitable(attribute))
				Attach(context, attribute);
		}
	}
}
