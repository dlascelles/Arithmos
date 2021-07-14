/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;

namespace ArithmosViewModels
{
    public class CalculatorViewModel : CommonViewModel
    {
        public CalculatorViewModel() : this(new PhraseDataService(), new SettingsService()) { }
        public CalculatorViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : base(phraseDataService, settingsService)
        {
            SaveMarkedItemsCommand = new RelayCommand(async () => await SaveMarkedItemsAsync(), CanSaveMarkedItems);
            this.phraseDataService = phraseDataService;
            SettingsService = settingsService;
            Phrases.CollectionChanged += Phrases_CollectionChanged;
        }

        private void Phrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SaveMarkedItemsCommand.NotifyCanExecuteChanged();
        }

        public RelayCommand SaveMarkedItemsCommand { get; private set; }
        public async Task SaveMarkedItemsAsync()
        {
            try
            {
                IsBusy = true;
                int savedItems = await phraseDataService.CreateAsync(GetMarkedPhrases(), default);
                IsBusy = false;
                NotificationMessage savedMessage = new($"{savedItems} phrases have been successfully saved.");
                WeakReferenceMessenger.Default.Send(savedMessage);
            }
            catch (Exception ex)
            {
                ErrorMessage errorMessage = new();
                errorMessage.Message = ex.Message;
                WeakReferenceMessenger.Default.Send(errorMessage);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool CanSaveMarkedItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }
    }
}