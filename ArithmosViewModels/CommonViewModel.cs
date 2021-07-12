/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace ArithmosViewModels
{
    public class CommonViewModel : ObservableObject
    {
        public CommonViewModel()
        {
            ClearAllItemsCommand = new RelayCommand(ClearAllItems, CanClearAllItems);
            AddNumericValueCommand = new RelayCommand(AddNumericValue, CanAddNumericValue);
            RemoveNumericValueCommand = new RelayCommand<int>(RemoveNumericValue, CanRemoveNumericValue);
            ClearNumericValuesCommand = new RelayCommand(ClearNumericValues, CanClearNumericValues);
            UnmarkSelectedItemsCommand = new RelayCommand(UnmarkSelectedItems, CanUnmarkSelectedItems);
            MarkSelectedItemsCommand = new RelayCommand(MarkSelectedItems, CanMarkSelectedItems);
            UnmarkAllItemsCommand = new RelayCommand(UnmarkAllItems, CanUnmarkAllItems);
            MarkAllItemsCommand = new RelayCommand(MarkAllItems, CanMarkAllItems);
            CopyMarkedItemsCommand = new RelayCommand(CopyMarkedItems, CanCopyMarkedItems);
            CancelCommand = new RelayCommand(CancelOperation, CanCancelOperation);
            Phrases.CollectionChanged += Phrases_CollectionChanged;
            NumericValues.CollectionChanged += NumericValues_CollectionChanged;
        }

        private void NumericValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GroupNotifyCanExecuteChanged();
        }

        private void GroupNotifyCanExecuteChanged()
        {
            ClearNumericValuesCommand.NotifyCanExecuteChanged();
            ClearAllItemsCommand.NotifyCanExecuteChanged();
            UnmarkSelectedItemsCommand.NotifyCanExecuteChanged();
            MarkSelectedItemsCommand.NotifyCanExecuteChanged();
            UnmarkAllItemsCommand.NotifyCanExecuteChanged();
            MarkAllItemsCommand.NotifyCanExecuteChanged();
            CopyMarkedItemsCommand.NotifyCanExecuteChanged();
            AddNumericValueCommand.NotifyCanExecuteChanged();
            RemoveNumericValueCommand.NotifyCanExecuteChanged();
        }

        private void Phrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GroupNotifyCanExecuteChanged();
        }

        public CommonViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : this()
        {
            this.phraseDataService = phraseDataService;
            SettingsService = settingsService;
            if (SettingsService.SelectGematria && SettingsService.ShowGematria) { CalculationMethod |= CalculationMethod.Gematria; }
            if (SettingsService.SelectOrdinal && SettingsService.ShowOrdinal) { CalculationMethod |= CalculationMethod.Ordinal; }
            if (SettingsService.SelectReduced && SettingsService.ShowReduced) { CalculationMethod |= CalculationMethod.Reduced; }
            if (SettingsService.SelectSumerian && SettingsService.ShowSumerian) { CalculationMethod |= CalculationMethod.Sumerian; }
            if (SettingsService.SelectPrimes && SettingsService.ShowPrimes) { CalculationMethod |= CalculationMethod.Primes; }
            if (SettingsService.SelectSquared && SettingsService.ShowSquared) { CalculationMethod |= CalculationMethod.Squared; }
            if (SettingsService.SelectMisparGadol && SettingsService.ShowMisparGadol) { CalculationMethod |= CalculationMethod.MisparGadol; }
            if (SettingsService.SelectMisparShemi && SettingsService.ShowMisparShemi) { CalculationMethod |= CalculationMethod.MisparShemi; }
            WeakReferenceMessenger.Default.Register<CommonViewModel, SettingsUpdatedMessage>(this, static (r, m) => r.MessagesUpdated(m));
        }

        private void MessagesUpdated(SettingsUpdatedMessage obj)
        {
            SettingsService = new SettingsService();
        }

        public List<Phrase> GetSelectedPhrases()
        {
            List<Phrase> phrases = new();
            foreach (PhraseViewModel phraseView in Phrases.Where(p => p.IsSelected))
            {
                phrases.Add(phraseView.Phrase);
            }
            return phrases;
        }

        public List<Phrase> GetMarkedPhrases()
        {
            List<Phrase> phrases = new();
            foreach (PhraseViewModel phraseView in Phrases.Where(p => p.IsMarked))
            {
                phrases.Add(phraseView.Phrase);
            }
            return phrases;
        }

        public RelayCommand ClearAllItemsCommand { get; private set; }
        public void ClearAllItems()
        {
            Phrases.Clear();
        }
        public bool CanClearAllItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand<int> RemoveNumericValueCommand { get; private set; }
        public void RemoveNumericValue(int value)
        {
            if (NumericValues != null && NumericValues.Contains(value))
            {
                NumericValues.Remove(value);
            }
        }
        public bool CanRemoveNumericValue(int value)
        {
            return true;
        }

        public RelayCommand AddNumericValueCommand { get; private set; }
        public void AddNumericValue()
        {
            if (NumericValue > 0 && !NumericValues.Contains(NumericValue))
            {
                NumericValues.Add(NumericValue);
                NumericValue = 0;
            }
        }
        public bool CanAddNumericValue()
        {
            return NumericValue > 0;
        }

        public RelayCommand ClearNumericValuesCommand { get; private set; }
        public void ClearNumericValues()
        {
            NumericValues.Clear();
        }
        public bool CanClearNumericValues()
        {
            return NumericValues.Count > 0;
        }

        public RelayCommand UnmarkSelectedItemsCommand { get; private set; }
        public void UnmarkSelectedItems()
        {
            foreach (PhraseViewModel phrase in Phrases.Where(p => p.IsSelected))
            {
                phrase.IsMarked = false;
            }
        }
        public bool CanUnmarkSelectedItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand MarkSelectedItemsCommand { get; private set; }
        public void MarkSelectedItems()
        {
            foreach (PhraseViewModel phrase in Phrases.Where(p => p.IsSelected))
            {
                phrase.IsMarked = true;
            }
        }
        public bool CanMarkSelectedItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand UnmarkAllItemsCommand { get; private set; }
        public void UnmarkAllItems()
        {
            foreach (PhraseViewModel phrase in phrases)
            {
                phrase.IsMarked = false;
            }
        }
        public bool CanUnmarkAllItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand MarkAllItemsCommand { get; private set; }
        public void MarkAllItems()
        {
            foreach (PhraseViewModel phrase in phrases)
            {
                phrase.IsMarked = true;
            }
        }
        public bool CanMarkAllItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand CopyMarkedItemsCommand { get; private set; }
        public void CopyMarkedItems()
        {
            Clipboard.Clear();
            StringBuilder sb = new();
            foreach (PhraseViewModel phrase in Phrases.Where(p => p.IsMarked))
            {
                sb.AppendLine(phrase.Phrase.NormalizedText);
            }
            Clipboard.SetText(sb.ToString());
        }
        public bool CanCopyMarkedItems()
        {
            return Phrases != null && Phrases.Count > 0;
        }

        public RelayCommand CancelCommand { get; private set; }
        public void CancelOperation()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }
        public bool CanCancelOperation()
        {
            return IsBusy;
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private CalculationMethod calculationMethod = CalculationMethod.None;
        public CalculationMethod CalculationMethod
        {
            get => calculationMethod;
            set => SetProperty(ref calculationMethod, value);
        }

        protected IPhraseDataService phraseDataService = new PhraseDataService();

        protected CancellationTokenSource cts;

        private int numericValue;
        public int NumericValue
        {
            get => numericValue;
            set
            {
                SetProperty(ref numericValue, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private ISettingsService settingsService;
        public ISettingsService SettingsService
        {
            get => settingsService;
            set => SetProperty(ref settingsService, value);
        }

        private ObservableCollection<int> numericValues = new() { };
        public ObservableCollection<int> NumericValues
        {
            get => numericValues;
            set => SetProperty(ref numericValues, value);
        }

        private ObservableCollection<PhraseViewModel> phrases = new();
        public ObservableCollection<PhraseViewModel> Phrases
        {
            get => phrases;
            set => SetProperty(ref phrases, value);
        }
    }
}