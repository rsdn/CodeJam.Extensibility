using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Helper methods for <see cref="Path{T}"/>.
	/// </summary>
	public static class PathHelper
	{
		/// <summary>
		/// Returns a new path containing all the elements of <paramref name="path"/> plus
		/// <paramref name="component"/>.
		/// </summary>
		public static Path<T> Add<T>(this Path<T> path, T component)
		{
			return new Path<T>(path, component);
		}

		/// <summary>
		/// Returns a new path containing all the elements of <see cref="Path{T}.ParentPath"/> plus
		/// <paramref name="component"/>.
		/// </summary>
		public static Path<T> ReplaceLastPathComponent<T>(this Path<T> path, T component)
		{
			return new Path<T>(path != null ? path.ParentPath : null, component);
		}

		/// <summary>
		/// Returns the path at the specified index.
		/// </summary>
		public static Path<T> GetPathAt<T>([NotNull] this Path<T> path, int index)
		{
			if (path == null)
				throw new ArgumentNullException("path");
			if (index < 0 || index >= path.Length)
				throw new ArgumentOutOfRangeException("index");

			var currentPath = path;
			while (currentPath.Length - 1 > index)
				currentPath = currentPath.ParentPath;
			return currentPath;
		}

		/// <summary>
		/// Returns an ordered array containing the components of <paramref name="path"/>.
		/// </summary>
		public static T[] GetPath<T>(this Path<T> path)
		{
			if (path == null)
				return EmptyArray<T>.Value;

			var i = path.Length;
			var result = new T[i--];
			for (var curPath = path; curPath != null; curPath = path.ParentPath)
				result[i--] = curPath.LastPathComponent;
			return result;
		}
	}
}