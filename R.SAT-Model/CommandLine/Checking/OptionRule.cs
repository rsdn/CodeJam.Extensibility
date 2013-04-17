using System;
using System.ComponentModel;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Option rule.
	/// </summary>
	[Localizable(false)]
	public class OptionRule
	{
		private readonly string _name;
		private readonly string _description;
		private readonly OptionType _type;
		private readonly bool _required;
		private readonly string[] _dependOnCommands;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule([NotNull] string name)
			: this(name, "")
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule([NotNull] string name, string description) : this(name, description, OptionType.Valueless)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			OptionType type)
			: this(name, "", type)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			string description,
			OptionType type)
			: this(name, description, type, false)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			OptionType type,
			bool required)
			: this(name, "", type, required)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			string description,
			OptionType type,
			bool required) : this(name, description, type, required, new string[0])
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			OptionType type,
			bool required,
			[NotNull] params string[] dependOnCommands) : this(name, "", type, required, dependOnCommands)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public OptionRule(
			[NotNull] string name,
			string description,
			OptionType type,
			bool required,
			[NotNull] params string[] dependOnCommands)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (dependOnCommands == null)
				throw new ArgumentNullException("dependOnCommands");
			if (type == OptionType.Valueless && required)
				throw new ArgumentException("Valueless property cannot be required");

			_name = name;
			_description = description;
			_type = type;
			_required = required;
			_dependOnCommands = dependOnCommands;
		}

		/// <summary>
		/// Option name.
		/// </summary>
		[NotNull]
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Option description.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Option type.
		/// </summary>
		public OptionType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Option required.
		/// </summary>
		public bool Required
		{
			get { return _required; }
		}

		/// <summary>
		/// List of command names, that option depend on.
		/// </summary>
		public string[] DependOnCommands
		{
			get { return _dependOnCommands; }
		}
	}
}