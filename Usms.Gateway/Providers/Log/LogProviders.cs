using System.Collections.Generic;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Log;

namespace Elapsus.Usms.Gateway.Providers.Log
{
	internal class LogProviders : Dictionary<string, ILogProvider>, IProviders<ILogProvider>
	{
		#region IProviders<ILogProvider> Members

		public ILogProvider Lookup(string name)
		{
			string keyName;

			keyName = name.Trim().ToLower();

			if (ContainsKey(keyName.ToLower()) == false)
				throw new ProviderNotFoundException(name);

			return this[keyName];
		}

		#endregion
	}
}
