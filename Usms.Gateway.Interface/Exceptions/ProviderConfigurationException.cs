using System;

namespace Elapsus.Usms.Gateway.Interface.Exceptions
{
	public enum EnConfigurationReason
	{
		MissingParameter,
		ExpectedNumeric,
		MissingDefaultProvider,
		MultipleDefaultProviders
	}

	public enum EnProviderType
	{
		QueueProvider
	}

	public sealed class ProviderConfigurationException : Exception
	{
		public EnConfigurationReason Reason { get; private set; }

		private ProviderConfigurationException (EnConfigurationReason reason, string message) : base(message)
		{
			Reason = reason;
		}

		public static ProviderConfigurationException MissingParameter(string providerName, string parameterName)
		{
			return new ProviderConfigurationException(EnConfigurationReason.MissingParameter, string.Format("{0} Sms Provider: missing {1} configuration parameter", providerName, parameterName));
		}

		public static ProviderConfigurationException ExpectedNumeric(string providerName, string parameterName)
		{
			return new ProviderConfigurationException(EnConfigurationReason.ExpectedNumeric, string.Format("{0} Sms Provider: parameter {1} is expected to be numeric", providerName, parameterName));
		}

		public static ProviderConfigurationException MissingDefaultProvider(EnProviderType providerType)
		{
			return new ProviderConfigurationException(EnConfigurationReason.MissingDefaultProvider, string.Format("{0}: No default provider specified", providerType));
		}

		public static ProviderConfigurationException MultipleDefaultProviders(EnProviderType providerType)
		{
			return new ProviderConfigurationException(EnConfigurationReason.MultipleDefaultProviders, string.Format("{0}: 2 or more default providers specified, but only 1 is allowed", providerType));
		}
	}
}
