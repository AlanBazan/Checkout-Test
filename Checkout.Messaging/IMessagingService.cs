using System;

namespace Checkout.Messaging
{
    public interface IMessagingService
    {
        void EnqueueMessage(string rabbitMQHostname, string exchangeName, object message);
        void ActivateMessageListener(string rabbitMQHostname, string exchangeName, Action<string> predicate);
        void DeactivateMessageListener();
    }
}