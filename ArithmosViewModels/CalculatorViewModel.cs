/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArithmosViewModels
{
    public class CalculatorViewModel : CommonViewModel
    {
        public CalculatorViewModel() : this(new PhraseDataService(), new SettingsService()) { }
        public CalculatorViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : base(phraseDataService, settingsService)
        {
            this.SaveMarkedItemsCommand = new RelayCommand(async () => await this.SaveMarkedItemsAsync(), this.CanSaveMarkedItems);
            this.phraseDataService = phraseDataService;
            this.SettingsService = settingsService;
        }

        public RelayCommand SaveMarkedItemsCommand { get; private set; }
        public async Task SaveMarkedItemsAsync()
        {
            try
            {
                this.IsBusy = true;
                int savedItems = await this.phraseDataService.CreateAsync(this.GetMarkedPhrases(), default);
                this.IsBusy = false;
                NotificationMessage savedMessage = new NotificationMessage(this, $"{savedItems} phrases have been successfully saved.");
                Messenger.Default.Send(savedMessage);
            }
            catch (Exception ex)
            {
                ErrorMessage errorMessage = new ErrorMessage(this, ex.Message);
                Messenger.Default.Send(errorMessage);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
        public bool CanSaveMarkedItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }
    }
}