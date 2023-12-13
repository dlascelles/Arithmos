/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for interacting with a file dialog asynchronously.
/// </summary>
/// <typeparam name="string">The type of the result representing the selected file path.</typeparam>
public class FileDialogMessage : AsyncRequestMessage<string>
{
    /// <summary>
    /// Gets or sets the message associated with the file dialog request.
    /// </summary>
    public string Message { get; set; }
}
