using System.Collections.Generic;

namespace Elapsus.Usms.Gateway.Interface.Providers
{
	public interface IProviders<Provider> : IDictionary<string, Provider>
	{
		Provider Lookup(string name);
	}
}