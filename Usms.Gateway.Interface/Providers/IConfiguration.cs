using System.Collections.Generic;

namespace Elapsus.Usms.Gateway.Interface.Providers
{
	public interface IConfiguration
	{
		Dictionary<string, string> Settings { get; }
	}
}