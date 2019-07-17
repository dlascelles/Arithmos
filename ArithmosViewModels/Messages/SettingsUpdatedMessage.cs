/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using GalaSoft.MvvmLight.Messaging;

namespace ArithmosViewModels.Messages
{
    public class SettingsUpdatedMessage : NotificationMessage
    {
        public SettingsUpdatedMessage(object sender, string message) : base(sender, message) { }
    }
}
