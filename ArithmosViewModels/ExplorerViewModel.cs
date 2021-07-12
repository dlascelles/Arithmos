/*
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
            LoadSelectedOperationsCommand = new AsyncRelayCommand(LoadSelectedOperationsAsync, CanLoadSelectedOperations);
            DeleteSelectedOperationsCommand = new RelayCommand(DeleteSelectedOperations, CanDeleteSelectedOperations);
            DeleteMarkedItemsCommand = new RelayCommand(DeleteMarkedItems, CanDeleteMarkedItems);
            SearchPhrasesCommand = new AsyncRelayCommand(SearchPhrasesAsync, CanSearchPhrases);
            LoadAllOperationsCommand = new AsyncRelayCommand(LoadAllOperationsAsync, CanLoadAllOperations);
            LoadAllOrphansCommand = new AsyncRelayCommand(LoadAllOrphansAsync, CanLoadAllOrphans);
            Phrases.CollectionChanged += Phrases_CollectionChanged;
            NumericValues.CollectionChanged += NumericValues_CollectionChanged;
            this.phraseDataService = phraseDataService;
            SettingsService = settingsService;
            if (SettingsService.SelectEnglish) { Alphabet |= Alphabet.English; }
            if (SettingsService.SelectHebrew) { Alphabet |= Alphabet.Hebrew; }
            if (SettingsService.SelectGreek) { Alphabet |= Alphabet.Greek; }
            if (SettingsService.SelectMixed) { Alphabet |= Alphabet.Mixed; }
            WeakReferenceMessenger.Default.Register<IsBusyChangedMessage>(this, (r, m) =>
            {
                DeleteMarkedItemsCommand.NotifyCanExecuteChanged();
            });
        }

        private void NumericValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SearchPhrasesCommand.NotifyCanExecuteChanged();
        }

        private void Phrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DeleteMarkedItemsCommand.NotifyCanExecuteChanged();
        }

        public AsyncRelayCommand LoadAllOperationsCommand { get; private set; }
        public async Task LoadAllOperationsAsync()
        {
            IsBusy = true;
            Operations.Clear();
            List<Operation> operations = await operationDataService.RetrieveAllAsync();
            foreach (Operation operation in operations)
            {
                Operations.Add(new OperationViewModel(operation));
            }
            IsBusy = false;
        }
        public bool CanLoadAllOperations()
        {
            return true;
        }

        public AsyncRelayCommand LoadSelectedOperationsCommand { get; private set; }
        public async Task LoadSelectedOperationsAsync()
        {
            IsBusy = true;
            Phrases.Clear();
            List<Operation> operations = new();
            foreach (OperationViewModel ovm in Operations.Where(o => o.IsSelected))
            {
                operations.Add(ovm.Operation);
            }
            foreach (Phrase phrase in await phraseDataService.RetrieveAsync(operations))
            {
                Phrases.Add(new PhraseViewModel(phrase));
            }
            IsBusy = false;
        }
        public bool CanLoadSelectedOperations()
        {
            return true;
        }

        public AsyncRelayCommand LoadAllOrphansCommand { get; private set; }
        public async Task LoadAllOrphansAsync()
        {
            IsBusy = true;
            Phrases.Clear();
            foreach (Phrase phrase in await phraseDataService.RetrieveOrphansAsync())
            {
                Phrases.Add(new PhraseViewModel(phrase));
            }
            IsBusy = false;
        }
        public bool CanLoadAllOrphans()
        {
            return true;
        }

        public RelayCommand DeleteSelectedOperationsCommand { get; private set; }
        public async void DeleteSelectedOperations()
        {
            ConfirmationMessage cm = new();
            cm.Message = "Are you sure you want to delete the selected operations?";
            await WeakReferenceMessenger.Default.Send(cm);
            if (await cm.Response == true)
            {
                try
                {
                    IsBusy = true;
                    int deletedOperations = await operationDataService.DeleteAsync(GetSelectedOperations());
                    await LoadAllOperationsAsync();
                    await RefreshItemsAsync();
                    IsBusy = false;
                    NotificationMessage deletedMessage = new($"{deletedOperations} operations have been deleted.");
                    WeakReferenceMessenger.Default.Send(deletedMessage);
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
        }
        public bool CanDeleteSelectedOperations()
        {
            return true;
        }

        public RelayCommand DeleteMarkedItemsCommand { get; private set; }
        public async void DeleteMarkedItems()
        {
            ConfirmationMessage cm = new();
            cm.Message = "Are you sure you want to delete the marked phrases?";
            await WeakReferenceMessenger.Default.Send(cm);
            if (await cm.Response == true)
            {
                try
                {
                    IsBusy = true;
                    int deletedPhrases = await phraseDataService.DeleteAsync(GetMarkedPhrases());
                    await RemoveMarkedItemsAsync();
                    IsBusy = false;
                    NotificationMessage deletedMessage = new($"{deletedPhrases} phrases have been deleted.");
                    WeakReferenceMessenger.Default.Send(deletedMessage);
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
        }
        public bool CanDeleteMarkedItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public AsyncRelayCommand SearchPhrasesCommand { get; private set; }
        public async Task SearchPhrasesAsync()
        {
            IsBusy = true;
            Phrases.Clear();
            foreach (Phrase phrase in await phraseDataService.RetrieveAsync(NumericValues.ToList(), CalculationMethod, Alphabet))
            {
                Phrases.Add(new PhraseViewModel(phrase));
            }
            IsBusy = false;
        }
        public bool CanSearchPhrases()
        {
            return CalculationMethod != CalculationMethod.None && Alphabet != Alphabet.None && NumericValues != null && NumericValues.Count > 0;
        }

        private List<Operation> GetSelectedOperations()
        {
            List<Operation> operations = new();
            foreach (OperationViewModel operationView in Operations.Where(o => o.IsSelected))
            {
                operations.Add(operationView.Operation);
            }
            return operations;
        }

        private async Task RefreshItemsAsync()
        {
            Phrases.Clear();
            if (GetSelectedOperations()?.Count > 0)
            {
                await LoadSelectedOperationsAsync();
            }
        }

        private async Task RemoveMarkedItemsAsync()
        {
            SynchronizationContext uiContext = SynchronizationContext.Current;
            await Task.Run(() =>
            {
                List<PhraseViewModel> unMarkedPhrasesVM = Phrases.Where(p => !p.IsMarked).ToList();
                uiContext.Send(x =>
                {
                    Phrases.Clear();
                    if (unMarkedPhrasesVM.Count != 0)
                    {
                        Phrases = new ObservableCollection<PhraseViewModel>(unMarkedPhrasesVM);
                    }
                }, null);
            });
        }

        private Alphabet alphabet = Alphabet.None;
        public Alphabet Alphabet
        {
            get => alphabet;
            set => SetProperty(ref alphabet, value);
        }

        private ObservableCollection<OperationViewModel> operations = new();
        public ObservableCollection<OperationViewModel> Operations
        {
            get => operations;
            set => SetProperty(ref operations, value);
        }
    }
}