using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace ArithmosViewModels.Messages
{
    public class NotificationMessage : RequestMessage<string>
    {
        public NotificationMessage(string message)
        {
            this.Message = message;
        }
        public string Message { get; set; }
    }
}