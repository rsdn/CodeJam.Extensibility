using System;
using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Command rule.
	///</summary>
	public class CommandRule
	{
		private readonly string _name;
		private readonly string _description;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandRule([NotNull] string name, string description)
		{
			if (name == null) throw new ArgumentNullException("name");
			_name = name;
			_description = description;
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
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Command description.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}
	}
}