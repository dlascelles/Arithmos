/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels;
using ArithmosViewModels.Messages;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Win32;
using System.Linq;
using System.Windows;

namespace Arithmos
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register<MainWindow, NotificationMessage>(this, static (r, m) => r.IncomingNotification(m));
            WeakReferenceMessenger.Default.Register<MainWindow, ErrorMessage>(this, static (r, m) => r.IncomingErrorMessage(m));
            WeakReferenceMessenger.Default.Register<MainWindow, ConfirmationMessage>(this, static (r, m) => r.IncomingConfirmation(m));
            WeakReferenceMessenger.Default.Register<MainWindow, FileDialogMessage>(this, static (r, m) => r.IncomingFileRequest(m));
        }

        private void IncomingFileRequest(FileDialogMessage msg)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = msg.Message;
            ofd.ShowDialog();
            msg.Reply(ofd.FileName);
        }

        private void IncomingConfirmation(ConfirmationMessage msg)
        {
            MessageBoxResult result = MessageBox.Show(msg.Message, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                msg.Reply(true);
            }
            else if (result == MessageBoxResult.No)
            {
                msg.Reply(false);
            }
            else
            {
                msg.Reply((bool?)null);
            }
        }

        private void IncomingErrorMessage(ErrorMessage msg)
        {
            MessageBox.Show(this, msg.Message, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void IncomingNotification(NotificationMessage msg)
        {
            MessageBox.Show(this, msg.Message, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //We need this here in order for datagrid row virtualization, otherwise our "IsSelected" bindings don't work.
        private void PhraseGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.RemovedItems != null && e.RemovedItems.Count > 0)
            {
                foreach (var removedItem in e.RemovedItems.OfType<PhraseViewModel>())
                {
                    removedItem.IsSelected = false;
                }
            }
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                foreach (var addedItem in e.AddedItems.OfType<PhraseViewModel>())
                {
                    addedItem.IsSelected = true;
                }
            }
        }
    }
}