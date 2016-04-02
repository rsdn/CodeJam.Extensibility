using System;
using System.Collections.Generic;

namespace Rsdn.SmartApp.Configuration
{
	internal class VarsContainer
	{
		private readonly VarsContainer _parent;
		private readonly IDictionary<string, string> _varsDic;

		public VarsContainer(IDictionary<string, string> varsDic, VarsContainer parent)
		{
			_varsDic = varsDic;
			_parent = parent;
		}

		public VarsContainer(IDictionary<string, string> varsDic) : this(varsDic, null)
		{
		}

		public string GetVar(string name)
		{
			string value;
			if (_varsDic != null && _varsDic.TryGetValue(name, out value))
				return value;
			if (_parent != null)
				return _parent.GetVar(name);
			throw new ArgumentException("Variable '" + name + "' is not defined.");
		}

		public bool IsVarDefined(string name)
		{
			if (_varsDic != null && _varsDic.ContainsKey(name))
				return true;
			return _parent != null && _parent.IsVarDefined(name);
		}
	}
}