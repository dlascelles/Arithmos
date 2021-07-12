using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace ArithmosViewModels.Messages
{
    public class IsBusyChangedMessage : ValueChangedMessage<bool>
    {
        public IsBusyChangedMessage(bool isBusy) : base(isBusy) { }
    }
}