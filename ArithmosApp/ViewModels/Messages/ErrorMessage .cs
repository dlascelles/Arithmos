/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for conveying an error.
/// </summary>
/// <typeparam name="string">The type of the error message.</typeparam>
public class ErrorMessage : RequestMessage<string>
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }
}
