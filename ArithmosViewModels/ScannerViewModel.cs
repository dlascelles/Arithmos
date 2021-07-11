/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Helpers;
using ArithmosViewModels.Messages;
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
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
            this.GetFilePathCommand = new RelayCommand(this.GetFilePath, this.CanGetFilePath);
            this.phraseDataService = phraseDataService;
            this.SettingsService = settingsService;
            this.NumericValues.CollectionChanged += NumericValues_CollectionChanged;
            this.CurrentOperation.PropertyChanged += CurrentOperation_PropertyChanged;
        }

        private void CurrentOperation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Description")
            {
                this.GroupNotifyCanExecuteChanged();
            }
        }

        private void NumericValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.GroupNotifyCanExecuteChanged();
        }

        private void GroupNotifyCanExecuteChanged()
        {
            this.ScanFileCommand.NotifyCanExecuteChanged();
            this.ScanTextCommand.NotifyCanExecuteChanged();
            this.SaveMarkedItemsCommand.NotifyCanExecuteChanged();
            this.GetFilePathCommand.NotifyCanExecuteChanged();
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
                    int savedItems = await this.phraseDataService.CreateAsync(this.GetMarkedPhrases(), this.CurrentOperation.Operation, this.cts.Token);
                    message = this.cts.IsCancellationRequested ? $"Operation interrupted by user." : $"{savedItems} phrases have been successfully saved.";
                }
                this.CurrentOperation = new OperationViewModel(new Operation());
                this.IsBusy = false;
                NotificationMessage savedMessage = new NotificationMessage(message);
                WeakReferenceMessenger.Default.Send(savedMessage);
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
                NotificationMessage scannedMessage = new NotificationMessage(message);
                WeakReferenceMessenger.Default.Send(scannedMessage);
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
                NotificationMessage scannedMessage = new NotificationMessage(message);
                WeakReferenceMessenger.Default.Send(scannedMessage);
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
        public bool CanScanText()
        {
            return !String.IsNullOrEmpty(this.ImportedText) && this.CalculationMethod != CalculationMethod.None && ((this.NumericValues != null && this.NumericValues.Count() > 0) || (this.GetAllText && this.MinimumCharacters >= 0));
        }

        public RelayCommand GetFilePathCommand { get; private set; }
        public async void GetFilePath()
        {
            FileDialogMessage fdm = new FileDialogMessage();
            fdm.Message = "Select a file to scan";
            await WeakReferenceMessenger.Default.Send(fdm);
            string file = await fdm.Response;
            if (File.Exists(file))
            {
                this.FilePath = file;
            }
        }
        public bool CanGetFilePath()
        {
            return true;
        }

        private string filePath = "";
        public string FilePath
        {
            get => this.filePath;
            set
            {
                SetProperty(ref this.filePath, value);
                this.GroupNotifyCanExecuteChanged();
            }
        }

        private string importedText = "";
        public string ImportedText
        {
            get => this.importedText;
            set
            {
                SetProperty(ref this.importedText, value);
                this.GroupNotifyCanExecuteChanged();
            }
        }

        private bool getAllText = false;
        public bool GetAllText
        {
            get => this.getAllText;
            set
            {
                SetProperty(ref this.getAllText, value);
                this.GroupNotifyCanExecuteChanged();
            }
        }

        private int minimumCharacters = 3;
        public int MinimumCharacters
        {
            get => this.minimumCharacters;
            set
            {
                if (value < 0) value = 3;
                SetProperty(ref this.minimumCharacters, value);
            }
        }

        private int minimumWordsPerPhrase = 1;
        public int MinimumWordsPerPhrase
        {
            get => this.minimumWordsPerPhrase;
            set
            {
                if (value > this.MaximumWordsPerPhrase) value = this.MaximumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                SetProperty(ref this.minimumWordsPerPhrase, value);
            }
        }

        private int maximumWordsPerPhrase = 1;
        public int MaximumWordsPerPhrase
        {
            get => this.maximumWordsPerPhrase;
            set
            {
                if (value < this.MinimumWordsPerPhrase) value = this.MinimumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                SetProperty(ref this.maximumWordsPerPhrase, value);
            }
        }

        private PhraseSeparator phraseSeparator = PhraseSeparator.All;
        public PhraseSeparator PhraseSeparator
        {
            get => this.phraseSeparator;
            set
            {
                if (value == PhraseSeparator.None)
                {
                    value = PhraseSeparator.NewLine | PhraseSeparator.Colon | PhraseSeparator.Comma | PhraseSeparator.FullStop | PhraseSeparator.GreekSemicolon | PhraseSeparator.Semicolon | PhraseSeparator.Space | PhraseSeparator.Tab;
                }
                SetProperty(ref this.phraseSeparator, value);
            }
        }

        private OperationViewModel currentOperation = new OperationViewModel(new Operation());
        public OperationViewModel CurrentOperation
        {
            get => this.currentOperation;
            set
            {
                SetProperty(ref this.currentOperation, value);
            }
        }
    }
}