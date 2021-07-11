/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace ArithmosViewModels.Messages
{
    public class ConfirmationMessage : AsyncRequestMessage<bool?>
    {
        public string Message { get; set; }
    }
}