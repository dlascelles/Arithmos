/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using ArithmosApp.ViewModels.Services;
using ArithmosModels;
using ArithmosModels.Extensions;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels;

public partial class ExplorerViewModel : CommonViewModel
{
    public ExplorerViewModel() : base(new GematriaMethodDataService(), new PhraseDataService(), new OperationDataService(), new SettingDataService())
    {
        WeakReferenceMessenger.Default.Register<ReloadOperationsMessage>(this, ReloadOperations);
        WeakReferenceMessenger.Default.Register<ReloadGematriaMethodsMessage>(this, ReloadGematriaMethods);
        WeakReferenceMessenger.Default.Register<RegenerateColumnsMessage>(this, RegenerateColumns);
        Operations = new(this.operationDataService.RetrieveAll());
        Phrases = [];
    }

    #region Commands
    [RelayCommand(CanExecute = nameof(CanAddValueToLookFor))]
    private void AddValueToLookFor()
    {
        if (ValueToLookFor == null || !(ValueToLookFor > 0) || ValuesToLookFor.Contains((int)ValueToLookFor)) return;

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


    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task SearchAsync()
    {
        if (!IsSearchValid(out string message))
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
            return;
        }
        IsBusy = true;
        BusyMessage = "Searching...";
        try
        {
            List<PhraseViewModel> phrases = await Task.Run(async () =>
            {
                List<PhraseViewModel> phraseViewModels = [];
                List<GematriaMethod> selectedMethods = [];
                foreach (GematriaMethodViewModel gematriaMethodViewModel in GematriaMethodsViewModels.Where(g => g.IsSelected))
                {
                    selectedMethods.Add(gematriaMethodViewModel.GetModel());
                }
                List<Phrase> phrases = SearchByValues ? await phraseDataService.SearchByValuesAsync(selectedMethods, ValuesToLookFor.ToList()) : await phraseDataService.SearchByTextAsync(TextToSearch);
                if (OutputToGrid)
                {
                    foreach (Phrase phrase in phrases)
                    {
                        phraseViewModels.Add(new PhraseViewModel(phrase));
                    }
                }
                if (OutputToFile)
                {
                    Exporter exporter = new(phrases, cancellationTokenSource.Token);
                    await exporter.ExportAsync();
                }
                return phraseViewModels;
            });
            Phrases = new(phrases);
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            UpdateGridLabel();
        }
    }

    private bool CanSearch()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanLoadOperations))]
    private async Task LoadOperationsAsync()
    {
        IsBusy = true;
        BusyMessage = "Loading...";
        try
        {
            List<Operation> selectedOperations = OperationsGridSource?.RowSelection?.SelectedItems?.ToList();
            if (selectedOperations != null && selectedOperations.Count != 0)
            {
                List<Phrase> phrases = await Task.Run(async () =>
                {
                    List<Phrase> phrases = [];
                    foreach (Operation operation in selectedOperations)
                    {
                        foreach (Phrase phrase in await phraseDataService.RetrieveByOperationAsync(operation.Id))
                        {
                            phrases.Add(phrase);
                        }
                    }
                    return phrases;
                });
                Phrases.Clear();
                if (OutputToGrid)
                {
                    foreach (Phrase phrase in phrases)
                    {
                        Phrases.Add(new PhraseViewModel(phrase));
                    }
                }
                if (OutputToFile)
                {
                    Exporter exporter = new(phrases, cancellationTokenSource.Token);
                    await exporter.ExportAsync();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            UpdateGridLabel();
        }
    }

    private bool CanLoadOperations()
    {
        return OperationsGridSource.RowSelection.SelectedItems != null && OperationsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanLoadOrphans))]
    private async Task LoadOrphansAsync()
    {
        IsBusy = true;
        BusyMessage = "Loading...";
        try
        {
            List<Phrase> phrases = await Task.Run(async () =>
            {
                List<Phrase> phrases = [];
                foreach (Phrase phrase in await phraseDataService.RetrieveAllOrphansAsync())
                {
                    phrases.Add(phrase);
                }
                return phrases;
            });
            Phrases.Clear();
            if (OutputToGrid)
            {
                foreach (Phrase phrase in phrases)
                {
                    Phrases.Add(new PhraseViewModel(phrase));
                }
            }
            if (OutputToFile)
            {
                Exporter exporter = new(phrases, cancellationTokenSource.Token);
                await exporter.ExportAsync();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            UpdateGridLabel();
        }
    }

    private bool CanLoadOrphans()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanDeleteSelectedOperations))]
    private async Task DeleteSelectedOperationsAsync()
    {
        ConfirmationMessage confirmationMessage = new() { Message = "Are you sure you want to delete the selected operation(s)?" };
        Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(confirmationMessage));
        while (!confirmationMessage.HasReceivedResponse) { await Task.Delay(500); }
        if (await confirmationMessage.Response != true) return;

        IsBusy = true;
        BusyMessage = "Deleting...";
        try
        {
            List<Operation> selectedOperations = OperationsGridSource.RowSelection.SelectedItems.ToList();
            if (selectedOperations != null && selectedOperations.Count != 0)
            {
                int result = await Task.Run(async () =>
                {
                    return await operationDataService.DeleteAsync(selectedOperations.Select(operation => operation.Id).ToList());
                });
            }
            WeakReferenceMessenger.Default.Send<ReloadOperationsMessage>();
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            UpdateGridLabel();
        }
        OperationsGridActionsNotify();
    }

    private bool CanDeleteSelectedOperations()
    {
        return OperationsGridSource.RowSelection.SelectedItems != null && OperationsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanClearAllResults))]
    private void ClearAllResults()
    {
        Phrases.Clear();
        ResultsGridActionsNotify();
        UpdateGridLabel();
    }

    private bool CanClearAllResults()
    {
        return Phrases != null && Phrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopy))]
    private void Copy()
    {
        StringBuilder stringBuilder = new();
        foreach (PhraseViewModel phrase in ResultsGridSource.RowSelection.SelectedItems)
        {
            stringBuilder.AppendLine(phrase.Phrase.Content);
        }
        ClipboardMessage clipboardMessage = new() { Value = stringBuilder.ToString() };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopy()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopyCSV))]
    private void CopyCSV()
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

    private bool CanCopyCSV()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanExportGrid))]
    private async Task ExportGridAsync()
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

    private bool CanExportGrid()
    {
        return Phrases != null && Phrases.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanDeleteSelectedPhrases))]
    private async Task DeleteSelectedPhrasesAsync()
    {
        ConfirmationMessage confirmationMessage = new() { Message = "Are you sure you want to delete the selected phrase(s)?" };
        Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(confirmationMessage));
        while (!confirmationMessage.HasReceivedResponse) { await Task.Delay(500); }
        if (await confirmationMessage.Response != true) return;

        IsBusy = true;
        BusyMessage = "Deleting...";
        try
        {
            List<PhraseViewModel> selectedPhrases = ResultsGridSource.RowSelection.SelectedItems.ToList();
            if (selectedPhrases != null && selectedPhrases.Count != 0)
            {
                long result = await Task.Run(async () =>
                {
                    return await phraseDataService.DeleteAsync(selectedPhrases.Select(phrase => phrase.Phrase.Id).ToList());
                });
            }
            foreach (PhraseViewModel phrase in selectedPhrases)
            {
                Phrases.Remove(phrase);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            ResultsGridActionsNotify();
            UpdateGridLabel();
        }
    }

    private bool CanDeleteSelectedPhrases()
    {
        return ResultsGridSource?.RowSelection.SelectedItems != null && ResultsGridSource.RowSelection.SelectedItems.Count > 0;
    }
    #endregion

    #region Methods
    private void GenerateResultsColumns()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        ResultsGridSource = new FlatTreeDataGridSource<PhraseViewModel>(Phrases);
        ResultsGridSource.RowSelection.SelectionChanged += ResultsRowSelection_SelectionChanged;
        ResultsGridSource.Rows.CollectionChanged += ResultsRows_CollectionChanged;
        ResultsGridSource.RowSelection!.SingleSelect = false;
        ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Phrase",
            x => x.Phrase.Content, options: new()
            {
                MinWidth = new GridLength(100),
                CanUserResizeColumn = true,
            }));
        foreach (((int Id, string Name) GematriaMethod, int Value) values in (new PhraseViewModel(new Phrase("Initialize", GetAllGematriaMethods())).Phrase.Values))
        {
            ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>(values.GematriaMethod.Name,
                p => p.Phrase.GetValue(values.GematriaMethod.Name),
                options: new()
                {
                    MinWidth = new GridLength(85),
                    CanUserResizeColumn = true
                }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnAlphabet) == Constants.Settings.True)
        {
            ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Alphabet",
              x => x.Phrase.Alphabet.ToString(), options: new()
              {
                  MinWidth = new GridLength(85),
                  CanUserResizeColumn = true
              }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnOperationId) == Constants.Settings.True)
        {
            ResultsGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>("Op. Id",
          x => x.Phrase.OperationId, options: new()
          {
              MinWidth = new GridLength(60),
              CanUserResizeColumn = true
          }));
        }
    }

    private void ResultsRows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        ResultsGridActionsNotify();
    }

    private void ResultsRowSelection_SelectionChanged(object sender, Avalonia.Controls.Selection.TreeSelectionModelSelectionChangedEventArgs<PhraseViewModel> e)
    {
        ResultsGridActionsNotify();
        UpdateGridLabel();
    }

    private void GenerateOperationsColumns()
    {
        OperationsGridSource = new FlatTreeDataGridSource<Operation>(Operations);
        OperationsGridSource.RowSelection.SelectionChanged += OperationRowSelection_SelectionChanged;
        OperationsGridSource.RowSelection!.SingleSelect = false;
        OperationsGridSource.Columns.Add(new TextColumn<Operation, int>("Id",
            x => x.Id, options: new()
            {
                CanUserResizeColumn = true
            }));
        OperationsGridSource.Columns.Add(new TextColumn<Operation, string>("Name",
            x => x.Name, options: new()
            {
                CanUserResizeColumn = true
            }));
        OperationsGridSource.Columns.Add(new TextColumn<Operation, DateTime>("Date",
            x => x.EntryDate, options: new()
            {
                CanUserResizeColumn = true
            }));
    }

    private void OperationRowSelection_SelectionChanged(object sender, Avalonia.Controls.Selection.TreeSelectionModelSelectionChangedEventArgs<Operation> e)
    {
        OperationsGridActionsNotify();
    }

    private void RegenerateColumns(object recipient, RegenerateColumnsMessage message)
    {
        GenerateResultsColumns();
    }

    private bool IsSearchValid(out string message)
    {
        message = string.Empty;
        if (SearchByValues && !GematriaMethodsViewModels.Where(m => m.IsSelected).Any())
        {
            message = "You must select at least one gematria method.";
        }
        else if (SearchByValues && (ValuesToLookFor == null || ValuesToLookFor.Count == 0))
        {
            message = "You must add at least one value to look for";
        }
        else if (SearchByText && string.IsNullOrWhiteSpace(TextToSearch))
        {
            message = "The text to search for, must not be empty";
        }
        else if (!OutputToFile && !OutputToGrid)
        {
            message = "You must select at least one output location.";
        }
        return string.IsNullOrWhiteSpace(message);
    }

    private void OperationsGridActionsNotify()
    {
        LoadOperationsCommand.NotifyCanExecuteChanged();
        DeleteSelectedOperationsCommand.NotifyCanExecuteChanged();
    }

    private void ResultsGridActionsNotify()
    {
        ClearAllResultsCommand.NotifyCanExecuteChanged();
        CopyCommand.NotifyCanExecuteChanged();
        CopyCSVCommand.NotifyCanExecuteChanged();
        ExportGridCommand.NotifyCanExecuteChanged();
        DeleteSelectedPhrasesCommand.NotifyCanExecuteChanged();
    }

    private void ReloadOperations(object recipient, ReloadOperationsMessage message)
    {
        Operations.Clear();
        try
        {
            operationDataService.RetrieveAll().ToList().ForEach(operation => Operations.Add(operation));
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
    }

    private void ReloadGematriaMethods(object recipient, ReloadGematriaMethodsMessage message)
    {
        GematriaMethodsViewModels.Clear();
        try
        {
            foreach (GematriaMethod gematriaMethod in gematriaMethodDataService.RetrieveAll().ToList())
            {
                GematriaMethodsViewModels.Add(new GematriaMethodViewModel(gematriaMethod));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        GenerateResultsColumns();
    }

    private void UpdateGridLabel()
    {
        ResultsGridCountLabel = ResultsGridSource != null ? GetCountLabel("Database Results", ResultsGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Database Results", 0, 0);
    }
    #endregion

    #region Properties
    private FlatTreeDataGridSource<PhraseViewModel> resultsGridSource;
    public FlatTreeDataGridSource<PhraseViewModel> ResultsGridSource
    {
        get => resultsGridSource;
        set => SetProperty(ref resultsGridSource, value);
    }

    private FlatTreeDataGridSource<Operation> operationsGridSource;
    public FlatTreeDataGridSource<Operation> OperationsGridSource
    {
        get => operationsGridSource;
        set => SetProperty(ref operationsGridSource, value);
    }

    private ObservableCollection<PhraseViewModel> phrases;
    public ObservableCollection<PhraseViewModel> Phrases
    {
        get => phrases;
        set
        {
            SetProperty(ref phrases, value);
            GenerateResultsColumns();
            ResultsGridActionsNotify();
        }
    }

    private ObservableCollection<Operation> operations;
    public ObservableCollection<Operation> Operations
    {
        get => operations;
        set
        {
            SetProperty(ref operations, value);
            GenerateOperationsColumns();
        }
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

    public bool OutputToGrid { get; set; } = true;

    public bool OutputToFile { get; set; } = false;

    private bool searchByValues = true;
    public bool SearchByValues
    {
        get => searchByValues;
        set => SetProperty(ref searchByValues, value);
    }

    private bool searchByText = false;
    public bool SearchByText
    {
        get => searchByText;
        set => SetProperty(ref searchByText, value);
    }

    private string textToSearch;
    public string TextToSearch
    {
        get => textToSearch;
        set => SetProperty(ref textToSearch, value.RemoveNewLines(" ").RemoveExtraSpaces());
    }

    private string resultsGridCountLabel = "Database Results";
    public string ResultsGridCountLabel
    {
        get => resultsGridCountLabel;
        set => SetProperty(ref resultsGridCountLabel, value);
    }
    #endregion
}
