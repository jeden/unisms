using System.Collections.Generic;
using Elapsus.Usms.Gateway.Interface.Exceptions;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Queue;

namespace Elapsus.Usms.Gateway.Providers.Queue
{
	internal class QueueProviders : Dictionary<string, IQueueProvider>, IProviders<IQueueProvider>
	{
		#region IProviders<IQueueProvider> Members

		public IQueueProvider Lookup(string name)
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
