CREATE MESSAGE
	TYPE [//flexysms.com/Sms/Message/Mt]
	VALIDATION = NONE;

CREATE CONTRACT
	[//flexysms.com/Sms/Contract/Mt]
	([//flexysms.com/Sms/Message/Mt] SENT BY INITIATOR)

CREATE QUEUE SmsMtQueue
	WITH STATUS = ON,
	RETENTION = OFF

CREATE QUEUE SmsMtTargetQueue
	WITH STATUS = ON,
	RETENTION = OFF

CREATE SERVICE [//flexysms.com/Sms/Service/Mt]
	ON QUEUE SmsMtQueue
	([//flexysms.com/Sms/Contract/Mt])
	
CREATE SERVICE [//flexysms.com/Sms/Service/MtTarget]
	ON QUEUE SmsMtTargetQueue
	([//flexysms.com/Sms/Contract/Mt])
