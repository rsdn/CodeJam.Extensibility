using System;
using System.Collections.Generic;
using System.IO;

namespace CodeJam.Extensibility.Razor
{
	/// <summary>
	/// Base page with layout.
	/// </summary>
	public abstract class RazorGenAppResource<TLayout> : RazorGenAppResource
		where TLayout : RazorLayout, new()
	{
		private readonly TLayout _layout;
		private Dictionary<string, Action> _sections;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected RazorGenAppResource(IServiceProvider svcProvider) : base(svcProvider)
		{
			_layout = new TLayout();
		}

		/// <summary>
		/// Layout instance.
		/// </summary>
		public TLayout Layout => _layout;

		/// <summary>
		/// Write text to output.
		/// </summary>
		protected override void WriteText(AppResourceRequest request, TextWriter writer)
		{
			var body = new StringWriter();
			_sections = new Dictionary<string, Action>();
			using (WriterGuard(body))
			{
				Execute();
				body.Flush();
				_layout.WriteText(
					writer,
					body.ToString,
					name =>
					{
						Action section;
						var sectionContent = new StringWriter();
						if (_sections.TryGetValue(name, out section))
							using (WriterGuard(sectionContent))
							{
								section();
								sectionContent.Flush();
								return sectionContent.ToString();
							}
						return "";
					});
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected void DefineSection(string name, Action sectionWriter)
		{
			_sections.Add(name, sectionWriter);
		}
	}
}