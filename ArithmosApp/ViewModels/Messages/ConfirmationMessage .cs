/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for confirming an action asynchronously.
/// </summary>
/// <typeparam name="bool?">The type of the confirmation result (nullable boolean).</typeparam>
public class ConfirmationMessage : AsyncRequestMessage<bool?>
{
    /// <summary>
    /// Gets or sets the message associated with the confirmation request.
    /// </summary>    
    public string Message { get; set; }
}
