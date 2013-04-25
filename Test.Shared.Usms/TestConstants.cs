using System;

namespace Test.Shared.Usms.Gateway
{
	public static class TestConstants
	{
		public const string Default_Recipient = "+48 668 405 761";
		public const string Default_Recipient_2 = "0048 885 351 591";
		public const string Default_Recipient_3 = "+1 347 351 591";

		public const string Default_Recipient_Normalized = "48668405761";
		public const string Default_Recipient_2_Normalized = "48885351591";
    
		public const string Default_Sender = "+48885351591";

		public static readonly Guid Default_ApplicationId = new Guid("{080E593A-B6BF-4837-96F3-D1E7A5CC1687}");
	}
}
