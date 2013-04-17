namespace Rsdn.SmartApp
{
	/// <summary>
	/// Represents a path.
	/// </summary>
	public class Path<T>
	{
		private readonly Path<T> _parentPath;
		private readonly T _lastPathComponent;
		private readonly int _length;

		///<summary>
		/// Initialize instance.
		///</summary>
		public Path(T lastPathComponent) : this(null, lastPathComponent) { }

		///<summary>
		/// Initialize instance.
		///</summary>
		public Path(Path<T> parentPath, T lastPathComponent)
		{
			_parentPath = parentPath;
			_lastPathComponent = lastPathComponent;
			_length = ParentPath != null ? ParentPath.Length + 1 : 1;
		}

		/// <summary>
		/// Returns the last component of this path.
		/// </summary>
		public T LastPathComponent
		{
			get { return _lastPathComponent; }
		}

		/// <summary>
		/// Returns a path containing all the elements of this path, except the last path component.
		/// </summary>
		public Path<T> ParentPath
		{
			get { return _parentPath; }
		}

		/// <summary>
		/// Returns the number of elements in the path.
		/// </summary>
		public int Length
		{
			get { return _length; }
		}
	}
}