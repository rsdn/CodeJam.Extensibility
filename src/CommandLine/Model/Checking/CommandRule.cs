using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.CommandLine
{
	///<summary>
	/// Command rule.
	///</summary>
	public class CommandRule
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandRule([NotNull] string name, string description)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			Name = name;
			Description = description;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandRule(string name) : this(name, "")
		{}

		/// <summary>
		/// Command name.
		/// </summary>
		[NotNull]
		public string Name { get; }

		/// <summary>
		/// Command description.
		/// </summary>
		public string Description { get; }
	}
}