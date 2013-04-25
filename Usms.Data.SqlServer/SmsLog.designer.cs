﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Elapsus.Usms.Data.SqlServer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="usms")]
	public partial class SmsLogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertSmsLog(SmsLog instance);
    partial void UpdateSmsLog(SmsLog instance);
    partial void DeleteSmsLog(SmsLog instance);
    partial void InsertSmsLogRecipient(SmsLogRecipient instance);
    partial void UpdateSmsLogRecipient(SmsLogRecipient instance);
    partial void DeleteSmsLogRecipient(SmsLogRecipient instance);
    #endregion
		
		public SmsLogDataContext() : 
				base(global::Elapsus.Usms.Data.SqlServer.Properties.Settings.Default.usmsConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public SmsLogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsLogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsLogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SmsLogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<SmsLog> SmsLogs
		{
			get
			{
				return this.GetTable<SmsLog>();
			}
		}
		
		public System.Data.Linq.Table<SmsLogRecipient> SmsLogRecipients
		{
			get
			{
				return this.GetTable<SmsLogRecipient>();
			}
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsLog_Forwarded")]
		public int SmsLog_Forwarded([global::System.Data.Linq.Mapping.ParameterAttribute(Name="SmsLogId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> smsLogId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProviderReference", DbType="VarChar(50)")] string providerReference, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ForwardedOn", DbType="DateTime")] System.Nullable<System.DateTime> forwardedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipient", DbType="VarChar(15)")] string recipient, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Failed", DbType="Bit")] System.Nullable<bool> failed, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorCode", DbType="Int")] System.Nullable<int> errorCode, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorMessage", DbType="NText")] string errorMessage)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), smsLogId, providerReference, forwardedOn, recipient, failed, errorCode, errorMessage);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsQueue_Enqueue")]
		public ISingleResult<SmsQueue_EnqueueResult> SmsQueue_Enqueue([global::System.Data.Linq.Mapping.ParameterAttribute(Name="SmsLogId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> smsLogId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ApplicationId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> applicationId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sender", DbType="VarChar(15)")] string sender, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TotalRecipients", DbType="Int")] System.Nullable<int> totalRecipients, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Message", DbType="NText")] string message, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="AcceptedOn", DbType="DateTime")] System.Nullable<System.DateTime> acceptedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipients", DbType="Text")] string recipients)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), smsLogId, applicationId, sender, totalRecipients, message, acceptedOn, recipients);
			return ((ISingleResult<SmsQueue_EnqueueResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsLog_Insert")]
		public ISingleResult<SmsLog_InsertResult> SmsLog_Insert([global::System.Data.Linq.Mapping.ParameterAttribute(Name="SmsLogId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> smsLogId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ApplicationId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> applicationId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Sender", DbType="VarChar(15)")] string sender, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="TotalRecipients", DbType="Int")] System.Nullable<int> totalRecipients, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Message", DbType="NText")] string message, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="AcceptedOn", DbType="DateTime")] System.Nullable<System.DateTime> acceptedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipients", DbType="Text")] string recipients, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Enqueued", DbType="Bit")] System.Nullable<bool> enqueued)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), smsLogId, applicationId, sender, totalRecipients, message, acceptedOn, recipients, enqueued);
			return ((ISingleResult<SmsLog_InsertResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsQueue_Dequeue")]
		public ISingleResult<SmsLog> SmsQueue_Dequeue([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ConversationHandle", DbType="UniqueIdentifier")] ref System.Nullable<System.Guid> conversationHandle)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), conversationHandle);
			conversationHandle = ((System.Nullable<System.Guid>)(result.GetParameterValue(0)));
			return ((ISingleResult<SmsLog>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsQueue_Dequeue_Commit")]
		public int SmsQueue_Dequeue_Commit([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ConversationHandle", DbType="UniqueIdentifier")] ref System.Nullable<System.Guid> conversationHandle, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Commit", DbType="Bit")] System.Nullable<bool> commit)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), conversationHandle, commit);
			conversationHandle = ((System.Nullable<System.Guid>)(result.GetParameterValue(0)));
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsQueue_GetQueueSize", IsComposable=true)]
		public System.Nullable<int> SmsQueue_GetQueueSize()
		{
			return ((System.Nullable<int>)(this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod()))).ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsLog_DeliveryStatusUpdate")]
		public int SmsLog_DeliveryStatusUpdate([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProviderReference", DbType="VarChar(50)")] string providerReference, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipient", DbType="VarChar(15)")] string recipient, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="NotifiedOn", DbType="DateTime")] System.Nullable<System.DateTime> notifiedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="NotificationCode", DbType="Int")] System.Nullable<int> notificationCode, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="NotificationMessage", DbType="NText")] string notificationMessage)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), providerReference, recipient, notifiedOn, notificationCode, notificationMessage);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsLog_DeliveryFailureNotification")]
		public int SmsLog_DeliveryFailureNotification([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProviderReference", DbType="VarChar(50)")] string providerReference, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipient", DbType="VarChar(15)")] string recipient, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="FailedOn", DbType="DateTime")] System.Nullable<System.DateTime> failedOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorCode", DbType="Int")] System.Nullable<int> errorCode, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ErrorMessage", DbType="NText")] string errorMessage)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), providerReference, recipient, failedOn, errorCode, errorMessage);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.SmsLog_DeliveryConfirmation")]
		public int SmsLog_DeliveryConfirmation([global::System.Data.Linq.Mapping.ParameterAttribute(Name="ProviderReference", DbType="VarChar(50)")] string providerReference, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Recipient", DbType="VarChar(15)")] string recipient, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DeliveredOn", DbType="DateTime")] System.Nullable<System.DateTime> deliveredOn, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="NotificationCode", DbType="Int")] System.Nullable<int> notificationCode)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), providerReference, recipient, deliveredOn, notificationCode);
			return ((int)(result.ReturnValue));
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SmsLog")]
	public partial class SmsLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _SmsLogId;
		
		private System.Guid _ApplicationId;
		
		private string _Sender;
		
		private int _TotalRecipients;
		
		private string _Message;
		
		private System.Nullable<System.DateTime> _AcceptedOn;
		
		private System.Nullable<System.DateTime> _ForwardedOn;
		
		private int _Failures;
		
		private int _Successes;
		
		private System.Nullable<int> _ErrorCode;
		
		private string _ErrorMessage;
		
		private EntitySet<SmsLogRecipient> _SmsLogRecipients;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSmsLogIdChanging(System.Guid value);
    partial void OnSmsLogIdChanged();
    partial void OnApplicationIdChanging(System.Guid value);
    partial void OnApplicationIdChanged();
    partial void OnSenderChanging(string value);
    partial void OnSenderChanged();
    partial void OnTotalRecipientsChanging(int value);
    partial void OnTotalRecipientsChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnAcceptedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnAcceptedOnChanged();
    partial void OnForwardedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnForwardedOnChanged();
    partial void OnFailuresChanging(int value);
    partial void OnFailuresChanged();
    partial void OnSuccessesChanging(int value);
    partial void OnSuccessesChanged();
    partial void OnErrorCodeChanging(System.Nullable<int> value);
    partial void OnErrorCodeChanged();
    partial void OnErrorMessageChanging(string value);
    partial void OnErrorMessageChanged();
    #endregion
		
		public SmsLog()
		{
			this._SmsLogRecipients = new EntitySet<SmsLogRecipient>(new Action<SmsLogRecipient>(this.attach_SmsLogRecipients), new Action<SmsLogRecipient>(this.detach_SmsLogRecipients));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmsLogId", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid SmsLogId
		{
			get
			{
				return this._SmsLogId;
			}
			set
			{
				if ((this._SmsLogId != value))
				{
					this.OnSmsLogIdChanging(value);
					this.SendPropertyChanging();
					this._SmsLogId = value;
					this.SendPropertyChanged("SmsLogId");
					this.OnSmsLogIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApplicationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ApplicationId
		{
			get
			{
				return this._ApplicationId;
			}
			set
			{
				if ((this._ApplicationId != value))
				{
					this.OnApplicationIdChanging(value);
					this.SendPropertyChanging();
					this._ApplicationId = value;
					this.SendPropertyChanged("ApplicationId");
					this.OnApplicationIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sender", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string Sender
		{
			get
			{
				return this._Sender;
			}
			set
			{
				if ((this._Sender != value))
				{
					this.OnSenderChanging(value);
					this.SendPropertyChanging();
					this._Sender = value;
					this.SendPropertyChanged("Sender");
					this.OnSenderChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalRecipients", DbType="Int NOT NULL")]
		public int TotalRecipients
		{
			get
			{
				return this._TotalRecipients;
			}
			set
			{
				if ((this._TotalRecipients != value))
				{
					this.OnTotalRecipientsChanging(value);
					this.SendPropertyChanging();
					this._TotalRecipients = value;
					this.SendPropertyChanged("TotalRecipients");
					this.OnTotalRecipientsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AcceptedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> AcceptedOn
		{
			get
			{
				return this._AcceptedOn;
			}
			set
			{
				if ((this._AcceptedOn != value))
				{
					this.OnAcceptedOnChanging(value);
					this.SendPropertyChanging();
					this._AcceptedOn = value;
					this.SendPropertyChanged("AcceptedOn");
					this.OnAcceptedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ForwardedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> ForwardedOn
		{
			get
			{
				return this._ForwardedOn;
			}
			set
			{
				if ((this._ForwardedOn != value))
				{
					this.OnForwardedOnChanging(value);
					this.SendPropertyChanging();
					this._ForwardedOn = value;
					this.SendPropertyChanged("ForwardedOn");
					this.OnForwardedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Failures", DbType="Int NOT NULL")]
		public int Failures
		{
			get
			{
				return this._Failures;
			}
			set
			{
				if ((this._Failures != value))
				{
					this.OnFailuresChanging(value);
					this.SendPropertyChanging();
					this._Failures = value;
					this.SendPropertyChanged("Failures");
					this.OnFailuresChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Successes", DbType="Int NOT NULL")]
		public int Successes
		{
			get
			{
				return this._Successes;
			}
			set
			{
				if ((this._Successes != value))
				{
					this.OnSuccessesChanging(value);
					this.SendPropertyChanging();
					this._Successes = value;
					this.SendPropertyChanged("Successes");
					this.OnSuccessesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorCode", DbType="Int")]
		public System.Nullable<int> ErrorCode
		{
			get
			{
				return this._ErrorCode;
			}
			set
			{
				if ((this._ErrorCode != value))
				{
					this.OnErrorCodeChanging(value);
					this.SendPropertyChanging();
					this._ErrorCode = value;
					this.SendPropertyChanged("ErrorCode");
					this.OnErrorCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorMessage", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string ErrorMessage
		{
			get
			{
				return this._ErrorMessage;
			}
			set
			{
				if ((this._ErrorMessage != value))
				{
					this.OnErrorMessageChanging(value);
					this.SendPropertyChanging();
					this._ErrorMessage = value;
					this.SendPropertyChanged("ErrorMessage");
					this.OnErrorMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SmsLog_SmsLogRecipient", Storage="_SmsLogRecipients", ThisKey="SmsLogId", OtherKey="SmsLogId")]
		public EntitySet<SmsLogRecipient> SmsLogRecipients
		{
			get
			{
				return this._SmsLogRecipients;
			}
			set
			{
				this._SmsLogRecipients.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_SmsLogRecipients(SmsLogRecipient entity)
		{
			this.SendPropertyChanging();
			entity.SmsLog = this;
		}
		
		private void detach_SmsLogRecipients(SmsLogRecipient entity)
		{
			this.SendPropertyChanging();
			entity.SmsLog = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.SmsLogRecipient")]
	public partial class SmsLogRecipient : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _SmsLogId;
		
		private string _Recipient;
		
		private string _ProviderReference;
		
		private short _Status;
		
		private System.Nullable<System.DateTime> _NotifiedOn;
		
		private System.Nullable<int> _NotificationCode;
		
		private string _NotificationMessage;
		
		private EntityRef<SmsLog> _SmsLog;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSmsLogIdChanging(System.Guid value);
    partial void OnSmsLogIdChanged();
    partial void OnRecipientChanging(string value);
    partial void OnRecipientChanged();
    partial void OnProviderReferenceChanging(string value);
    partial void OnProviderReferenceChanged();
    partial void OnStatusChanging(short value);
    partial void OnStatusChanged();
    partial void OnNotifiedOnChanging(System.Nullable<System.DateTime> value);
    partial void OnNotifiedOnChanged();
    partial void OnNotificationCodeChanging(System.Nullable<int> value);
    partial void OnNotificationCodeChanged();
    partial void OnNotificationMessageChanging(string value);
    partial void OnNotificationMessageChanged();
    #endregion
		
		public SmsLogRecipient()
		{
			this._SmsLog = default(EntityRef<SmsLog>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmsLogId", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid SmsLogId
		{
			get
			{
				return this._SmsLogId;
			}
			set
			{
				if ((this._SmsLogId != value))
				{
					if (this._SmsLog.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnSmsLogIdChanging(value);
					this.SendPropertyChanging();
					this._SmsLogId = value;
					this.SendPropertyChanged("SmsLogId");
					this.OnSmsLogIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Recipient", DbType="VarChar(15) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Recipient
		{
			get
			{
				return this._Recipient;
			}
			set
			{
				if ((this._Recipient != value))
				{
					this.OnRecipientChanging(value);
					this.SendPropertyChanging();
					this._Recipient = value;
					this.SendPropertyChanged("Recipient");
					this.OnRecipientChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProviderReference", DbType="VarChar(50)")]
		public string ProviderReference
		{
			get
			{
				return this._ProviderReference;
			}
			set
			{
				if ((this._ProviderReference != value))
				{
					this.OnProviderReferenceChanging(value);
					this.SendPropertyChanging();
					this._ProviderReference = value;
					this.SendPropertyChanged("ProviderReference");
					this.OnProviderReferenceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="SmallInt NOT NULL")]
		public short Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotifiedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> NotifiedOn
		{
			get
			{
				return this._NotifiedOn;
			}
			set
			{
				if ((this._NotifiedOn != value))
				{
					this.OnNotifiedOnChanging(value);
					this.SendPropertyChanging();
					this._NotifiedOn = value;
					this.SendPropertyChanged("NotifiedOn");
					this.OnNotifiedOnChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotificationCode", DbType="Int")]
		public System.Nullable<int> NotificationCode
		{
			get
			{
				return this._NotificationCode;
			}
			set
			{
				if ((this._NotificationCode != value))
				{
					this.OnNotificationCodeChanging(value);
					this.SendPropertyChanging();
					this._NotificationCode = value;
					this.SendPropertyChanged("NotificationCode");
					this.OnNotificationCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotificationMessage", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string NotificationMessage
		{
			get
			{
				return this._NotificationMessage;
			}
			set
			{
				if ((this._NotificationMessage != value))
				{
					this.OnNotificationMessageChanging(value);
					this.SendPropertyChanging();
					this._NotificationMessage = value;
					this.SendPropertyChanged("NotificationMessage");
					this.OnNotificationMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="SmsLog_SmsLogRecipient", Storage="_SmsLog", ThisKey="SmsLogId", OtherKey="SmsLogId", IsForeignKey=true)]
		public SmsLog SmsLog
		{
			get
			{
				return this._SmsLog.Entity;
			}
			set
			{
				SmsLog previousValue = this._SmsLog.Entity;
				if (((previousValue != value) 
							|| (this._SmsLog.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._SmsLog.Entity = null;
						previousValue.SmsLogRecipients.Remove(this);
					}
					this._SmsLog.Entity = value;
					if ((value != null))
					{
						value.SmsLogRecipients.Add(this);
						this._SmsLogId = value.SmsLogId;
					}
					else
					{
						this._SmsLogId = default(System.Guid);
					}
					this.SendPropertyChanged("SmsLog");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	public partial class SmsQueue_EnqueueResult
	{
		
		private System.Guid _SmsLogId;
		
		private System.Guid _ApplicationId;
		
		private string _Sender;
		
		private int _TotalRecipients;
		
		private string _Message;
		
		private System.Nullable<System.DateTime> _AcceptedOn;
		
		private System.Nullable<System.DateTime> _ForwardedOn;
		
		private int _Failures;
		
		private int _Successes;
		
		private System.Nullable<int> _ErrorCode;
		
		private string _ErrorMessage;
		
		public SmsQueue_EnqueueResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmsLogId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid SmsLogId
		{
			get
			{
				return this._SmsLogId;
			}
			set
			{
				if ((this._SmsLogId != value))
				{
					this._SmsLogId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApplicationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ApplicationId
		{
			get
			{
				return this._ApplicationId;
			}
			set
			{
				if ((this._ApplicationId != value))
				{
					this._ApplicationId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sender", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string Sender
		{
			get
			{
				return this._Sender;
			}
			set
			{
				if ((this._Sender != value))
				{
					this._Sender = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalRecipients", DbType="Int NOT NULL")]
		public int TotalRecipients
		{
			get
			{
				return this._TotalRecipients;
			}
			set
			{
				if ((this._TotalRecipients != value))
				{
					this._TotalRecipients = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this._Message = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AcceptedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> AcceptedOn
		{
			get
			{
				return this._AcceptedOn;
			}
			set
			{
				if ((this._AcceptedOn != value))
				{
					this._AcceptedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ForwardedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> ForwardedOn
		{
			get
			{
				return this._ForwardedOn;
			}
			set
			{
				if ((this._ForwardedOn != value))
				{
					this._ForwardedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Failures", DbType="Int NOT NULL")]
		public int Failures
		{
			get
			{
				return this._Failures;
			}
			set
			{
				if ((this._Failures != value))
				{
					this._Failures = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Successes", DbType="Int NOT NULL")]
		public int Successes
		{
			get
			{
				return this._Successes;
			}
			set
			{
				if ((this._Successes != value))
				{
					this._Successes = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorCode", DbType="Int")]
		public System.Nullable<int> ErrorCode
		{
			get
			{
				return this._ErrorCode;
			}
			set
			{
				if ((this._ErrorCode != value))
				{
					this._ErrorCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorMessage", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string ErrorMessage
		{
			get
			{
				return this._ErrorMessage;
			}
			set
			{
				if ((this._ErrorMessage != value))
				{
					this._ErrorMessage = value;
				}
			}
		}
	}
	
	public partial class SmsLog_InsertResult
	{
		
		private System.Guid _SmsLogId;
		
		private System.Guid _ApplicationId;
		
		private string _Sender;
		
		private int _TotalRecipients;
		
		private string _Message;
		
		private System.Nullable<System.DateTime> _AcceptedOn;
		
		private System.Nullable<System.DateTime> _ForwardedOn;
		
		private int _Failures;
		
		private int _Successes;
		
		private System.Nullable<int> _ErrorCode;
		
		private string _ErrorMessage;
		
		public SmsLog_InsertResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SmsLogId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid SmsLogId
		{
			get
			{
				return this._SmsLogId;
			}
			set
			{
				if ((this._SmsLogId != value))
				{
					this._SmsLogId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApplicationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ApplicationId
		{
			get
			{
				return this._ApplicationId;
			}
			set
			{
				if ((this._ApplicationId != value))
				{
					this._ApplicationId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sender", DbType="VarChar(15) NOT NULL", CanBeNull=false)]
		public string Sender
		{
			get
			{
				return this._Sender;
			}
			set
			{
				if ((this._Sender != value))
				{
					this._Sender = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalRecipients", DbType="Int NOT NULL")]
		public int TotalRecipients
		{
			get
			{
				return this._TotalRecipients;
			}
			set
			{
				if ((this._TotalRecipients != value))
				{
					this._TotalRecipients = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Message", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this._Message = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AcceptedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> AcceptedOn
		{
			get
			{
				return this._AcceptedOn;
			}
			set
			{
				if ((this._AcceptedOn != value))
				{
					this._AcceptedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ForwardedOn", DbType="DateTime")]
		public System.Nullable<System.DateTime> ForwardedOn
		{
			get
			{
				return this._ForwardedOn;
			}
			set
			{
				if ((this._ForwardedOn != value))
				{
					this._ForwardedOn = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Failures", DbType="Int NOT NULL")]
		public int Failures
		{
			get
			{
				return this._Failures;
			}
			set
			{
				if ((this._Failures != value))
				{
					this._Failures = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Successes", DbType="Int NOT NULL")]
		public int Successes
		{
			get
			{
				return this._Successes;
			}
			set
			{
				if ((this._Successes != value))
				{
					this._Successes = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorCode", DbType="Int")]
		public System.Nullable<int> ErrorCode
		{
			get
			{
				return this._ErrorCode;
			}
			set
			{
				if ((this._ErrorCode != value))
				{
					this._ErrorCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorMessage", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string ErrorMessage
		{
			get
			{
				return this._ErrorMessage;
			}
			set
			{
				if ((this._ErrorMessage != value))
				{
					this._ErrorMessage = value;
				}
			}
		}
	}
}
#pragma warning restore 1591