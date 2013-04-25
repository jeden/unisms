using System;
using System.Net;
using Elapsus.Usms.Gateway.Interface.Providers.Gateway;

namespace Elapsus.Usms.Gateway.Providers
{
	public enum EnRequestMethod
	{
		POST,
		GET
	} ;

	public abstract class HttpSmsGateway : SmsGateway
	{
		private HttpWebRequest _httpRequest;
		//private HttpWebResponse _httpResponse;
		private Uri _currentHost;
		private readonly EnRequestMethod _requestMethod;

		protected abstract Uri SelectRemoteHost();

		protected HttpSmsGateway(ISmsProvider smsProvider, EnRequestMethod requestMethod) : base(smsProvider)
		{
			KeepConnectionOpen = false;
			_requestMethod = requestMethod;
		}

		public override void Connect()
		{
			if (IsConnected == false)
			{
				_currentHost = SelectRemoteHost();
				_httpRequest = (HttpWebRequest) WebRequest.Create(_currentHost.OriginalString);
				_httpRequest.Method = _requestMethod.ToString();
			}
		}
	}
}