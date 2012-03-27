

namespace ProjectDaHooch
{
	public struct Message
	{
		#region public uint QueueTime
		private uint queueTime;
		private bool queueTimeSet;
		public uint QueueTime
		{
			get { return queueTime; }
			set
			{
				if (!queueTimeSet) { queueTime = value; queueTimeSet = true; }
				else
				{
					throw new System.AccessViolationException("QueueTime has already been set. Make a new Message to change it.");
				}
			}
		}
		#endregion	
		#region public string MessageData
		private string messageData;
		private bool messageDataSet;
		public string MessageData
		{
			get { return messageData; }
			set
			{
				if (!messageDataSet) { messageData = value; }
				else
				{
					throw new System.InvalidOperationException("MessageData has already been set. Make a new Message to change it.");
				}
			}
		}
		#endregion
		#region public uint DispatchTime
		private uint dispatchTime;
		private bool dispatchTimeSet;
		public uint DispatchTime
		{
			get { return dispatchTime; }
			set
			{
				if (!dispatchTimeSet) { dispatchTime = value; dispatchTimeSet = true; }
				else
				{
					throw new System.InvalidOperationException("DispatchTime has already been set. Make a new Message to change it.");
				}
			}
		}
		#endregion
		public readonly Entity Sender;
		public readonly string MessageType;
		private readonly uint MessageID;

		private static uint messageIdCounter = 0;

		public Message(Entity sender, string messageType)
		{
			this.MessageType = messageType;
			this.queueTime = 0;
			this.dispatchTime = 0;
			this.Sender = sender;
			this.MessageID = GetNewID();
			this.messageData = string.Empty;
			queueTimeSet = false;
			messageDataSet = false;
			dispatchTimeSet = false;
		}

		//Generates unique IDs for each message
		private static uint GetNewID()
		{
			return ++messageIdCounter;
		}

		#region Operator Overloads
		public static bool operator ==(Message left, Message right)
		{
			return (left.MessageID == right.MessageID);
		}
		public static bool operator !=(Message left, Message right)
		{
			return (left.MessageID != right.MessageID);
		}
		#endregion
	}
}
