/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels;
using ArithmosViewModels.Messages;
using GalaSoft.MvvmLight.Messaging;
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
            Messenger.Default.Register<NotificationMessage>(this, this.IncomingNotification);
            Messenger.Default.Register<ErrorMessage>(this, this.IncomingErrorMessage);
            Messenger.Default.Register<ConfirmationMessage>(this, this.IncomingConfirmation);
        }

        private void IncomingConfirmation(ConfirmationMessage msg)
        {
            MessageBoxResult result = MessageBox.Show(msg.Notification, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                msg.Execute(true);
            }
            else if (result == MessageBoxResult.No)
            {
                msg.Execute(false);
            }
            else
            {
                msg.Execute(null);
            }
        }

        private void IncomingErrorMessage(ErrorMessage message)
        {
            MessageBox.Show(this, message.Notification, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void IncomingNotification(NotificationMessage message)
        {
            MessageBox.Show(this, message.Notification, Application.Current.MainWindow.GetType().Assembly.GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            ScannerViewModel svm = (ScannerViewModel)(this.DataContext as MainViewModel).ChildViews[1];
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            svm.FilePath = ofd.FileName;
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