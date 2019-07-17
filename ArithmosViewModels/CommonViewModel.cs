/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ArithmosViewModels
{
    public class CommonViewModel : ModelBase
    {
        protected IPhraseDataService phraseDataService = new PhraseDataService();
        public CommonViewModel()
        {
            this.ClearAllItemsCommand = new RelayCommand(this.ClearAllItems, this.CanClearAllItems);
            this.AddNumericValueCommand = new RelayCommand(this.AddNumericValue, this.CanAddNumericValue);
            this.RemoveNumericValueCommand = new RelayCommand<int>(this.RemoveNumericValue, this.CanRemoveNumericValue());
            this.ClearNumericValuesCommand = new RelayCommand(this.ClearNumericValues, this.CanClearNumericValues);
            this.UnmarkSelectedItemsCommand = new RelayCommand(this.UnmarkSelectedItems, this.CanUnmarkSelectedItems);
            this.MarkSelectedItemsCommand = new RelayCommand(this.MarkSelectedItems, this.CanMarkSelectedItems);
            this.UnmarkAllItemsCommand = new RelayCommand(this.UnmarkAllItems, this.CanUnmarkAllItems);
            this.MarkAllItemsCommand = new RelayCommand(this.MarkAllItems, this.CanMarkAllItems);
            this.CopyMarkedItemsCommand = new RelayCommand(this.CopyMarkedItems, this.CanCopyMarkedItems);
        }

        public CommonViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : this()
        {
            this.phraseDataService = phraseDataService;
            this.SettingsService = settingsService;
            if (this.SettingsService.SelectGematria && this.SettingsService.ShowGematria) { this.CalculationMethod |= CalculationMethod.Gematria; }
            if (this.SettingsService.SelectOrdinal && this.SettingsService.ShowOrdinal) { this.CalculationMethod |= CalculationMethod.Ordinal; }
            if (this.SettingsService.SelectReduced && this.SettingsService.ShowReduced) { this.CalculationMethod |= CalculationMethod.Reduced; }
            if (this.SettingsService.SelectSumerian && this.SettingsService.ShowSumerian) { this.CalculationMethod |= CalculationMethod.Sumerian; }
            if (this.SettingsService.SelectPrimes && this.SettingsService.ShowPrimes) { this.CalculationMethod |= CalculationMethod.Primes; }
            if (this.SettingsService.SelectSquared && this.SettingsService.ShowSquared) { this.CalculationMethod |= CalculationMethod.Squared; }
            if (this.SettingsService.SelectMisparGadol && this.SettingsService.ShowMisparGadol) { this.CalculationMethod |= CalculationMethod.MisparGadol; }
            if (this.SettingsService.SelectMisparShemi && this.SettingsService.ShowMisparShemi) { this.CalculationMethod |= CalculationMethod.MisparShemi; }
            Messenger.Default.Register<SettingsUpdatedMessage>(this, this.MessagesUpdated);
        }

        private void MessagesUpdated(SettingsUpdatedMessage obj)
        {
            this.SettingsService = new SettingsService();
        }

        public List<Phrase> GetSelectedPhrases()
        {
            List<Phrase> phrases = new List<Phrase>();
            foreach (PhraseViewModel phraseView in this.Phrases.Where(p => p.IsSelected))
            {
                phrases.Add(phraseView.Phrase);
            }
            return phrases;
        }

        public List<Phrase> GetMarkedPhrases()
        {
            List<Phrase> phrases = new List<Phrase>();
            foreach (PhraseViewModel phraseView in this.Phrases.Where(p => p.IsMarked))
            {
                phrases.Add(phraseView.Phrase);
            }
            return phrases;
        }

        public RelayCommand ClearAllItemsCommand { get; private set; }
        public void ClearAllItems()
        {
            this.Phrases.Clear();
        }
        public bool CanClearAllItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand<int> RemoveNumericValueCommand { get; private set; }
        public void RemoveNumericValue(int value)
        {
            if (this.NumericValues != null && this.NumericValues.Contains(value))
            {
                this.NumericValues.Remove(value);
            }
        }
        public bool CanRemoveNumericValue()
        {
            return true;
        }

        public RelayCommand AddNumericValueCommand { get; private set; }
        public void AddNumericValue()
        {
            if (this.NumericValue > 0 && !this.NumericValues.Contains(this.NumericValue))
            {
                this.NumericValues.Add(this.NumericValue);
                this.NumericValue = 0;
            }
        }
        public bool CanAddNumericValue()
        {
            return this.NumericValue > 0;
        }

        public RelayCommand ClearNumericValuesCommand { get; private set; }
        public void ClearNumericValues()
        {
            this.NumericValues.Clear();
        }
        public bool CanClearNumericValues()
        {
            return this.NumericValues.Count() > 0;
        }

        public RelayCommand UnmarkSelectedItemsCommand { get; private set; }
        public void UnmarkSelectedItems()
        {
            foreach (PhraseViewModel phrase in this.Phrases.Where(p => p.IsSelected))
            {
                phrase.IsMarked = false;
            }
        }
        public bool CanUnmarkSelectedItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand MarkSelectedItemsCommand { get; private set; }
        public void MarkSelectedItems()
        {
            foreach (PhraseViewModel phrase in this.Phrases.Where(p => p.IsSelected))
            {
                phrase.IsMarked = true;
            }
        }
        public bool CanMarkSelectedItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand UnmarkAllItemsCommand { get; private set; }
        public void UnmarkAllItems()
        {
            foreach (PhraseViewModel phrase in this.phrases)
            {
                phrase.IsMarked = false;
            }
        }
        public bool CanUnmarkAllItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand MarkAllItemsCommand { get; private set; }
        public void MarkAllItems()
        {
            foreach (PhraseViewModel phrase in this.phrases)
            {
                phrase.IsMarked = true;
            }
        }
        public bool CanMarkAllItems()
        {
            return this.Phrases != null && this.Phrases.Count() > 0;
        }

        public RelayCommand CopyMarkedItemsCommand { get; private set; }
        public void CopyMarkedItems()
        {
            Clipboard.Clear();
            StringBuilder sb = new StringBuilder();
            foreach (PhraseViewModel phrase in this.Phrases.Where(p => p.IsMarked))
            {
                sb.AppendLine(phrase.Phrase.NormalizedText);
            }
            Clipboard.SetText(sb.ToString());
        }
        public bool CanCopyMarkedItems()
        {
            return this.Phrases != null && this.Phrases.Count > 0;
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetField(ref this.isBusy, value); }
        }

        private CalculationMethod calculationMethod = CalculationMethod.None;
        public CalculationMethod CalculationMethod
        {
            get { return this.calculationMethod; }
            set { this.SetField(ref this.calculationMethod, value); }
        }

        private int numericValue;
        public int NumericValue
        {
            get { return this.numericValue; }
            set { this.SetField(ref this.numericValue, value); }
        }

        private ISettingsService settingsService;
        public ISettingsService SettingsService
        {
            get { return this.settingsService; }
            set { this.SetField(ref this.settingsService, value); }
        }

        private ObservableCollection<int> numericValues = new ObservableCollection<int> { };
        public ObservableCollection<int> NumericValues
        {
            get { return this.numericValues; }
            set { this.SetField(ref this.numericValues, value); }
        }

        private ObservableCollection<PhraseViewModel> phrases = new ObservableCollection<PhraseViewModel>();
        public ObservableCollection<PhraseViewModel> Phrases
        {
            get { return this.phrases; }
            set { this.SetField(ref this.phrases, value); }
        }
    }
}