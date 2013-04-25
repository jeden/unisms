using System;

namespace Elapsus.Usms.Gateway.Interface.Providers
{
	public abstract class Plugin<ProviderType> where ProviderType : IProvider
	{
		private readonly ProviderType _provider;

		protected Plugin(ProviderType provider)
		{
			_provider = provider;
		}

		protected ProviderType Provider { get { return _provider; } }

		protected static bool ContainsSetting(IConfiguration config, string key)
		{
			return (config.Settings.ContainsKey(key) && (config.Settings[key].Trim() != string.Empty));
		}

		/// <summary>
		/// Initialize the plugin
		/// </summary>
		public abstract void Initialize();
		
		/// <summary>
		/// Verify that all configuration parameters have been
		/// provided in the configuration file
		/// </summary>
		public abstract void VerifyConfiguration();

		protected static bool IsNumeric(IConfiguration config, string key)
		{
			bool ret;

			try
			{
				int.Parse(config.Settings[key]);
				ret = true;
			}
			catch (Exception)
			{
				ret = false;
			}

			return ret;
		}
	}
}