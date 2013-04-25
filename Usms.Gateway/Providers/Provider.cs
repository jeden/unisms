using System.Collections.Generic;
using Elapsus.Usms.Gateway.Interface.Providers;

namespace Elapsus.Usms.Gateway.Providers
{
	internal class Configuration : IConfiguration
	{
		private readonly Dictionary<string, string> _settings;
		
		public Configuration ()
		{
			_settings = new Dictionary<string, string>();
		}

		#region IConfiguration Members

		public Dictionary<string,string>  Settings { get { return _settings; } }

		#endregion
	}

	internal abstract class Provider : IProvider
	{
		private string _assemblyName;
		private string _className;
		private readonly Configuration _configuration;

		internal string AssemblyName { get { return _assemblyName; } set { _assemblyName = value; } }
		internal string ClassName { get { return _className; } set { _className = value; } }

		public string Name { get; set; }
		public bool Enabled { get; set; }

		protected Provider ()
		{
			_configuration = new Configuration();
		}

		#region IProvider Members

		public IConfiguration Configuration { get { return _configuration; } }

		#endregion
	}
}