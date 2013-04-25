using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Elapsus.Usms.Gateway.Interface.Engine;
using Elapsus.Usms.Gateway.Interface.Envelope;
using Elapsus.Usms.Gateway.Interface.Providers;
using Elapsus.Usms.Gateway.Interface.Providers.Log;
using Elapsus.Usms.Gateway.Providers.Log;
using Elapsus.Usms.Logging;

namespace Elapsus.Usms.Gateway.Engine
{
	internal delegate void LogProviderAcceptanceEvent(ISms sms, DateTime timeStamp);
  internal delegate void LogProviderSubmissionEvent(ISms sms, ISmsStatusCollection status, DateTime timeStamp);
	internal delegate void LogProviderNotificationEvent(NotificationArgs args);

	internal abstract class BaseSmsc
	{
		#region Private Data Members

		private readonly LogProviders _logProviders;
		private readonly IDictionary<string, ILogHandler> _logHandlers;

		// Events
		private LogProviderAcceptanceEvent OnAcceptance;
		private LogProviderAcceptanceEvent OnEnqueuement;
		private LogProviderSubmissionEvent OnSubmission;
		private LogProviderSubmissionEvent OnSubmissionFailure;
		private LogProviderNotificationEvent OnDelivery;
		private LogProviderNotificationEvent OnDeliveryFailure;
		private LogProviderNotificationEvent OnDeliveryStatusUpdate;

		protected void NotifyAcceptance(ISms sms) { if (OnAcceptance != null) OnAcceptance(sms, DateTime.Now); }
		protected void NotifyEnqueuement(ISms sms) { if (OnEnqueuement != null) OnEnqueuement(sms, DateTime.Now); }
		protected void NotifySubmission(ISms sms, ISmsStatusCollection status) { if (OnSubmission != null) OnSubmission(sms, status, DateTime.Now); }
		protected void NotifySubmissionFailure(ISms sms, ISmsStatusCollection status) { if (OnSubmissionFailure != null) OnSubmissionFailure(sms, status, DateTime.Now); }
		protected void NotifyDelivery(NotificationArgs args) { if (OnDelivery != null) OnDelivery(args); }
		protected void NotifyDeliveryFailure(NotificationArgs args) { if (OnDeliveryFailure != null) OnDeliveryFailure(args); }
		protected void NotifyDeliveryStatusUpdate(NotificationArgs args) { if (OnDeliveryStatusUpdate != null) OnDeliveryStatusUpdate(args); }

		#endregion

		#region Constructors

		protected BaseSmsc()
		{
			_logProviders = new LogProviders();
			_logHandlers = new Dictionary<string, ILogHandler>();
			ReadLogProvidersConfiguration();
			LoadLogProviders();
		}

		#endregion

		public IProviders<ILogProvider> LogProviders { get { return _logProviders; } }

		#region Private Methods

		private void ReadLogProvidersConfiguration()
		{
			XmlReaderSettings settings;
			XmlReader config;
			LogProvider logProvider;
			string key, value;

			settings = new XmlReaderSettings();
			settings.Schemas.Add(null, Constants.LOG_PROVIDERS_CONFIG_SCHEMA);
			settings.ValidationType = ValidationType.Schema;
			config = XmlReader.Create(Constants.LOG_PROVIDERS_CONFIG, settings);

			config.ReadStartElement("usmsgw");
			config.ReadStartElement("logProviders");
			while (config.ReadToFollowing("logProvider"))
			{
				config.Read();

				logProvider = new LogProvider
				{
					Name = config.ReadElementString("name"),
					AssemblyName = config.ReadElementString("assembly"),
					ClassName = config.ReadElementString("class"),
					Enabled = IsFeatureEnabled(config.GetAttribute("enabled"))
				};

				_logProviders.Add(logProvider.Name.Trim().ToLower(), logProvider);

				///
				/// Configuration
				/// 
				config.ReadToFollowing("configuration");
				config.ReadToFollowing("set");
				do
				{
					key = config.GetAttribute("key");
					value = config.GetAttribute("value");

					if ((string.IsNullOrEmpty(key) == false) && (string.IsNullOrEmpty(value) == false))
						logProvider.Configuration.Settings[key] = value;

				} while (config.ReadToNextSibling("set"));
			}
		}

		/// <summary>
		/// Instantiate the Log Providers
		/// </summary>
		private void LoadLogProviders()
		{
			Assembly assembly;
			LogHandler logHandler;

			foreach (LogProvider provider in _logProviders.Values)
			{
				var assemblyName = new AssemblyName(provider.AssemblyName);
				assembly = Assembly.Load(assemblyName);
				logHandler = assembly.CreateInstance(provider.ClassName, false, BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, null, new object[] { provider }, null, null) as LogHandler;
				if (logHandler != null)
				{
					logHandler.VerifyConfiguration();
					logHandler.Initialize();

					OnAcceptance += logHandler.OnAcceptance;
					OnEnqueuement += logHandler.OnEnqueuement;
					OnSubmission += logHandler.OnSubmission;
					OnSubmissionFailure += logHandler.OnSubmissionFailure;
					OnDelivery += logHandler.OnDelivery;
					OnDeliveryFailure += logHandler.OnDeliveryFailure;
					OnDeliveryStatusUpdate += logHandler.OnDeliveryStatusUpdate;

					_logHandlers[provider.Name] = logHandler;
				}
			}
		}

		protected static bool IsFeatureEnabled(string value)
		{
			return (!string.IsNullOrEmpty(value)) && ((value.ToLower().Equals("true") || value.ToLower().Equals("yes")));
		}

		#endregion
	}
}
