using System;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public sealed class ProviderNotFoundException : Exception
	{
		public ProviderNotFoundException() : base("No Provider found") {}
		public ProviderNotFoundException(string name) : base (string.Format ("The {0} provider was not found", name)) {}
	}
}
