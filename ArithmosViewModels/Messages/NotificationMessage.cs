/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace ArithmosViewModels.Messages
{
    public class NotificationMessage : RequestMessage<string>
    {
        public NotificationMessage(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}