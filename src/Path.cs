namespace CodeJam.Extensibility
{
	/// <summary>
	/// Represents a path.
	/// </summary>
	public class Path<T>
	{
		///<summary>
		/// Initialize instance.
		///</summary>
		public Path(T lastPathComponent) : this(null, lastPathComponent) { }

		///<summary>
		/// Initialize instance.
		///</summary>
		public Path(Path<T> parentPath, T lastPathComponent)
		{
			ParentPath = parentPath;
			LastPathComponent = lastPathComponent;
			Length = ParentPath?.Length + 1 ?? 1;
		}

		/// <summary>
		/// Returns the last component of this path.
		/// </summary>
		public T LastPathComponent { get; }

		/// <summary>
		/// Returns a path containing all the elements of this path, except the last path component.
		/// </summary>
		public Path<T> ParentPath { get; }

		/// <summary>
		/// Returns the number of elements in the path.
		/// </summary>
		public int Length { get; }
	}
}