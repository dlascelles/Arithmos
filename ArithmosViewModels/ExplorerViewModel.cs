﻿/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosViewModels
{
    public class ExplorerViewModel : CommonViewModel
    {
        private readonly IOperationDataService operationDataService = new OperationDataService();

        public ExplorerViewModel() : this(new PhraseDataService(), new SettingsService()) { }

        public ExplorerViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : base(phraseDataService, settingsService)
        {
            this.LoadSelectedOperationsCommand = new RelayCommand(async () => await this.LoadSelectedOperationsAsync(), this.CanLoadSelectedOperations);
            this.DeleteSelectedOperationsCommand = new RelayCommand(this.DeleteSelectedOperations, this.CanDeleteSelectedOperations);
            this.DeleteMarkedItemsCommand = new RelayCommand(this.DeleteMarkedItems, this.CanDeleteMarkedItems);
            this.SearchPhrasesCommand = new RelayCommand(async () => await this.SearchPhrasesAsync(), this.CanSearchPhrases);
            this.LoadAllOperationsCommand = new RelayCommand(async () => await this.LoadAllOperationsAsync(), this.CanLoadAllOperations);
            this.LoadAllOrphansCommand = new RelayCommand(async () => await this.LoadAllOrphansAsync(), this.CanLoadAllOrphans);
            this.Phrases.CollectionChanged += Phrases_CollectionChanged;
            this.phraseDataService = phraseDataService;
            this.SettingsService = settingsService;
            if (this.SettingsService.SelectEnglish) { this.Alphabet |= Alphabet.English; }
            if (this.SettingsService.SelectHebrew) { this.Alphabet |= Alphabet.Hebrew; }
            if (this.SettingsService.SelectGreek) { this.Alphabet |= Alphabet.Greek; }
            if (this.SettingsService.SelectMixed) { this.Alphabet |= Alphabet.Mixed; }
        }

        private void Phrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.DeleteMarkedItemsCommand.NotifyCanExecuteChanged();
        }

        public RelayCommand LoadAllOperationsCommand { get; private set; }
        public async Task LoadAllOperationsAsync()
        {
            this.IsBusy = true;
            this.Operations.Clear();
            List<Operation> operations = await this.operationDataService.RetrieveAllAsync();
            foreach (Operation operation in operations)
            {
                this.Operations.Add(new OperationViewModel(operation));
            }
            this.IsBusy = false;
        }
        public bool CanLoadAllOperations()
        {
            return true;
        }

        public RelayCommand LoadSelectedOperationsCommand { get; private set; }
        public async Task LoadSelectedOperationsAsync()
        {
            this.IsBusy = true;
            this.Phrases.Clear();
            List<Operation> operations = new List<Operation>();
            foreach (OperationViewModel ovm in this.Operations.Where(o => o.IsSelected))
            {
                operations.Add(ovm.Operation);
            }
            foreach (Phrase phrase in await this.phraseDataService.RetrieveAsync(operations))
            {
                this.Phrases.Add(new PhraseViewModel(phrase));
            }
            this.IsBusy = false;
        }
        public bool CanLoadSelectedOperations()
        {
            return true;
        }

        public RelayCommand LoadAllOrphansCommand { get; private set; }
        public async Task LoadAllOrphansAsync()
        {
            this.IsBusy = true;
            this.Phrases.Clear();
            foreach (Phrase phrase in await this.phraseDataService.RetrieveOrphansAsync())
            {
                this.Phrases.Add(new PhraseViewModel(phrase));
            }
            this.IsBusy = false;
        }
        public bool CanLoadAllOrphans()
        {
            return true;
        }

        public RelayCommand DeleteSelectedOperationsCommand { get; private set; }
        public async void DeleteSelectedOperations()
        {
            ConfirmationMessage cm = new ConfirmationMessage();
            cm.Message = "Are you sure you want to delete the selected operations?";
            await WeakReferenceMessenger.Default.Send(cm);
            if (await cm.Response == true)
            {
                try
                {
                    this.IsBusy = true;
                    int deletedOperations = await this.operationDataService.DeleteAsync(this.GetSelectedOperations());
                    await this.LoadAllOperationsAsync();
                    await this.RefreshItemsAsync();
                    this.IsBusy = false;
                    NotificationMessage deletedMessage = new NotificationMessage($"{deletedOperations} operations have been deleted.");
                    WeakReferenceMessenger.Default.Send(deletedMessage);
                }
                catch (Exception ex)
                {
                    ErrorMessage errorMessage = new ErrorMessage();
                    errorMessage.Message = ex.Message;
                    WeakReferenceMessenger.Default.Send(errorMessage);
                }
                finally
                {
                    this.IsBusy = false;
                }
            }
        }
        public bool CanDeleteSelectedOperations()
        {
            return true;
        }

        public RelayCommand DeleteMarkedItemsCommand { get; private set; }
        public async void DeleteMarkedItems()
        {
            ConfirmationMessage cm = new ConfirmationMessage();
            cm.Message = "Are you sure you want to delete the selected phrases?";
            await WeakReferenceMessenger.Default.Send(cm);
            if (await cm.Response == true)
            {
                try
                {
                    this.IsBusy = true;
                    int deletedPhrases = await this.phraseDataService.DeleteAsync(this.GetMarkedPhrases());
                    await this.RemoveMarkedItemsAsync();
                    this.IsBusy = false;
                    NotificationMessage deletedMessage = new NotificationMessage($"{deletedPhrases} phrases have been deleted.");
                    WeakReferenceMessenger.Default.Send(deletedMessage);
                }
                catch (Exception ex)
                {
                    ErrorMessage errorMessage = new ErrorMessage();
                    errorMessage.Message = ex.Message;
                    WeakReferenceMessenger.Default.Send(errorMessage);
                }
                finally
                {
                    this.IsBusy = false;
                }
            }
        }
        public bool CanDeleteMarkedItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand SearchPhrasesCommand { get; private set; }
        public async Task SearchPhrasesAsync()
        {
            this.IsBusy = true;
            this.Phrases.Clear();
            foreach (Phrase phrase in await this.phraseDataService.RetrieveAsync(this.NumericValues.ToList(), this.CalculationMethod, this.Alphabet))
            {
                this.Phrases.Add(new PhraseViewModel(phrase));
            }
            this.IsBusy = false;
        }
        public bool CanSearchPhrases()
        {
            return this.CalculationMethod != CalculationMethod.None && this.Alphabet != Alphabet.None && this.NumericValues != null && this.NumericValues.Count > 0;
        }

        private List<Operation> GetSelectedOperations()
        {
            List<Operation> operations = new List<Operation>();
            foreach (OperationViewModel operationView in this.Operations.Where(o => o.IsSelected))
            {
                operations.Add(operationView.Operation);
            }
            return operations;
        }

        private async Task RefreshItemsAsync()
        {
            this.Phrases.Clear();
            if (this.GetSelectedOperations()?.Count() > 0)
            {
                await this.LoadSelectedOperationsAsync();
            }
        }

        private async Task RemoveMarkedItemsAsync()
        {
            SynchronizationContext uiContext = SynchronizationContext.Current;
            await Task.Run(() =>
            {
                List<PhraseViewModel> unMarkedPhrasesVM = this.Phrases.Where(p => !p.IsMarked).ToList();
                uiContext.Send(x =>
                {
                    this.Phrases.Clear();
                    if (unMarkedPhrasesVM.Count != 0)
                    {
                        this.Phrases = new ObservableCollection<PhraseViewModel>(unMarkedPhrasesVM);
                    }
                }, null);
            });
        }

        private Alphabet alphabet = Alphabet.None;
        public Alphabet Alphabet
        {
            get => this.alphabet;
            set => SetProperty(ref this.alphabet, value);
        }

        private ObservableCollection<OperationViewModel> operations = new ObservableCollection<OperationViewModel>();
        public ObservableCollection<OperationViewModel> Operations
        {
            get => this.operations;
            set => SetProperty(ref this.operations, value);
        }
    }
}