/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ArithmosApp.ViewModels.Messages;

/// <summary>
/// Represents a message for requesting the regeneration of datagrid columns based on a Gematria method.
/// </summary>
/// <typeparam name="GematriaMethod">The type of Gematria method associated with the regeneration request.</typeparam>
public class RegenerateColumnsMessage : RequestMessage<GematriaMethod> { }
