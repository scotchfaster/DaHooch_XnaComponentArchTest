using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ProjectDaHooch
{
	public delegate void MessageHandler(Message message);
	public interface IMessageService : IGameComponent
	{
		/// <summary>
		/// Adds an MessageHandler object to the invocation list of the delegate associated with the given message type.
		/// </summary>
		/// <param name="messageType">The message type to subscribe to</param>
		/// <param name="subscriber">The MessageHandler delegate that will receive the notification when the message is invoked</param>
		void SubscribeToMessageType(string messageType, MessageHandler subscriber);

		/// <summary>
		/// Remove an IMessageBaseReceiver object from the invocation list of the delegate associated with the given message type.
		/// </summary>
		/// <param name="messageType">The message type to unsubscribe from</param>
		/// <param name="subscriber">The MessageHandler delegate to unsubscribe</param>
		void UnsubscribeFromMessageType(string messageType, MessageHandler subscriber);

		/// <summary>
		/// Send a message of a particular type, specifying the object of origin, and event arguments.
		/// </summary>
		/// <param name="message">The message to send, encapsulating message type and sender reference.</param>
		/// <returns>True if there are subscribers to the message type.</returns>
		bool DispatchMessage(Message message);

		/// <summary>
		/// Send a message of a particular type, without having to instantiate a new MessageBase object.
		/// </summary>
		/// <param name="messageType">The message type to generate and send.</param>
		/// <returns>True if there are subscribers to the message type.</returns>
		bool DispatchMessage(string messageType);

		/// <summary>
		/// Queue a message to be dispatched at a later time. This method will modify the QueueTime and DispatchTime
		/// properties of the MessageBase object you pass it.
		/// </summary>
		/// <param name="message">The message to be sent, encapsulating message type and sender reference.</param>
		/// <param name="delayTime">The amount of time in milliseconds to delay the message.</param>
		void EnqueueMessage(Message message, uint delayTime);

		/// <summary>
		/// Queue a message to be dispatched at a later time by specifying a message type and allowing
		/// MessageDispatcher to generate a default MessageBase message object.
		/// </summary>
		/// <param name="messageType">The message type to be generated and sent</param>
		/// <param name="delayTime">The amount of time in milliseconds to delay the message</param>
		void EnqueueMessage(string messageType, uint delayTime);


		/// <summary>
		/// Removes the specified MessageBase object from the cue if it is there
		/// </summary>
		/// <param name="message">MessageBase object to dequeue</param>
		/// <returns>True if an object was found and dequeued</returns>
		bool DeQueueMessage(Message message);


		/// <summary>
		/// Removes the last occurrance of a MessageBase object with the specified message type if it exists
		/// </summary>
		/// <param name="messageType">The message type of the object to dequeue</param>
		/// <returns>True if a MessageBase object with the specified message type was found and removed</returns>
		bool DeQueueLastMessageType(string messageType);


		/// <summary>
		/// Checks if there are any objects subscribed to any type of message
		/// </summary>
		/// <returns>true if there are any objects subscribed to any type of message, otherwise returns false</returns>
		bool HasSubscribers();

		/// <summary>
		/// Checks if there are any objects subscribed to a particular type of message
		/// </summary>
		/// <param name="messageType">The message type for checking if there are subscribers</param>
		/// <returns>true if that message type has subscribers, otherwise returns false</returns>
		bool HasSubscribers(string messageType);

		/// <summary>
		/// Clears all subscribers of all message types from the list, removing strong references
		/// </summary>
		void ClearSubscribers();

		/// <summary>
		/// Clears all subscribers of a particular message type from the list, removing strong references
		/// </summary>
		/// <param name="messageType"></param>
		void ClearSubscribers(string messageType);

		/// <summary>
		/// Updates the MessageDispatcher, dispatching queued messages that have waited past their delay time.
		/// </summary>
		/// <param name="gameTime">GameTime object being passed around for this update tick</param>
		void Update(GameTime gameTime);
	}
}
