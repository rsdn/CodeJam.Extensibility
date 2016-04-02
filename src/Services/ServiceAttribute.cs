using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
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
				throw new ArgumentNullException(nameof(contractTypes));
			ContractTypes = contractTypes;
		}

		/// <summary>
		/// Service contract types.
		/// </summary>
		public Type[] ContractTypes { get; private set; }
	}
}