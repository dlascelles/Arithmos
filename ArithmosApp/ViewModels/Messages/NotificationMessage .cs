/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Initializes a new instance of the <see cref="NotificationMessage"/> class with the specified message.
/// </summary>
/// <param name="message">The message associated with the notification.</param>
public class NotificationMessage(string message) : RequestMessage<string>
{
    /// <summary>
    /// Gets or sets the message associated with the notification.
    /// </summary>
    public string Message { get; set; } = message;
}
