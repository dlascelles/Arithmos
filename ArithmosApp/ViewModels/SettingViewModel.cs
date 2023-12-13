/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using ArithmosApp.ViewModels.Services;
using ArithmosDataAccess;
using ArithmosModels;
using Avalonia;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels;

public partial class SettingViewModel : CommonViewModel
{
    public SettingViewModel() : base(new GematriaMethodDataService(), new PhraseDataService(), new OperationDataService(), new SettingDataService())
    {
        IsDarkMode = settingDataService.Retrieve(Constants.Settings.Theme) == Constants.Settings.ThemeDark;
        ShowColumnOperationId = settingDataService.Retrieve(Constants.Settings.ShowColumnOperationId) == Constants.Settings.True;
        ShowColumnAlphabet = settingDataService.Retrieve(Constants.Settings.ShowColumnAlphabet) == Constants.Settings.True;
    }

    [RelayCommand(CanExecute = nameof(CanRecalculatePhrases))]
    private async Task RecalculatePhrasesAsync()
    {
        IsBusy = true;
        BusyMessage = "Recalculating phrase values...";
        try
        {            
            int result = await Task.Run(async () => { return await phraseDataService.RecreatePhrasesAsync(); });
            NotificationMessage recalculationMessage = new("Recalculation completed.");
            WeakReferenceMessenger.Default.Send(recalculationMessage);
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

    private bool CanRecalculatePhrases()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanOptimizeDatabase))]
    private async Task OptimizeDatabaseAsync()
    {
        IsBusy = true;
        BusyMessage = "Optimizing database...";
        try
        {
           
            bool result = await Task.Run(Database.OptimizeDatabase);
            NotificationMessage recalculationMessage = new("Optimization is completed.");
            WeakReferenceMessenger.Default.Send(recalculationMessage);
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

    private bool CanOptimizeDatabase()
    {
        return true;
    }


    [RelayCommand(CanExecute = nameof(CanCancelOperation))]
    private void CancelOperation()
    {
        cancellationTokenSource.Cancel();
        BusyMessage = "Canceling operation...";
    }

    private bool CanCancelOperation()
    {
        return true;
    }

    private void ChangeTheme()
    {
        string theme = IsDarkMode ? Constants.Settings.ThemeDark : Constants.Settings.ThemeLight;
        Application.Current.RequestedThemeVariant = new Avalonia.Styling.ThemeVariant(theme, null);
        settingDataService.Upsert(Constants.Settings.Theme, IsDarkMode ? Constants.Settings.ThemeDark : Constants.Settings.ThemeLight);
    }

    private bool isDarkMode;
    public bool IsDarkMode
    {
        get => isDarkMode;
        set
        {
            SetProperty(ref isDarkMode, value);
            ChangeTheme();
        }
    }

    private bool showColumnOperationId;
    public bool ShowColumnOperationId
    {
        get => showColumnOperationId;
        set
        {
            SetProperty(ref showColumnOperationId, value);
            settingDataService.Upsert(Constants.Settings.ShowColumnOperationId, ShowColumnOperationId ? Constants.Settings.True : Constants.Settings.False);
            WeakReferenceMessenger.Default.Send<RegenerateColumnsMessage>();
        }
    }

    private bool showColumnAlphabet;
    public bool ShowColumnAlphabet
    {
        get => showColumnAlphabet;
        set
        {
            SetProperty(ref showColumnAlphabet, value);
            settingDataService.Upsert(Constants.Settings.ShowColumnAlphabet, ShowColumnAlphabet ? Constants.Settings.True : Constants.Settings.False);
            WeakReferenceMessenger.Default.Send<RegenerateColumnsMessage>();
        }
    }
}
