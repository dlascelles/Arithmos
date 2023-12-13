/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using ArithmosApp.ViewModels.Services;
using ArithmosModels;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace ArithmosApp.ViewModels;

public class CommonViewModel : ViewModelBase
{
    public CommonViewModel() : this(new GematriaMethodDataService(), new PhraseDataService(), new OperationDataService(), new SettingDataService()) { }
    public CommonViewModel(IGematriaMethodDataService gematriaMethodDataService, IPhraseDataService phraseDataService, IOperationDataService operationDataService, ISettingDataService settingDataService)
    {
        this.gematriaMethodDataService = gematriaMethodDataService;
        this.phraseDataService = phraseDataService;
        this.operationDataService = operationDataService;
        this.settingDataService = settingDataService;
        GematriaMethodsViewModels = [];
        try
        {
            foreach (GematriaMethod method in this.gematriaMethodDataService.RetrieveAll())
            {
                GematriaMethodsViewModels.Add(new GematriaMethodViewModel(method));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
    }

    public List<GematriaMethod> GetAllGematriaMethods()
    {
        List<GematriaMethod> methods = [];
        foreach (GematriaMethodViewModel methodViewModel in GematriaMethodsViewModels)
        {
            methods.Add(methodViewModel.GetModel());
        }
        return methods;
    }

    protected static string GetCountLabel(string prefix, int selections, int total)
    {
        if (total == 0) return prefix;

        string phraseLabel = total == 1 ? "phrase" : "phrases";
        return selections == 0 ? $"{prefix} ({total} {phraseLabel})" : $"{prefix} ({selections} of {total} {phraseLabel})";
    }

    protected readonly IGematriaMethodDataService gematriaMethodDataService;

    protected readonly IPhraseDataService phraseDataService;

    protected readonly IOperationDataService operationDataService;

    protected readonly ISettingDataService settingDataService;

    protected CancellationTokenSource cancellationTokenSource = new();

    private ObservableCollection<GematriaMethodViewModel> gematriaMethodsViewModels = [];
    public ObservableCollection<GematriaMethodViewModel> GematriaMethodsViewModels
    {
        get => gematriaMethodsViewModels;
        set { SetProperty(ref gematriaMethodsViewModels, value); }
    }

    private string busyMessage = "";
    public string BusyMessage
    {
        get => busyMessage;
        set { SetProperty(ref busyMessage, value); }
    }

    private bool isBusy = false;
    public bool IsBusy
    {
        get => isBusy;
        set { SetProperty(ref isBusy, value); }
    }
}