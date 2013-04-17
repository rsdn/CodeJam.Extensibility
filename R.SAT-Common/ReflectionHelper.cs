using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Reflection helper methods.
	/// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// Return custom attributes, specified by T type argument.
		/// </summary>
		[NotNull]
		public static IEnumerable<T> GetCustomAttributes<T>(
			[NotNull] this ICustomAttributeProvider attrsProvider,
			bool inherit)
			where T : Attribute
		{
			if (attrsProvider == null)
				throw new ArgumentNullException("attrsProvider");

			return
				attrsProvider
					.GetCustomAttributes(typeof (T), inherit)
					.Cast<T>();
		}

		/// <summary>
		/// Return custom attribute, specified by T type argument.
		/// </summary>
		[CanBeNull]
		public static T GetCustomAttribute<T>(
			[NotNull] this ICustomAttributeProvider attrsProvider,
			bool inherit)
			where T : Attribute
		{
			if (attrsProvider == null)
				throw new ArgumentNullException("attrsProvider");

			return
				attrsProvider
					.GetCustomAttributes<T>(inherit)
					.FirstOrDefault();
		}

		/// <summary>
		/// Returns delegate parameter infos.
		/// </summary>
		[NotNull]
		public static ParameterInfo[] GetDelegateParams([NotNull] Type delegateType)
		{
			if (delegateType == null)
				throw new ArgumentNullException("delegateType");
// ReSharper disable PossibleNullReferenceException
			return delegateType.GetMethod("Invoke").GetParameters();
// ReSharper restore PossibleNullReferenceException
		}

		private static IEnumerable<Type> GetInheritancePath(Type type)
		{
			if (type == null)
				yield break;
			yield return type;
			foreach (var baseType in GetInheritancePath(type.BaseType))
				yield return baseType;
		}

		private static IEnumerable<Type> GetAllContracts(Type type)
		{
			if (type.BaseType != null)
				foreach (var baseType in GetInheritancePath(type.BaseType))
					yield return baseType;
			foreach (var iface in type.GetInterfaces())
				yield return iface;
		}

		/// <summary>
		/// Returns true, if type implements or inherits specified contract.
		/// </summary>
		public static bool IsImplemented([NotNull] this Type type, [NotNull] string assemblyQualifiedContractName)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (assemblyQualifiedContractName.IsNullOrEmpty())
				throw new ArgumentNullException("assemblyQualifiedContractName");
			return
				GetAllContracts(type).Any(
					ct => StringComparer.Ordinal.Equals(ct.AssemblyQualifiedName, assemblyQualifiedContractName));
		}
	}
}