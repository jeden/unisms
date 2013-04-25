using System.Collections.Generic;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers.Gateway
{
	internal class SmsProviders : Dictionary<string, ISmsProvider>, IProviders<ISmsProvider>
	{

		#region ISmsProviders Members

		public ISmsProvider Lookup(string name)
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