/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for requesting the reload of operations.
/// </summary>
/// <typeparam name="Operation">The type of operation associated with the reload request.</typeparam>
public class ReloadOperationsMessage : RequestMessage<Operation> { }