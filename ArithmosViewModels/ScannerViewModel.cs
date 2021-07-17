/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
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
using System.Windows.Forms;

namespace ArithmosViewModels
{
    public class ScannerViewModel : CommonViewModel
    {
        public ScannerViewModel() : this(new PhraseDataService(), new SettingsService()) { }

        public ScannerViewModel(IPhraseDataService phraseDataService, ISettingsService settingsService) : base(phraseDataService, settingsService)
        {
            ScanFileCommand = new AsyncRelayCommand(ScanFileAsync, CanScanFile);
            ScanTextCommand = new AsyncRelayCommand(ScanTextAsync, CanScanText);
            SaveMarkedItemsCommand = new AsyncRelayCommand(SaveMarkedItems, CanSaveMarkedItems);
            GetFilePathCommand = new RelayCommand(GetFilePath, CanGetFilePath);
            GetFolderPathCommand = new RelayCommand(GetFolderPath, CanGetFolderPath);
            this.phraseDataService = phraseDataService;
            SettingsService = settingsService;
            NumericValues.CollectionChanged += NumericValues_CollectionChanged;
            CurrentOperation.PropertyChanged += CurrentOperation_PropertyChanged;
        }

        private void CurrentOperation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Description")
            {
                GroupNotifyCanExecuteChanged();
            }
        }

        private void NumericValues_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GroupNotifyCanExecuteChanged();
        }

        private void GroupNotifyCanExecuteChanged()
        {
            ScanFileCommand.NotifyCanExecuteChanged();
            ScanTextCommand.NotifyCanExecuteChanged();
            SaveMarkedItemsCommand.NotifyCanExecuteChanged();
            GetFilePathCommand.NotifyCanExecuteChanged();
        }

        public AsyncRelayCommand SaveMarkedItemsCommand { get; private set; }
        public async Task SaveMarkedItems()
        {
            try
            {
                IsBusy = true;
                string message = "";
                using (cts = new CancellationTokenSource())
                {
                    int savedItems = await phraseDataService.CreateAsync(GetMarkedPhrases(), CurrentOperation.Operation, cts.Token);
                    message = cts.IsCancellationRequested ? $"Operation interrupted by user." : $"{savedItems} phrases have been successfully saved.";
                }
                CurrentOperation = new OperationViewModel(new Operation());
                IsBusy = false;
                NotificationMessage savedMessage = new(message);
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
            return CurrentOperation != null && !String.IsNullOrEmpty(CurrentOperation.Description) && CurrentOperation.Description.Length > 0;
        }

        public AsyncRelayCommand ScanFileCommand { get; private set; }
        public async Task ScanFileAsync()
        {
            try
            {
                IsBusy = true;
                Phrases.Clear();
                List<Phrase> phrases = new();
                string message = "";

                using (cts = new CancellationTokenSource())
                {
                    if (!GetAllText)
                    {
                        phrases = await Scanner.ScanFileAsync(FilePath, NumericValues.ToHashSet(), CalculationMethod, PhraseSeparator, MinimumCharacters, MinimumWordsPerPhrase, MaximumWordsPerPhrase, cts.Token);
                    }
                    else
                    {
                        phrases = await Scanner.ScanFileAsync(FilePath, PhraseSeparator, MinimumCharacters, MinimumWordsPerPhrase, MaximumWordsPerPhrase, cts.Token);
                    }

                    if (cts.IsCancellationRequested == false)
                    {
                        if (GridOutput)
                        {
                            foreach (Phrase phrase in phrases)
                            {
                                Phrases.Add(new PhraseViewModel(phrase));
                            }
                        }

                        if (FileOutput && !string.IsNullOrWhiteSpace(ExportFolderPath))
                        {
                            await Exporter.ExportAsync(Exporter.GetPhrasesForExport(phrases, ','), ExportFolderPath, cts.Token, "Arithmos_Export_", "csv");
                        }
                    }

                    message = cts.IsCancellationRequested ? $"Operation interrupted by user." : $"The file was successfully scanned. {phrases.Count} unique phrases were extracted.";
                }

                IsBusy = false;
                NotificationMessage scannedMessage = new(message);
                WeakReferenceMessenger.Default.Send(scannedMessage);
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
        public bool CanScanFile()
        {
            return !String.IsNullOrEmpty(FilePath) && CalculationMethod != CalculationMethod.None && ((NumericValues != null && NumericValues.Count > 0) || (GetAllText && MinimumCharacters >= 0));
        }

        public AsyncRelayCommand ScanTextCommand { get; private set; }
        public async Task ScanTextAsync()
        {
            try
            {
                IsBusy = true;
                Phrases.Clear();
                List<Phrase> phrases = new();
                string message = "";
                using (cts = new CancellationTokenSource())
                {
                    if (!GetAllText)
                    {
                        phrases = await Scanner.ScanTextAsync(ImportedText, NumericValues.ToHashSet<int>(), CalculationMethod, PhraseSeparator, MinimumCharacters, MinimumWordsPerPhrase, MaximumWordsPerPhrase, cts.Token);
                    }
                    else
                    {
                        phrases = await Scanner.ScanTextAsync(ImportedText, PhraseSeparator, MinimumCharacters, MinimumWordsPerPhrase, MaximumWordsPerPhrase, cts.Token);
                    }

                    if (cts.IsCancellationRequested == false)
                    {
                        if (GridOutput)
                        {
                            foreach (Phrase phrase in phrases)
                            {
                                Phrases.Add(new PhraseViewModel(phrase));
                            }
                        }

                        if (FileOutput && !string.IsNullOrWhiteSpace(ExportFolderPath))
                        {
                            await Exporter.ExportAsync(Exporter.GetPhrasesForExport(phrases, ','), ExportFolderPath, cts.Token, "Arithmos_Export_", "csv");
                        }
                    }

                    message = cts.IsCancellationRequested ? $"Operation interrupted by user." : $"The text was successfully scanned. {phrases.Count} unique phrases were extracted.";
                }

                IsBusy = false;
                NotificationMessage scannedMessage = new(message);
                WeakReferenceMessenger.Default.Send(scannedMessage);
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
        public bool CanScanText()
        {
            return !String.IsNullOrEmpty(ImportedText) && CalculationMethod != CalculationMethod.None && ((NumericValues != null && NumericValues.Count > 0) || (GetAllText && MinimumCharacters >= 0));
        }

        public RelayCommand GetFilePathCommand { get; private set; }
        public async void GetFilePath()
        {
            FileDialogMessage fdm = new();
            fdm.Message = "Select a file to scan";
            await WeakReferenceMessenger.Default.Send(fdm);
            string file = await fdm.Response;
            if (File.Exists(file))
            {
                FilePath = file;
            }
        }
        public bool CanGetFilePath()
        {
            return true;
        }

        public RelayCommand GetFolderPathCommand { get; private set; }
        public void GetFolderPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK && GetFolderPathCommand.CanExecute(fbd.SelectedPath))
            {
                ExportFolderPath = fbd.SelectedPath;
            }
        }
        public bool CanGetFolderPath()
        {
            return true;
        }

        private string filePath = "";
        public string FilePath
        {
            get => filePath;
            set
            {
                SetProperty(ref filePath, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private string importedText = "";
        public string ImportedText
        {
            get => importedText;
            set
            {
                SetProperty(ref importedText, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private bool getAllText = false;
        public bool GetAllText
        {
            get => getAllText;
            set
            {
                SetProperty(ref getAllText, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private int minimumCharacters = 3;
        public int MinimumCharacters
        {
            get => minimumCharacters;
            set
            {
                if (value < 0) value = 3;
                SetProperty(ref minimumCharacters, value);
            }
        }

        private int minimumWordsPerPhrase = 1;
        public int MinimumWordsPerPhrase
        {
            get => minimumWordsPerPhrase;
            set
            {
                if (value > MaximumWordsPerPhrase) value = MaximumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                SetProperty(ref minimumWordsPerPhrase, value);
            }
        }

        private int maximumWordsPerPhrase = 1;
        public int MaximumWordsPerPhrase
        {
            get => maximumWordsPerPhrase;
            set
            {
                if (value < MinimumWordsPerPhrase) value = MinimumWordsPerPhrase;
                if (value > 30 || value <= 0) value = 1;
                SetProperty(ref maximumWordsPerPhrase, value);
            }
        }

        private PhraseSeparator phraseSeparator = PhraseSeparator.All;
        public PhraseSeparator PhraseSeparator
        {
            get => phraseSeparator;
            set
            {
                if (value == PhraseSeparator.None)
                {
                    value = PhraseSeparator.NewLine | PhraseSeparator.Colon | PhraseSeparator.Comma | PhraseSeparator.FullStop | PhraseSeparator.GreekSemicolon | PhraseSeparator.Semicolon | PhraseSeparator.Space | PhraseSeparator.Tab;
                }
                SetProperty(ref phraseSeparator, value);
            }
        }

        private OperationViewModel currentOperation = new(new Operation());
        public OperationViewModel CurrentOperation
        {
            get => currentOperation;
            set
            {
                SetProperty(ref currentOperation, value);
                CurrentOperation.PropertyChanged += CurrentOperation_PropertyChanged;
                GroupNotifyCanExecuteChanged();
            }
        }

        private bool gridOutput = true;
        public bool GridOutput
        {
            get => gridOutput;
            set
            {
                SetProperty(ref gridOutput, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private bool fileOutput = false;
        public bool FileOutput
        {
            get => fileOutput;
            set
            {
                SetProperty(ref fileOutput, value);
                GroupNotifyCanExecuteChanged();
            }
        }

        private string exportFolderPath;
        public string ExportFolderPath
        {
            get => exportFolderPath;
            set
            {
                SetProperty(ref exportFolderPath, value);
                GroupNotifyCanExecuteChanged();
            }
        }
    }
}