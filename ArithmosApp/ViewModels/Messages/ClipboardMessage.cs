/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for clipboard-related operations.
/// </summary>
/// <typeparam name="string">The type of the value associated with the clipboard message.</typeparam>
public class ClipboardMessage : RequestMessage<string>
{
    /// <summary>
    /// Gets or sets the value associated with the clipboard message.
    /// </summary>    
    public string Value { get; set; }
}
