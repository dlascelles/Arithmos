/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Helpers;
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosViewModels
{
    public class ScannerViewModel : CommonViewModel
    {
        public ScannerViewModel() : this(new PhraseDataService(), new SettingsService()) { }

        public ScannerViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : base(phraseDataService, settingsService)
        {
            this.ScanFileCommand = new RelayCommand(async () => await this.ScanFileAsync(), this.CanScanFile);
            this.ScanTextCommand = new RelayCommand(async () => await this.ScanTextAsync(), this.CanScanText);
            this.SaveMarkedItemsCommand = new RelayCommand(async () => await this.SaveMarkedItems(), this.CanSaveMarkedItems);
            this.phraseDataService = phraseDataService;
            this.SettingsService = settingsService;
        }

        public RelayCommand SaveMarkedItemsCommand { get; private set; }
        public async Task SaveMarkedItems()
        {
            try
            {
                this.IsBusy = true;
                string message = "";
                using (this.cts = new CancellationTokenSource())
                {
                    int savedItems = await this.phraseDataService.CreateAsync(this.GetMarkedPhrases(), this.CurrentOperation, this.cts.Token);
                    message = this.cts.IsCancellationRequested ? $"Operation interrupted by user." : $"{savedItems} phrases have been successfully saved.";
                }
                this.CurrentOperation = new Operation();
                this.IsBusy = false;
                NotificationMessage savedMessage = new NotificationMessage(this, message);
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
            return this.CurrentOperation != null && !String.IsNullOrEmpty(this.CurrentOperation.Description) && this.CurrentOperation.Description.Length > 0;
        }

        public RelayCommand ScanFileCommand { get; private set; }
        public async Task ScanFileAsync()
        {
            try
            {
                this.IsBusy = true;
                this.Phrases.Clear();
                List<Phrase> phrases = new List<Phrase>();
                string message = "";
                using (this.cts = new CancellationTokenSource())
                {
                    if (!this.GetAllText)
                    {
                        phrases = await Scanner.ScanFileAsync(this.FilePath, this.NumericValues.ToArray(), this.CalculationMethod, this.PhraseSeparator, this.MinimumCharacters, this.MinimumWordsPerPhrase, this.MaximumWordsPerPhrase, this.cts.Token);
                    }
                    else
                    {
                        phrases = await Scanner.ScanFileAsync(this.FilePath, this.PhraseSeparator, this.MinimumCharacters, this.MinimumWordsPerPhrase, this.MaximumWordsPerPhrase, this.cts.Token);
                    }
                    message = this.cts.IsCancellationRequested ? $"Operation interrupted by user." : $"The file was successfully scanned. {phrases.Count} unique phrases were extracted.";
                }
                foreach (Phrase phrase in phrases)
                {
                    this.Phrases.Add(new PhraseViewModel(phrase));
                }
                this.IsBusy = false;
                NotificationMessage scannedMessage = new NotificationMessage(this, message);
                Messenger.Default.Send(scannedMessage);
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
        public bool CanScanFile()
        {
            return !String.IsNullOrEmpty(this.FilePath) && this.CalculationMethod != CalculationMethod.None && ((this.NumericValues != null && this.NumericValues.Count() > 0) || (this.GetAllText && this.MinimumCharacters >= 0));
        }

        public RelayCommand ScanTextCommand { get; private set; }
        public async Task ScanTextAsync()
        {
            try
            {
                this.IsBusy = true;
                this.Phrases.Clear();
                List<Phrase> phrases = new List<Phrase>();
                string message = "";
                using (this.cts = new CancellationTokenSource())
                {
                    if (!this.GetAllText)
                    {
                        phrases = await Scanner.ScanTextAsync(this.ImportedText, this.NumericValues.ToArray(), this.CalculationMethod, this.PhraseSeparator, this.MinimumCharacters, this.MinimumWordsPerPhrase, this.MaximumWordsPerPhrase, this.cts.Token);
                    }
                    else
                    {
                        phrases = await Scanner.ScanTextAsync(this.ImportedText, this.PhraseSeparator, this.MinimumCharacters, this.MinimumWordsPerPhrase, this.MaximumWordsPerPhrase, this.cts.Token);
                    }
                    message = this.cts.IsCancellationRequested ? $"Operation interrupted by user." : $"The text was successfully scanned. {phrases.Count} unique phrases were extracted.";
                }
                foreach (Phrase phrase in phrases)
                {
                    this.Phrases.Add(new PhraseViewModel(phrase));
                }
                this.IsBusy = false;
                NotificationMessage scannedMessage = new NotificationMessage(this, message);
                Messenger.Default.Send(scannedMessage);
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
        public bool CanScanText()
        {
            return !String.IsNullOrEmpty(this.ImportedText) && this.CalculationMethod != CalculationMethod.None && ((this.NumericValues != null && this.NumericValues.Count() > 0) || (this.GetAllText && this.MinimumCharacters >= 0));
        }

        private string filePath = "";
        public string FilePath
        {
            get { return this.filePath; }
            set { this.SetField(ref this.filePath, value); }
        }

        private string importedText = "";
        public string ImportedText
        {
            get { return this.importedText; }
            set { this.SetField(ref this.importedText, value); }
        }

        private bool getAllText = false;
        public bool GetAllText
        {
            get { return this.getAllText; }
            set { this.SetField(ref this.getAllText, value); }
        }

        private int minimumCharacters = 3;
        public int MinimumCharacters
        {
            get { return this.minimumCharacters; }
            set
            {
                if (value < 0) value = 3;
                this.SetField(ref this.minimumCharacters, value);
            }
        }

        private int minimumWordsPerPhrase = 1;
        public int MinimumWordsPerPhrase
        {
            get { return this.minimumWordsPerPhrase; }
            set
            {
                if (value > this.MaximumWordsPerPhrase) value = this.MaximumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                this.SetField(ref this.minimumWordsPerPhrase, value);
            }
        }

        private int maximumWordsPerPhrase = 1;
        public int MaximumWordsPerPhrase
        {
            get { return this.maximumWordsPerPhrase; }
            set
            {
                if (value < this.MinimumWordsPerPhrase) value = this.MinimumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                this.SetField(ref this.maximumWordsPerPhrase, value);
            }
        }

        private PhraseSeparator phraseSeparator = PhraseSeparator.All;
        public PhraseSeparator PhraseSeparator
        {
            get { return this.phraseSeparator; }
            set
            {
                if (value == PhraseSeparator.None)
                {
                    value = PhraseSeparator.NewLine | PhraseSeparator.Colon | PhraseSeparator.Comma | PhraseSeparator.FullStop | PhraseSeparator.GreekSemicolon | PhraseSeparator.Semicolon | PhraseSeparator.Space | PhraseSeparator.Tab;
                }
                this.SetField(ref this.phraseSeparator, value);
            }
        }

        private Operation currentOperation = new Operation();
        public Operation CurrentOperation
        {
            get { return this.currentOperation; }
            set { this.SetField(ref this.currentOperation, value); }
        }
    }
}