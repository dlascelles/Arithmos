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

public partial class CalculatorViewModel : CommonViewModel
{
    public CalculatorViewModel() : base(new GematriaMethodDataService(), new PhraseDataService(), new OperationDataService(), new SettingDataService())
    {
        WeakReferenceMessenger.Default.Register<ReloadGematriaMethodsMessage>(this, ReloadGematriaMethods);
        WeakReferenceMessenger.Default.Register<RegenerateColumnsMessage>(this, RegenerateColumns);
        Phrases = [];
    }

    #region Commands    
    [RelayCommand(CanExecute = nameof(CanSaveCurrentPhrase))]
    private void SaveCurrentPhrase()
    {
        if (CurrentPhrase == null || string.IsNullOrWhiteSpace(CurrentPhrase.Content)) return;

        try
        {
            phraseDataService.Create(CurrentPhrase.Phrase, null);
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
    }

    private bool CanSaveCurrentPhrase()
    {
        return CurrentPhrase != null && !string.IsNullOrWhiteSpace(CurrentPhrase.Content);
    }


    [RelayCommand(CanExecute = nameof(CanSearchByCurrentPhrase))]
    private async Task SearchByCurrentPhraseAsync()
    {
        if (CurrentPhrase == null || string.IsNullOrWhiteSpace(CurrentPhrase.Content)) return;

        IsBusy = true;
        BusyMessage = "Searching...";
        try
        {
            List<PhraseViewModel> phrases = await Task.Run(async () =>
            {
                List<int> selectedMethodsIds = [];
                foreach (GematriaMethodViewModel gematriaMethodViewModel in GematriaMethodsViewModels)
                {
                    if (gematriaMethodViewModel.IsSelected)
                    {
                        selectedMethodsIds.Add(gematriaMethodViewModel.Id);
                    }
                }
                List<PhraseViewModel> phraseViewModels = [];
                foreach (Phrase phrase in await phraseDataService.SearchForSimilarPhrasesAsync(currentPhrase.Phrase, selectedMethodsIds.ToArray()))
                {
                    phraseViewModels.Add(new PhraseViewModel(phrase));
                }
                return phraseViewModels;
            });
            Phrases = new(phrases);
            ResultsGridCountLabel = CalculatorGridSource != null ? GetCountLabel("Database Results", CalculatorGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Database Results", 0, 0);
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        finally
        {
            IsBusy = false;
            GridActionsNotify();
        }
    }

    private bool CanSearchByCurrentPhrase()
    {
        return CurrentPhrase != null && !string.IsNullOrWhiteSpace(CurrentPhrase.Content);
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


    [RelayCommand(CanExecute = nameof(CanCopy))]
    private void Copy()
    {
        StringBuilder stringBuilder = new();
        foreach (PhraseViewModel phrase in CalculatorGridSource.RowSelection.SelectedItems)
        {
            stringBuilder.AppendLine(phrase.Phrase.Content);
        }
        ClipboardMessage clipboardMessage = new() { Value = stringBuilder.ToString() };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopy()
    {
        return CalculatorGridSource?.RowSelection.SelectedItems != null && CalculatorGridSource.RowSelection.SelectedItems.Count > 0;
    }


    [RelayCommand(CanExecute = nameof(CanCopyCSV))]
    private void CopyCSV()
    {
        List<Phrase> phrases = [];
        foreach (PhraseViewModel phrase in CalculatorGridSource.RowSelection.SelectedItems)
        {
            phrases.Add(phrase.Phrase);
        }
        Exporter exporter = new(phrases, cancellationTokenSource.Token);
        ClipboardMessage clipboardMessage = new() { Value = exporter.Data };
        WeakReferenceMessenger.Default.Send(clipboardMessage);
    }

    private bool CanCopyCSV()
    {
        return CalculatorGridSource?.RowSelection.SelectedItems != null && CalculatorGridSource.RowSelection.SelectedItems.Count > 0;
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


    [RelayCommand(CanExecute = nameof(CanDeleteSelected))]
    private async Task DeleteSelectedAsync()
    {
        ConfirmationMessage confirmationMessage = new() { Message = "Are you sure you want to delete the selected phrase(s)?" };
        Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(confirmationMessage));
        while (!confirmationMessage.HasReceivedResponse) { await Task.Delay(500); }
        if (await confirmationMessage.Response != true) return;
        IsBusy = true;
        BusyMessage = "Deleting...";
        try
        {
            List<PhraseViewModel> selectedPhrases = CalculatorGridSource.RowSelection.SelectedItems.ToList();
            if (selectedPhrases != null && selectedPhrases.Count != 0)
            {
                long result = await Task.Run(async () =>
                {
                    return await phraseDataService.DeleteAsync(selectedPhrases.Select(selectedPhrase => selectedPhrase.Phrase).Select(phrase => phrase.Id).ToList());
                });
            }
            for (int i = CalculatorGridSource.RowSelection.SelectedItems.Count - 1; i >= 0; i--)
            {
                Phrases.Remove(CalculatorGridSource.RowSelection.SelectedItems[i]);
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
        }
        GridActionsNotify();
    }

    private bool CanDeleteSelected()
    {
        return CalculatorGridSource?.RowSelection.SelectedItems != null && CalculatorGridSource.RowSelection.SelectedItems.Count > 0;
    }
    #endregion

    #region Methods
    private void UpdateCurrentPhrase()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        List<GematriaMethod> methods = [];
        foreach (GematriaMethodViewModel gematriaMethodViewModel in GematriaMethodsViewModels)
        {
            methods.Add(gematriaMethodViewModel.GetModel());
        }
        CurrentPhrase = new PhraseViewModel(new Phrase(CurrentPhraseContent, methods));
    }

    private void UpdateGematriaValues()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        foreach (GematriaMethodViewModel gematriaMethodViewModel in GematriaMethodsViewModels)
        {
            gematriaMethodViewModel.Value = CurrentPhrase.Values[gematriaMethodViewModel.Name];
        }
    }

    private void GenerateResultsColumns()
    {
        if (GematriaMethodsViewModels == null || GematriaMethodsViewModels.Count == 0) return;

        CalculatorGridSource = new FlatTreeDataGridSource<PhraseViewModel>(Phrases);
        CalculatorGridSource.RowSelection!.SingleSelect = false;
        CalculatorGridSource.RowSelection.SelectionChanged += RowSelection_SelectionChanged;
        CalculatorGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Phrase",
            x => x.Content, options: new()
            {
                MinWidth = new GridLength(100),
                CanUserResizeColumn = true
            }));
        foreach (KeyValuePair<string, int> kvp in (new PhraseViewModel(new Phrase("Initialize", GetAllGematriaMethods())).Values))
        {
            CalculatorGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>(kvp.Key,
                p => p.Values[kvp.Key],
                options: new()
                {
                    MinWidth = new GridLength(85),
                    CanUserResizeColumn = true
                }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnAlphabet) == Constants.Settings.True)
        {
            CalculatorGridSource.Columns.Add(new TextColumn<PhraseViewModel, string>("Alphabet",
          x => x.Alphabet, options: new()
          {
              MinWidth = new GridLength(85),
              CanUserResizeColumn = true
          }));
        }
        if (settingDataService.Retrieve(Constants.Settings.ShowColumnOperationId) == Constants.Settings.True)
        {
            CalculatorGridSource.Columns.Add(new TextColumn<PhraseViewModel, int>("Op. Id",
          x => x.OperationId, options: new()
          {
              MinWidth = new GridLength(60),
              CanUserResizeColumn = true
          }));
        }
        GridActionsNotify();
    }

    private void RegenerateColumns(object recipient, RegenerateColumnsMessage message)
    {
        GenerateResultsColumns();
    }

    private void RowSelection_SelectionChanged(object sender, Avalonia.Controls.Selection.TreeSelectionModelSelectionChangedEventArgs<PhraseViewModel> e)
    {
        GridActionsNotify();
        ResultsGridCountLabel = CalculatorGridSource != null ? GetCountLabel("Database Results", CalculatorGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Database Results", 0, 0);
    }

    private void Phrases_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        ResultsGridCountLabel = CalculatorGridSource != null ? GetCountLabel("Database Results", CalculatorGridSource.RowSelection.Count, Phrases.Count) : GetCountLabel("Database Results", 0, 0);
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

    private void GridActionsNotify()
    {
        ClearAllResultsCommand.NotifyCanExecuteChanged();
        CopyCommand.NotifyCanExecuteChanged();
        CopyCSVCommand.NotifyCanExecuteChanged();
        ExportGridCommand.NotifyCanExecuteChanged();
        DeleteSelectedCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Properties
    private PhraseViewModel currentPhrase;
    public PhraseViewModel CurrentPhrase
    {
        get => currentPhrase;
        set => SetProperty(ref currentPhrase, value);
    }

    private string currentPhraseContent;
    public string CurrentPhraseContent
    {
        get => currentPhraseContent;
        set
        {
            SetProperty(ref currentPhraseContent, value.RemoveNewLines(" ").RemoveExtraSpaces());
            UpdateCurrentPhrase();
            UpdateGematriaValues();
            SaveCurrentPhraseCommand.NotifyCanExecuteChanged();
            SearchByCurrentPhraseCommand.NotifyCanExecuteChanged();
        }
    }

    private FlatTreeDataGridSource<PhraseViewModel> calculatorGridSource;
    public FlatTreeDataGridSource<PhraseViewModel> CalculatorGridSource
    {
        get => calculatorGridSource;
        set => SetProperty(ref calculatorGridSource, value);
    }

    private ObservableCollection<PhraseViewModel> phrases;
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

    private string resultsGridCountLabel = "Database Results";
    public string ResultsGridCountLabel
    {
        get => resultsGridCountLabel;
        set => SetProperty(ref resultsGridCountLabel, value);
    }
    #endregion
}