using System.Diagnostics;

using CodeJam.Extensibility.Registration;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Стратегия регистрации active parts.
	/// </summary>
	public class ActivePartRegStrategy : RegElementsStrategy<ActivePartInfo, ActivePartAttribute>
	{
		/// <include file='../CommonXmlDocs.xml' path='common-docs/def-ctor/*'/>
		public ActivePartRegStrategy(IServicePublisher publisher) : base(publisher)
		{}

		/// <summary>
		/// Создать описатель.
		/// </summary>
		public override ActivePartInfo CreateElement(ExtensionAttachmentContext context, ActivePartAttribute attr)
		{
			//if (!typeof(IActivePart).IsAssignableFrom(context.Type))
			//	throw new ExtensibilityException(
			//		$"Type \'{context.Type}\' must implement interface \'{typeof(IActivePart)}");
			Debug.Assert(context.Type.AssemblyQualifiedName != null, "context.Type.AssemblyQualifiedName != null");
			return new ActivePartInfo(context.Type.AssemblyQualifiedName);
		}
	}
}