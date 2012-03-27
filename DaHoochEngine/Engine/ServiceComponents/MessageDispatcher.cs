using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ProjectDaHooch
{
	/// <summary>
	/// This class is a singleton used for broadcasting messages between objects in the game.
	/// It supports instantaneous and time-delayed messages. with DispatchMessage and QueueMessage respectfully.
	/// </summary>
	public class MessageDispatcher : GameComponent, IMessageService
	{
		//private static MessageDispatcher _instance;

		///// <summary>
		///// The global singleton instance of MessageDispatcher
		///// </summary>
		//public static MessageDispatcher GlobalInstance
		//{
		//    get
		//    {
		//        if (_instance == null)
		//            _instance = new MessageDispatcher();
		//        return _instance;
		//    }
		//}

		private Dictionary<string, MessageHandler> MessageTypeList = new Dictionary<string, MessageHandler>(1, StringComparer.OrdinalIgnoreCase);
		private MessageHandler tempMessageHandler;
		private List<Message> messageQueue;
		private uint currentTime;

		public MessageDispatcher(Game game)
			: base(game)
		{
			messageQueue = new List<Message>(5);
			game.Components.Add(this);
			game.Services.AddService(typeof(IMessageService), this);
		}

		public MessageDispatcher(Game game, int queueSize)
			: base(game)
		{
			messageQueue = new List<Message>(queueSize);
			game.Components.Add(this);
			game.Services.AddService(typeof(IMessageService), this);
		}

		/// <summary>
		/// Adds an MessageBaseReceiver object to the invocation list of the delegate associated with the given message type.
		/// </summary>
		/// <param name="messageType">The message type to subscribe to</param>
		/// <param name="subscriber">The MessageHandler delegate that will receive the notification when the message is invoked</param>
		public void SubscribeToMessageType(string messageType, MessageHandler subscriber)
		{
			if (MessageTypeList.TryGetValue(messageType, out tempMessageHandler))
			{
				tempMessageHandler += new MessageHandler(subscriber);
				MessageTypeList.Remove(messageType);
				MessageTypeList.Add(messageType, tempMessageHandler);
				Debug.WriteLine("Added a subscriber!");
			}
			else
			{
				Debug.WriteLine("Ruh roh, event handler is NULL, I'll fix this.");
				tempMessageHandler += new MessageHandler(subscriber);
				MessageTypeList.Add(messageType, tempMessageHandler);
				Debug.WriteLine("Now that we've initialized the delegate, we've added the subscriber.");
			}
		}

		/// <summary>
		/// Remove an IMessageBaseReceiver object from the invocation list of the delegate associated with the given message type.
		/// </summary>
		/// <param name="messageType">The message type to unsubscribe from</param>
		/// <param name="subscriber">The MessageHandler delegate to unsubscribe</param>
		public void UnsubscribeFromMessageType(string messageType, MessageHandler subscriber)
		{
			if (MessageTypeList.TryGetValue(messageType, out tempMessageHandler))
			{
				tempMessageHandler -= subscriber;
			}
			else
			{
				Debug.WriteLine("Object (Hash#" + subscriber.GetHashCode() + ") tried to unsubscribe from " +
					"message type '" + messageType + "'. No such message type.");
			}
		}

		/// <summary>
		/// Send a message of a particular type, specifying the object of origin, and event arguments.
		/// </summary>
		/// <param name="message">The message to send, encapsulating message type and sender reference.</param>
		/// <returns>True if there are subscribers to the message type.</returns>
		public bool DispatchMessage(Message message)
		{
			MessageTypeList.TryGetValue(message.MessageType, out tempMessageHandler);
			if (tempMessageHandler != null)
			{
				tempMessageHandler(message);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Send a message of a particular type, without having to instantiate a new MessageBase object.
		/// </summary>
		/// <param name="messageType">The message type to generate and send.</param>
		/// <returns>True if there are subscribers to the message type.</returns>
		public bool DispatchMessage(string messageType)
		{
			MessageTypeList.TryGetValue(messageType, out tempMessageHandler);
			if (tempMessageHandler != null)
			{
				Message tempMessage = new Message(null, messageType);
				tempMessageHandler(tempMessage);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Queue a message to be dispatched at a later time. This method will modify the QueueTime and DispatchTime
		/// properties of the MessageBase object you pass it.
		/// </summary>
		/// <param name="message">The message to be sent, encapsulating message type and sender reference.</param>
		/// <param name="delayTime">The amount of time in milliseconds to delay the message.</param>
		public void EnqueueMessage(Message message, uint delayTime)
		{
			if (this.HasSubscribers(message.MessageType))
			{
				message.QueueTime = this.currentTime;
				message.DispatchTime = message.QueueTime + delayTime;
				messageQueue.Add(message);
			}
		}

		/// <summary>
		/// Queue a message to be dispatched at a later time by specifying a message type and allowing
		/// MessageDispatcher to generate a default MessageBase message object.
		/// </summary>
		/// <param name="messageType">The message type to be generated and sent</param>
		/// <param name="delayTime">The amount of time in milliseconds to delay the message</param>
		public void EnqueueMessage(string messageType, uint delayTime)
		{
			EnqueueMessage(new Message(null, messageType), delayTime);
		}


		/// <summary>
		/// Removes the specified MessageBase object from the cue if it is there
		/// </summary>
		/// <param name="message">MessageBase object to dequeue</param>
		/// <returns>True if an object was found and dequeued</returns>
		public bool DeQueueMessage(Message message)
		{
			if (messageQueue.Count > 0)
			{
				for (int i = 0; i < messageQueue.Count; i++)
				{
					if (messageQueue[i] == message)
					{
						messageQueue.RemoveAt(i);
						return true;
					}
				}
			}
			return false;
		}


		/// <summary>
		/// Removes the last occurrance of a MessageBase object with the specified message type if it exists
		/// </summary>
		/// <param name="messageType">The message type of the object to dequeue</param>
		/// <returns>True if a MessageBase object with the specified message type was found and removed</returns>
		public bool DeQueueLastMessageType(string messageType)
		{
			if (messageQueue.Count > 0)
			{
				for (int i = 0; i < messageQueue.Count; i++)
				{
					if (messageQueue[i].MessageType.Equals(messageType))
					{
						messageQueue.RemoveAt(i);
						return true;
					}
				}
			}
			return false;
		}


		/// <summary>
		/// Checks if there are any objects subscribed to any type of message
		/// </summary>
		/// <returns>true if there are any objects subscribed to any type of message, otherwise returns false</returns>
		public bool HasSubscribers()
		{
			bool hasSubscribers = MessageTypeList.Count > 0 ? true : false;
			return hasSubscribers;
		}
		
		/// <summary>
		/// Checks if there are any objects subscribed to a particular type of message
		/// </summary>
		/// <param name="messageType">The message type for checking if there are subscribers</param>
		/// <returns>true if that message type has subscribers, otherwise returns false</returns>
		public bool HasSubscribers(string messageType)
		{
			MessageTypeList.TryGetValue(messageType, out tempMessageHandler);
			bool hasSubscribers = tempMessageHandler != null ? true : false;
			return hasSubscribers;
		}

		/// <summary>
		/// Clears all subscribers of all message types from the list, removing strong references
		/// </summary>
		public void ClearSubscribers()
		{
			MessageTypeList.Clear();
		}

		/// <summary>
		/// Clears all subscribers of a particular message type from the list, removing strong references
		/// </summary>
		/// <param name="messageType"></param>
		public void ClearSubscribers(string messageType)
		{
			MessageTypeList.Remove(messageType);
		}

		/// <summary>
		/// Updates the MessageDispatcher, dispatching queued messages that have waited past their delay time.
		/// </summary>
		/// <param name="gameTime">GameTime object being passed around for this update tick</param>
		public override void Update(GameTime gameTime)
		{
			this.currentTime += (uint)gameTime.ElapsedGameTime.TotalMilliseconds;
			if (messageQueue.Count > 0)
			{
				int count = messageQueue.Count;
				for (int i = 0; i < count; i++)
				{
					if (currentTime >= messageQueue[i].DispatchTime)
					{
						Message msg = messageQueue[i];
						msg.DispatchTime = currentTime;
						DispatchMessage(msg);
						messageQueue.RemoveAt(i);
						i--;
						count--;
					}

				}
			}
		}
	}
}
