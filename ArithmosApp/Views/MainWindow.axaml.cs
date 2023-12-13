/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArithmosApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<MainWindow, NotificationMessage>(this, static async (r, m) => await r.IncomingNotification(m));
        WeakReferenceMessenger.Default.Register<MainWindow, ErrorMessage>(this, static async (r, m) => await r.IncomingErrorMessage(m));
        WeakReferenceMessenger.Default.Register<MainWindow, ConfirmationMessage>(this, static async (r, m) => { await r.IncomingConfirmation(m); });
        WeakReferenceMessenger.Default.Register<MainWindow, FileDialogMessage>(this, static (r, m) => r.IncomingFileRequest(m));
        WeakReferenceMessenger.Default.Register<MainWindow, ClipboardMessage>(this, static async (r, m) => await r.IncomingClipboardRequest(m));        
    }

    private async Task IncomingClipboardRequest(ClipboardMessage msg)
    {
        IClipboard clipboard = GetTopLevel(this)?.Clipboard;
        DataObject dataObject = new();
        dataObject.Set(DataFormats.Text, msg.Value);
        await clipboard.SetDataObjectAsync(dataObject);
    }

    private void IncomingFileRequest(FileDialogMessage msg)
    {
        IReadOnlyList<IStorageFile> file = Task.Run(() => this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions { Title = "Select text file to scan...", AllowMultiple = false })).Result;
        if (file != null && file.Count > 0)
        {
            msg.Reply(file[0].TryGetLocalPath());
        }
        else
        {
            msg.Reply(string.Empty);
        }
    }

    private async Task IncomingConfirmation(ConfirmationMessage msg)
    {
        IMsBox<string> box = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
            {
                new() { Name = "Yes", },
                new() { Name = "No", }
            },
            ContentTitle = "Confirmation",
            ContentMessage = msg.Message,
            Icon = MsBox.Avalonia.Enums.Icon.Question,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 500,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = false,
            WindowIcon = new WindowIcon(new Bitmap(AssetLoader.Open(new Uri("avares://ArithmosApp/Assets/alpha.ico"))))
        });

        string result = await box.ShowWindowDialogAsync(this);
        if (result == "Yes")
        {
            msg.Reply(true);
        }
        else if (result == "No")
        {
            msg.Reply(false);
        }
        else
        {
            msg.Reply((bool?)null);
        }
    }

    private async Task IncomingErrorMessage(ErrorMessage msg)
    {
        IMsBox<ButtonResult> box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = "Error",
            ContentMessage = msg.Message,
            Icon = MsBox.Avalonia.Enums.Icon.Error,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 500,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = false,
            WindowIcon = new WindowIcon(new Bitmap(AssetLoader.Open(new Uri("avares://ArithmosApp/Assets/alpha.ico"))))
        });
        await box.ShowWindowDialogAsync(this);
    }

    private async Task IncomingNotification(NotificationMessage msg)
    {
        IMsBox<ButtonResult> box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = "Information",
            ContentMessage = msg.Message,
            Icon = MsBox.Avalonia.Enums.Icon.Info,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            MaxWidth = 500,
            MaxHeight = 800,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInCenter = true,
            Topmost = false,
            WindowIcon = new WindowIcon(new Bitmap(AssetLoader.Open(new Uri("avares://ArithmosApp/Assets/alpha.ico"))))
        });
        await box.ShowWindowDialogAsync(this);
    }
}
