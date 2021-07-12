using ArithmosViewModels.Messages;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Diagnostics;

namespace ArithmosViewModels
{
    public class AboutViewModel : ObservableObject
    {
        public AboutViewModel()
        {
            NavigateToProjectUrlCommand = new RelayCommand(NavigateToProjectUrl, CanNavigateToProjectUrl);

        }
        public RelayCommand NavigateToProjectUrlCommand { get; private set; }
        public void NavigateToProjectUrl()
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://github.com/dlascelles/Arithmos") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                ErrorMessage errorMessage = new();
                errorMessage.Message = ex.Message;
                WeakReferenceMessenger.Default.Send(errorMessage);
            }
        }
        public bool CanNavigateToProjectUrl()
        {
            return true;
        }
    }
}