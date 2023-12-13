/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using ArithmosApp.ViewModels.Services;
using ArithmosModels;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels;

public partial class ScannerViewModel : CommonViewModel
{
    public ScannerViewModel() : base(new GematriaMethodDataService(), new PhraseDataService(), new OperationDataService(), new SettingDataService())
    {
        WeakReferenceMessenger.Default.Register<ReloadOperationsMessage>(this, ReloadOperations);
        WeakReferenceMessenger.Default.Register<ReloadGematriaMethodsMessage>(this, ReloadGematriaMethods);
        WeakReferenceMessenger.Default.Register<RegenerateColumnsMessage>(this, RegenerateColumns);
        Operations = new(operationDataService.RetrieveAll());
        Phrases = [];
        SelectedPhrases = [];
        BusyMessage = "Scanning...";
    }

    #region Commands    
    [RelayCommand(CanExecute = nameof(CanAddValueToLookFor))]
    private void AddValueToLookFor()
    {
        if (ValueToLookFor == null || (ValueToLookFor < 0) || ValuesToLookFor.Contains((int)ValueToLookFor)) return;

        ValuesToLookFor.Add((int)ValueToLookFor);
        ValueToLookFor = null;
        ClearValuesToLookForCommand.NotifyCanExecuteChanged();
    }

    private bool CanAddValueToLookFor()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanRemoveValueToLookFor))]
    private void RemoveValueToLookFor(object value)
    {
        if (!ValuesToLookFor.Contains((int)value)) return;

        ValuesToLookFor.Remove((int)value);
        ClearValuesToLookForCommand.NotifyCanExecuteChanged();
    }

    private bool CanRemoveValueToLookFor(object value)
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanClearValuesToLookFor))]
    private void ClearValuesToLookFor()
    {
        if (ValuesToLookFor == null) return;

        ValuesToLookFor.Clear();
        ClearValuesToLookForCommand.NotifyCanExecuteChanged();
    }

    private bool CanClearValuesToLookFor()
    {
        return ValuesToLookFor != null && ValuesToLookFor.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanClearAllResults))]
    private void ClearAllResults()
    {
        Phrases.Clear();
        GridActionsNotify();
    }

    private bool CanClearAllResults()
    {
        return Phrases != null && Phrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopyResults))]
    private void CopyResults()
    {
        StringBuilder stringBuilder = new();
        foreach (PhraseViewModel phrase in ResultsGridSource.RowSelection.SelectedItems)
        {
            stringBuilder.AppendLine(phrase.Phrase.Content);
        }
        ClipboardMessage clipboardMessage = new() { Value = stringBuilder.ToString() };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopyResults()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopyResultsCSV))]
    private void CopyResultsCSV()
    {
        List<Phrase> phrases = [];
        foreach (PhraseViewModel phrase in ResultsGridSource.RowSelection.SelectedItems)
        {
            phrases.Add(phrase.Phrase);
        }
        Exporter exporter = new(phrases, cancellationTokenSource.Token);
        ClipboardMessage clipboardMessage = new() { Value = exporter.Data };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopyResultsCSV()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanExportResultsGrid))]
    private async Task ExportResultsGridAsync()
    {
        try
        {
            IsBusy = true;
            BusyMessage = "Exporting...";
            await Task.Run(async () =>
            {
                List<Phrase> phrases = [];
                foreach (PhraseViewModel phrase in Phrases)
                {
                    phrases.Add(phrase.Phrase);
                }
                cancellationTokenSource = new();
                Exporter exporter = new(phrases, cancellationTokenSource.Token);
                await exporter.ExportAsync();
            });
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanExportResultsGrid()
    {
        return Phrases != null && Phrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanMoveResults))]
    private void MoveResults()
    {
        for (int i = 0; i < ResultsGridSource.RowSelection.SelectedItems.Count; i++)
        {
            SelectedPhrases.Add(ResultsGridSource.RowSelection.SelectedItems[i]);
        }
        if (ResultsGridSource.RowSelection.SelectedItems.Count < 5000)
        {
            List<int> indexes = [];
            foreach (IndexPath index in ResultsGridSource.RowSelection.SelectedIndexes)
            {
                indexes.Add(index[0]);
            }
            indexes.Sort();
            indexes.Reverse();
            foreach (int index in indexes)
            {
                Phrases.RemoveAt(index);
            }
        }
        else
        {
            HashSet<PhraseViewModel> selectedPhraseViewModels = new(ResultsGridSource.RowSelection.SelectedItems);
            HashSet<PhraseViewModel> phraseViewModels = new(Phrases);
            Phrases.Clear();
            foreach (PhraseViewModel phraseViewModel in phraseViewModels)
            {
                if (!selectedPhraseViewModels.Contains(phraseViewModel))
                {
                    Phrases.Add(phraseViewModel);
                }
            }
        }
        GridActionsNotify();
    }

    private bool CanMoveResults()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanMoveAllResults))]
    private void MoveAllResults()
    {
        foreach (PhraseViewModel phrase in Phrases)
        {
            SelectedPhrases.Add(phrase);
        }
        Phrases.Clear();
        GridActionsNotify();
    }

    private bool CanMoveAllResults()
    {
        return Phrases != null && Phrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanClearAllSelections))]
    private void ClearAllSelections()
    {
        SelectedPhrases.Clear();
        GridActionsNotify();
    }

    private bool CanClearAllSelections()
    {
        return SelectedPhrases != null && SelectedPhrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopySelections))]
    private void CopySelections()
    {
        StringBuilder stringBuilder = new();
        foreach (PhraseViewModel phrase in SelectionsGridSource.RowSelection.SelectedItems)
        {
            stringBuilder.AppendLine(phrase.Phrase.Content);
        }
        ClipboardMessage clipboardMessage = new() { Value = stringBuilder.ToString() };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopySelections()
    {
        return SelectionsGridSource?.RowSelection.SelectedItems != null && SelectionsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopySelectionsCSV))]
    private void CopySelectionsCSV()
    {
        List<Phrase> phrases = [];
        foreach (PhraseViewModel phrase in SelectionsGridSource.RowSelection.SelectedItems)
        {
            phrases.Add(phrase.Phrase);
        }
        Exporter exporter = new(phrases, cancellationTokenSource.Token);
        ClipboardMessage clipboardMessage = new() { Value = exporter.Data };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopySelectionsCSV()
    {
        return SelectionsGridSource?.RowSelection.SelectedItems != null && SelectionsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanExportSelectionsGrid))]
    private async Task ExportSelectionsGridAsync()
    {
        try
        {
            IsBusy = true;
            BusyMessage = "Exporting...";
            await Task.Run(async () =>
            {
                List<Phrase> phrases = [];
                foreach (PhraseViewModel phrase in SelectedPhrases)
                {
                    phrases.Add(phrase.Phrase);
                }
                cancellationTokenSource = new();
                Exporter exporter = new(phrases, cancellationTokenSource.Token);
                await exporter.ExportAsync();
            });
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanExportSelectionsGrid()
    {
        return SelectedPhrases != null && SelectedPhrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanMoveSelections))]
    private void MoveSelections()
    {
        for (int i = 0; i < SelectionsGridSource.RowSelection.SelectedItems.Count; i++)
        {
            Phrases.Add(SelectionsGridSource.RowSelection.SelectedItems[i]);
        }
        if (SelectionsGridSource.RowSelection.SelectedItems.Count < 5000)
        {
            List<int> indexes = [];
            foreach (IndexPath index in SelectionsGridSource.RowSelection.SelectedIndexes)
            {
                indexes.Add(index[0]);
            }
            indexes.Sort();
            indexes.Reverse();
            foreach (int index in indexes)
            {
                SelectedPhrases.RemoveAt(index);
            }
        }
        else
        {
            HashSet<PhraseViewModel> selectedPhraseViewModels = new(SelectionsGridSource.RowSelection.SelectedItems);
            HashSet<PhraseViewModel> phraseViewModels = new(SelectedPhrases);
            SelectedPhrases.Clear();
            foreach (PhraseViewModel phraseViewModel in phraseViewModels)
            {
                if (!selectedPhraseViewModels.Contains(phraseViewModel))
                {
                    SelectedPhrases.Add(phraseViewModel);
                }
            }
        }
        GridActionsNotify();
    }

    private bool CanMoveSelections()
    {
        return SelectionsGridSource?.RowSelection.SelectedItems != null && SelectionsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanMoveAllSelections))]
    private void MoveAllSelections()
    {
        foreach (PhraseViewModel phrase in SelectedPhrases)
        {
            Phrases.Add(phrase);
        }
        SelectedPhrases.Clear();
        GridActionsNotify();
    }

    private bool CanMoveAllSelections()
    {
        return SelectedPhrases != null && SelectedPhrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanScan))]
    private async Task ScanAsync()
    {
        if (!IsScannerValid(out string message))
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
            return;
        }
        try
        {
            IsBusy = true;
            BusyMessage = "Scanning...";
            List<Phrase> phrasesFound = [];
            cancellationTokenSource = new();
            Scanner scanner = new(GetAllGematriaMethods(), GetSelectedGematriaMethods(), cancellationTokenSource.Token, GetAllText ? null : ValuesToLookFor.ToHashSet())
            {
                MinimumCharactersPerPhrase = MinimumCharactersPerPhrase,
                MinimumWordsPerPhrase = MinimumWordsPerPhrase,
                MaximumWordsPerPhrase = MaximumWordsPerPhrase,
                TextSeparators = GetTextSeparator().GetSelectedSeparators()
            };
            Phrases.Clear();
            try
            {
                phrasesFound = TextSource ? await scanner.ScanTextAsync(TextToScan) : await scanner.ScanFileAsync(FilePathToScan);
            }
            catch (OperationCanceledException)
            {
                Phrases.Clear();
            }
            if (OutputToFile)
            {
                Exporter exporter = new(phrasesFound, cancellationTokenSource.Token);
                await exporter.ExportAsync();
            }
            if (OutputToGrid)
            {
                foreach (Phrase phrase in phrasesFound)
                {
                    Phrases.Add(new PhraseViewModel(phrase));
                }
            }
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                message = $"Scan completed. {phrasesFound.Count}" + (phrasesFound.Count == 1 ? " phrase was found." : " phrases were found.");
            }
            else
            {
                message = "Scan was cancelled by the user.";
            }
            NotificationMessage scannedMessage = new(message);
            WeakReferenceMessenger.Default.Send(scannedMessage);
            GridActionsNotify();
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanScan()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanSaveSelections))]
    private async Task SaveSelectionsAsync()
    {
        if (!IsSavingSelectionsValid(out string message))
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
            return;
        }
        try
        {
            IsBusy = true;
            BusyMessage = "Saving...";
            List<Phrase> phrases = [];
            foreach (PhraseViewModel phraseViewModel in SelectedPhrases)
            {
                phrases.Add(phraseViewModel.Phrase);
            }
            Operation operation = Operations.SingleOrDefault(o => o.Name == OperationDescription) ?? new Operation() { Name = OperationDescription, EntryDate = DateTime.Now };
            cancellationTokenSource = new();
            int result = await Task.Run(async () =>
            {
                try
                {
                    return await phraseDataService.CreateAsync(phrases, cancellationTokenSource.Token, operation);
                }
                catch (OperationCanceledException)
                {
                    return -1;
                }
            });
            ClearAllSelections();
            OperationDescription = "";
            if (result >= 0)
            {
                int duplicatePhrases = phrases.Count - result;
                message = $"Saved {result} of {phrases.Count} phrases." + ((duplicatePhrases > 0) ? $" {duplicatePhrases} phrases were already present in the database." : "");
                WeakReferenceMessenger.Default.Send<ReloadOperationsMessage>();
            }
            else
            {
                message = "Operation was canceled by the user. Nothing was saved.";
            }
            NotificationMessage scannedMessage = new(message);
            WeakReferenceMessenger.Default.Send(scannedMessage);
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanSaveSelections()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanGetFilePath))]
    private async Task GetFilePathAsync()
    {
        FileDialogMessage fdm = new() { Message = "Select a file to scan" };
        await WeakReferenceMessenger.Default.Send(fdm);
        string file = await fdm.Response;
        if (File.Exists(file))
        {
            FilePathToScan = file;
        }
    }

    private bool CanGetFilePath()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanCancelOperation))]
    private void CancelOperation()
    {
        cancellationTokenSource.Cancel();
    }

    private bool CanCancelOperation()
    {
        return true;
    }
    #endregion

    #region Methods
    private bool IsScannerValid(out string message)
    {
        message = string.Empty;
        if (!GetAllText && (ValuesToLookFor == null || ValuesToLookFor.Count == 0))
        {
            message = "You must either add some values to look for, or select the 'Get All Text' option.";
        }
        else if (!GematriaMethodsViewModels.Where(m => m.IsSelected).Any())
        {
            message = "You must select at least one gematria method.";
        }
        else if (GetTextSeparator().GetSelectedSeparators().Length == 0)
        {
            message = "You must select at least one phrase separator.";
        }
        else if (!OutputToFile && !OutputToGrid)
        {
            message = "You must select at least one output location.";
        }
        else if (FileSource && string.IsNullOrWhiteSpace(FilePathToScan))
        {
            message = "You must enter a valid file path";
        }
        else if (FileSource && !string.IsNullOrWhiteSpace(FilePathToScan) && !File.Exists(FilePathToScan))
        {
            message = "The file path you entered could not be found.";
        }
        else if (TextSource && string.IsNullOrWhiteSpace(TextToScan))
        {
            message = "The text to be scanned must not be empty.";
        }
        return string.IsNullOrWhiteSpace(message);
    }

    private bool IsSavingSelectionsValid(out string message)
    {
        message = string.Empty;
        if (SelectedPhrases == null || SelectedPhrases.Count == 0)
        {
            message = "You must select at least one phrase to be saved.";
        }
        else if (string.IsNullOrWhiteSpace(OperationDescription))
        {
            message = "The description cannot be empty";
        }
        else if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0)
        {
            message = "There are no gematria methods. You must add at least one method in order to save.";
        }
        return string.IsNullOrWhiteSpace(message);
    }

    private void GenerateResultsColumns()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        ResultsGridSource = new FlatTreeDataGridSource<PhraseViewModel>(Phrases);
        ResultsGridSource.RowSelection.SelectionChanged += Results_RowSelection_SelectionChanged;
        ResultsGridSource.RowSelection!.SingleSelect = false;
        ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Phrase",
            x => x.Content, options: new()
            {
                MinWidth = new GridLength(100),
                CanUserResizeColumn = true
            }));
        foreach (KeyValuePair<string, int> kvp in (new PhraseViewModel(new Phrase("Initialize", GetAllGematriaMethods())).Values))
        {
            ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>(kvp.Key,
                p => p.Values[kvp.Key],
                options: new()
                {
                    MinWidth = new GridLength(85),
                    CanUserResizeColumn = true
                }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnAlphabet) == Constants.Settings.True)
        {
            ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Alphabet",
          x => x.Alphabet, options: new()
          {
              MinWidth = new GridLength(85),
              CanUserResizeColumn = true
          }));
        }
    }

    private void Results_RowSelection_SelectionChanged(object sender, Avalonia.Controls.Selection.TreeSelectionModelSelectionChangedEventArgs<PhraseViewModel> e)
    {
        GridActionsNotify();
        ResultsGridCountLabel = ResultsGridSource != null ? GetCountLabel("Scanner Results", ResultsGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Scanner Results", 0, 0);
    }

    private void Phrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        ResultsGridCountLabel = ResultsGridSource != null ? GetCountLabel("Scanner Results", ResultsGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Scanner Results", 0, 0);
    }

    private void GenerateSelectionsColumns()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        SelectionsGridSource = new FlatTreeDataGridSource<PhraseViewModel>(SelectedPhrases);
        SelectionsGridSource.RowSelection.SelectionChanged += Selections_RowSelection_SelectionChanged;
        SelectionsGridSource.RowSelection!.SingleSelect = false;
        SelectionsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Phrase",
            x => x.Content, options: new()
            {
                MinWidth = new GridLength(100),
                CanUserResizeColumn = true
            }));
        foreach (KeyValuePair<string, int> kvp in (new PhraseViewModel(new Phrase("Initialize", GetAllGematriaMethods())).Values))
        {
            SelectionsGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>(kvp.Key,
                p => p.Values[kvp.Key],
                options: new()
                {
                    MinWidth = new GridLength(85),
                    CanUserResizeColumn = true
                }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnAlphabet) == Constants.Settings.True)
        {
            SelectionsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Alphabet",
          x => x.Alphabet, options: new()
          {
              MinWidth = new GridLength(85),
              CanUserResizeColumn = true
          }));
        }
    }

    private void Selections_RowSelection_SelectionChanged(object sender, Avalonia.Controls.Selection.TreeSelectionModelSelectionChangedEventArgs<PhraseViewModel> e)
    {
        GridActionsNotify();
        SelectionsGridCountLabel = SelectionsGridSource != null ? GetCountLabel("Selected Results", SelectionsGridSource.RowSelection.Count, SelectedPhrases.Count) : GetCountLabel("Selected Results", 0, 0);
    }

    private void SelectedPhrases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        SelectionsGridCountLabel = SelectionsGridSource != null ? GetCountLabel("Selected Results", SelectionsGridSource.RowSelection.Count, SelectedPhrases.Count) : GetCountLabel("Selected Results", 0, 0);
    }

    private void RegenerateColumns(object recipient, RegenerateColumnsMessage message)
    {
        GenerateResultsColumns();
        GenerateSelectionsColumns();
    }

    private TextSeparator GetTextSeparator()
    {
        ArithmosModels.Enums.Separator separator = ArithmosModels.Enums.Separator.None;
        if (NewLine) separator |= ArithmosModels.Enums.Separator.NewLine;
        if (Comma) separator |= ArithmosModels.Enums.Separator.Comma;
        if (Semicolon) separator |= ArithmosModels.Enums.Separator.Semicolon;
        if (GreekSemicolon) separator |= ArithmosModels.Enums.Separator.GreekSemicolon;
        if (Tab) separator |= ArithmosModels.Enums.Separator.Tab;
        if (Colon) separator |= ArithmosModels.Enums.Separator.Colon;
        if (FullStop) separator |= ArithmosModels.Enums.Separator.FullStop;
        if (Pipe) separator |= ArithmosModels.Enums.Separator.Pipe;
        if (Space) separator |= ArithmosModels.Enums.Separator.Space;
        return new TextSeparator(separator);
    }

    private List<GematriaMethod> GetSelectedGematriaMethods()
    {
        List<GematriaMethod> methods = [];
        foreach (GematriaMethodViewModel methodViewModel in GematriaMethodsViewModels.Where(m => m.IsSelected))
        {
            methods.Add(methodViewModel.GetModel());
        }
        return methods;
    }

    private void GridActionsNotify()
    {
        ClearAllResultsCommand.NotifyCanExecuteChanged();
        MoveResultsCommand.NotifyCanExecuteChanged();
        MoveAllResultsCommand.NotifyCanExecuteChanged();
        CopyResultsCommand.NotifyCanExecuteChanged();
        CopyResultsCSVCommand.NotifyCanExecuteChanged();
        ExportResultsGridCommand.NotifyCanExecuteChanged();
        ClearAllSelectionsCommand.NotifyCanExecuteChanged();
        CopySelectionsCommand.NotifyCanExecuteChanged();
        CopySelectionsCSVCommand.NotifyCanExecuteChanged();
        ExportSelectionsGridCommand.NotifyCanExecuteChanged();
        MoveSelectionsCommand.NotifyCanExecuteChanged();
        MoveAllSelectionsCommand.NotifyCanExecuteChanged();
    }

    private void ReloadOperations(object recipient, ReloadOperationsMessage message)
    {
        Operations = new(operationDataService.RetrieveAll());
    }

    private void ReloadGematriaMethods(object recipient, ReloadGematriaMethodsMessage message)
    {
        List<GematriaMethod> gematriaMethods = gematriaMethodDataService.RetrieveAll().ToList();
        GematriaMethodsViewModels.Clear();
        foreach (GematriaMethod gematriaMethod in gematriaMethods)
        {
            GematriaMethodsViewModels.Add(new GematriaMethodViewModel(gematriaMethod));
        }
        GenerateResultsColumns();
        GenerateSelectionsColumns();
    }
    #endregion

    #region Properties
    private FlatTreeDataGridSource<PhraseViewModel> resultsGridSource;
    public FlatTreeDataGridSource<PhraseViewModel> ResultsGridSource
    {
        get => resultsGridSource;
        set => SetProperty(ref resultsGridSource, value);
    }

    private FlatTreeDataGridSource<PhraseViewModel> selectionsGridSource;
    public FlatTreeDataGridSource<PhraseViewModel> SelectionsGridSource
    {
        get => selectionsGridSource;
        set => SetProperty(ref selectionsGridSource, value);
    }

    private ObservableCollection<PhraseViewModel> phrases = [];
    public ObservableCollection<PhraseViewModel> Phrases
    {
        get => phrases;
        set
        {
            SetProperty(ref phrases, value);
            GenerateResultsColumns();
            Phrases.CollectionChanged += Phrases_CollectionChanged;

        }
    }

    private ObservableCollection<PhraseViewModel> selectedPhrases;
    public ObservableCollection<PhraseViewModel> SelectedPhrases
    {
        get => selectedPhrases;
        set
        {
            SetProperty(ref selectedPhrases, value);
            GenerateSelectionsColumns();
            SelectedPhrases.CollectionChanged += SelectedPhrases_CollectionChanged;
        }
    }

    private ObservableCollection<Operation> operations;
    public ObservableCollection<Operation> Operations
    {
        get => operations;
        set => SetProperty(ref operations, value);
    }

    private int? valueToLookFor;
    public int? ValueToLookFor
    {
        get => valueToLookFor;
        set => SetProperty(ref valueToLookFor, value);
    }

    private ObservableCollection<int> valuesToLookFor = [];
    public ObservableCollection<int> ValuesToLookFor
    {
        get => valuesToLookFor;
        set => SetProperty(ref valuesToLookFor, value);
    }

    public int MinimumCharactersPerPhrase { get; set; } = 3;

    public int MinimumWordsPerPhrase { get; set; } = 1;

    public int MaximumWordsPerPhrase { get; set; } = 1;

    private bool getAllText = false;
    public bool GetAllText
    {
        get => getAllText;
        set => SetProperty(ref getAllText, value);
    }

    public bool NewLine { get; set; } = true;

    public bool Comma { get; set; } = true;

    public bool Semicolon { get; set; } = true;

    public bool GreekSemicolon { get; set; } = true;

    public bool Tab { get; set; } = true;

    public bool Colon { get; set; } = true;

    public bool FullStop { get; set; } = true;

    public bool Pipe { get; set; } = true;

    public bool Space { get; set; } = true;

    public bool OutputToGrid { get; set; } = true;

    public bool OutputToFile { get; set; } = false;

    private bool fileSource;
    public bool FileSource
    {
        get => fileSource;
        set => SetProperty(ref fileSource, value);
    }

    private bool textSource = true;
    public bool TextSource
    {
        get => textSource;
        set => SetProperty(ref textSource, value);
    }

    private string filePathToScan;
    public string FilePathToScan
    {
        get => filePathToScan;
        set => SetProperty(ref filePathToScan, value);
    }

    public string TextToScan { get; set; }

    private string operationDescription;
    public string OperationDescription
    {
        get => operationDescription;
        set => SetProperty(ref operationDescription, value);
    }

    private string resultsGridCountLabel = "Scanner Results";
    public string ResultsGridCountLabel
    {
        get => resultsGridCountLabel;
        set => SetProperty(ref resultsGridCountLabel, value);
    }

    private string selectionsGridCountLabel = "Selected Results";
    public string SelectionsGridCountLabel
    {
        get => selectionsGridCountLabel;
        set => SetProperty(ref selectionsGridCountLabel, value);
    }
    #endregion
}
