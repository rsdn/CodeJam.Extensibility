using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Mark autoinstantiated services.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	[MeansImplicitUse]
	public class ServiceAttribute : Attribute
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		public ServiceAttribute([NotNull] params Type[] contractTypes)
		{
			if (contractTypes == null)
				throw new ArgumentNullException("contractTypes");
			ContractTypes = contractTypes;
		}

		/// <summary>
		/// Service contract types.
		/// </summary>
		public Type[] ContractTypes { get; private set; }
	}
}