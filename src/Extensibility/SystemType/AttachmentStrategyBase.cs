using System;
using System.Linq;
using System.Reflection;

namespace CodeJam.Extensibility.SystemType
{
	/// <summary>
	/// Базовая реализация стратегии для <see cref="Type"/>.
	/// </summary>
	public abstract class AttachmentStrategyBase : IExtensionAttachmentStrategy
	{
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
			SupportedAttrNames = supportedAttrNames;
		}

		/// <summary>
		/// Поддерживаемые типы.
		/// </summary>
		public string[] SupportedAttrNames { get; }

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
			return SupportedAttrNames.Any(name => name == attrName);
		}

		void IExtensionAttachmentStrategy.Attach(ExtensionAttachmentContext context, CustomAttributeData attribute)
		{
			if (IsSuitable(attribute))
				Attach(context, attribute);
		}
	}
}
