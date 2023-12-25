/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels.Messages;
using ArithmosApp.ViewModels.Services;
using ArithmosModels;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels;

public partial class CipherViewModel : ViewModelBase
{
    public CipherViewModel() : this(new GematriaMethodDataService()) { }
    public CipherViewModel(IGematriaMethodDataService gematriaMethodDataService)
    {
        WeakReferenceMessenger.Default.Register<ReloadGematriaMethodsMessage>(this, ReloadGematriaMethods);
        this.gematriaMethodDataService = gematriaMethodDataService;
        GematriaMethodsViewModels = [];
        foreach (GematriaMethod method in this.gematriaMethodDataService.RetrieveAll())
        {
            GematriaMethodsViewModels.Add(new GematriaMethodViewModel(method));
        }
    }

    #region Commands
    [RelayCommand(CanExecute = nameof(CanClearCipherPairs))]
    private void ClearCipherPairs()
    {
        GematriaCipher?.Clear();
        CipherEditControlsNotify();
    }

    private bool CanClearCipherPairs()
    {
        return GematriaCipher != null && GematriaCipher.Count != 0;
    }


    [RelayCommand(CanExecute = nameof(CanAddCipher))]
    private void AddCipher()
    {
        GematriaCipher ??= [];
        GematriaCipher.Clear();
        IsEditMode = true;
        ResetSelectedGematriaMethod();
    }

    private bool CanAddCipher()
    {
        return !IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanEditCipher))]
    private void EditCipher()
    {
        IsEditMode = true;
        UpdateCipher(SelectedGematriaMethod.Cipher);
        CipherEditControlsNotify();
    }

    private bool CanEditCipher()
    {
        return SelectedGematriaMethod != null && !IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanMoveUpCipher))]
    private void MoveUpCipher()
    {
        if (SelectedGematriaMethod == null) return;

        int index = GematriaMethodsViewModels.IndexOf(SelectedGematriaMethod);
        if (index <= 0 || index >= GematriaMethodsViewModels.Count) return;

        (GematriaMethodsViewModels[index - 1].Sort, GematriaMethodsViewModels[index].Sort) = (GematriaMethodsViewModels[index].Sort, GematriaMethodsViewModels[index - 1].Sort);
        try
        {
            gematriaMethodDataService.Update(GematriaMethodsViewModels[index].GetModel());
            gematriaMethodDataService.Update(GematriaMethodsViewModels[index - 1].GetModel());
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        WeakReferenceMessenger.Default.Send<ReloadGematriaMethodsMessage>();
        SelectedGematriaMethod = GematriaMethodsViewModels[index - 1];
    }

    private bool CanMoveUpCipher()
    {
        return SelectedGematriaMethod != null && !IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanMoveDownCipher))]
    private void MoveDownCipher()
    {
        if (SelectedGematriaMethod == null) return;

        int index = GematriaMethodsViewModels.IndexOf(SelectedGematriaMethod);
        if (index < 0 || index >= GematriaMethodsViewModels.Count - 1) return;

        (GematriaMethodsViewModels[index + 1].Sort, GematriaMethodsViewModels[index].Sort) = (GematriaMethodsViewModels[index].Sort, GematriaMethodsViewModels[index + 1].Sort);
        try
        {
            gematriaMethodDataService.Update(GematriaMethodsViewModels[index].GetModel());
            gematriaMethodDataService.Update(GematriaMethodsViewModels[index + 1].GetModel());
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        WeakReferenceMessenger.Default.Send<ReloadGematriaMethodsMessage>();
        SelectedGematriaMethod = GematriaMethodsViewModels[index + 1];
    }

    private bool CanMoveDownCipher()
    {
        return SelectedGematriaMethod != null && !IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanDeleteCipher))]
    private async Task DeleteCipherAsync()
    {
        if (SelectedGematriaMethod == null) return;

        ConfirmationMessage confirmationMessage = new() { Message = "Are you sure you want to delete the selected gematria method?" };
        Dispatcher.UIThread.Post(() => WeakReferenceMessenger.Default.Send(confirmationMessage));
        while (!confirmationMessage.HasReceivedResponse) { await Task.Delay(500); }
        if (await confirmationMessage.Response != true) return;
        try
        {
            gematriaMethodDataService.Delete(SelectedGematriaMethod.Id);
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        WeakReferenceMessenger.Default.Send<ReloadGematriaMethodsMessage>();
        IsEditMode = false;
        ResetSelectedGematriaMethod();
        GematriaCipher.Clear();
    }

    private bool CanDeleteCipher()
    {
        return SelectedGematriaMethod != null && !IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanAddPair))]
    private void AddPair()
    {
        GematriaCipher.Add(new ValuePairViewModel());
        CipherEditControlsNotify();
    }

    private bool CanAddPair()
    {
        return IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanRemoveCipherPair))]
    private void RemoveCipherPair(object value)
    {
        if (!GematriaCipher.Contains((ValuePairViewModel)value)) return;

        GematriaCipher.Remove((ValuePairViewModel)value);
        ClearCipherPairsCommand.NotifyCanExecuteChanged();
    }

    private bool CanRemoveCipherPair(object value)
    {
        return IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanAddGroup))]
    private void AddGroup()
    {
        if (SelectedCipherGroup == null || string.IsNullOrWhiteSpace(SelectedCipherGroup)) return;

        switch (SelectedCipherGroup)
        {
            case Constants.Ciphers.EnglishAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.English.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishStandardName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishStandard)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishOrdinalName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishOrdinal)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishReversedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishReversed)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishReducedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishReduced)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishSumerianName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishSumerian)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishPrimesName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishPrimes)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishFibonacciName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishFibonacci)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishTetrahedralName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishTetrahedral)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.EnglishTriangularName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.EnglishTriangular)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.Greek.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekStandardName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekStandard)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekOrdinalName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekOrdinal)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekReversedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekReversed)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekReducedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekReduced)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekPrimesName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekPrimes)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekFibonacciName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekFibonacci)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekTetrahedralName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekTetrahedral)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.GreekTriangularName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.GreekTriangular)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.Hebrew.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewStandardName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewStandard)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewOrdinalName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewOrdinal)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewReversedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewReversed)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewReducedName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewReduced)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewPrimesName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewPrimes)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewFibonacciName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewFibonacci)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewTetrahedralName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewTetrahedral)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.HebrewTriangularName:
                {
                    GetPairsFromCipher(new Cipher(Constants.Ciphers.HebrewTriangular)).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.ArabicAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.Arabic.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.CyrillicAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.Cyrillic.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
            case Constants.Ciphers.CopticAlphabetName:
                {
                    GetPairsFromAlphabet(Constants.Alphabets.Coptic.ToArray()).ForEach(v => GematriaCipher.Add(v));
                    break;
                }
        }
        CipherEditControlsNotify();
        SelectedCipherGroup = "";
    }

    private bool CanAddGroup()
    {
        return IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanCancelEdit))]
    private void CancelEdit()
    {
        IsEditMode = false;
        ResetSelectedGematriaMethod();
        GematriaCipher.Clear();
    }

    private bool CanCancelEdit()
    {
        return IsEditMode;
    }


    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        if (!IsCipherValid(out string message))
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
            return;
        }
        GematriaMethod gematriaMethod = new(GetCipher(GematriaCipher)) { Name = SelectedGematriaMethodName, AddsTotalNumberOfCharacters = SelectedGematriaMethodAddsCharacters, AddsTotalNumberOfWords = SelectedGematriaMethodAddsWords };
        try
        {
            if (SelectedGematriaMethod != null)
            {
                gematriaMethod.Id = SelectedGematriaMethod.Id;
                gematriaMethod.Sort = SelectedGematriaMethod.Sort;
                gematriaMethodDataService.Update(gematriaMethod);
            }
            else
            {
                gematriaMethod.Sort = GematriaMethodsViewModels != null && GematriaMethodsViewModels.Count > 0 ? GematriaMethodsViewModels.Max(g => g.Sort) + 1 : 1;
                gematriaMethodDataService.Create(gematriaMethod);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage errorMessage = new() { Message = ex.Message };
            WeakReferenceMessenger.Default.Send(errorMessage);
        }
        WeakReferenceMessenger.Default.Send<ReloadGematriaMethodsMessage>();
        IsEditMode = false;
        ResetSelectedGematriaMethod();
        GematriaCipher.Clear();
    }

    private bool CanSave()
    {
        return IsEditMode;
    }
    #endregion

    #region Methods
    private bool IsCipherValid(out string message)
    {
        message = string.Empty;
        if (GematriaCipher == null || GematriaCipher.Count == 0)
        {
            message = "The cipher cannot be empty.";
        }
        else if (!GetCipher(GematriaCipher).IsValid())
        {
            message = "The cipher does not seem to be valid.";
        }
        else if (string.IsNullOrWhiteSpace(SelectedGematriaMethodName))
        {
            message = "The name of the method cannot be empty.";
        }
        else if (SelectedGematriaMethod == null && GematriaMethodsViewModels.SingleOrDefault(g => g.Name == SelectedGematriaMethodName) != null)
        {
            message = "There is already another method with the same name.";
        }
        else if (SelectedGematriaMethod != null && GematriaMethodsViewModels.SingleOrDefault(g => g.Name == SelectedGematriaMethodName && g.Id != SelectedGematriaMethod.Id) != null)
        {
            message = "There is already another method with the same name.";
        }
        return string.IsNullOrWhiteSpace(message);
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
    }

    private void UpdateCipher(Cipher cipher)
    {
        if (!cipher.IsValid()) return;

        GematriaCipher.Clear();
        foreach (ValuePairViewModel valuePairViewModel in GetPairsFromCipher(cipher))
        {
            GematriaCipher.Add(valuePairViewModel);
        }
    }

    private void ResetSelectedGematriaMethod()
    {
        SelectedGematriaMethodName = "";
        SelectedGematriaMethodAddsCharacters = false;
        SelectedGematriaMethodAddsWords = false;
        SelectedGematriaMethod = null;
    }

    private static Cipher GetCipher(ObservableCollection<ValuePairViewModel> valuePairViewModels)
    {
        StringBuilder stringBuilder = new();
        foreach (ValuePairViewModel valuePairViewModel in valuePairViewModels)
        {
            stringBuilder.Append(valuePairViewModel.Character);
            stringBuilder.Append('=');
            stringBuilder.Append(valuePairViewModel.Value);
            stringBuilder.Append(',');
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        Cipher cipher = new(stringBuilder.ToString());
        return cipher;
    }

    private static List<ValuePairViewModel> GetPairsFromCipher(Cipher cipher)
    {
        List<ValuePairViewModel> valuePairViewModels = [];
        string[] splittedPairs = cipher.Body.Split(cipher.PairSeparator);
        foreach (string pair in splittedPairs)
        {
            string[] splittedValue = pair.Split(cipher.ValueSeparator);
            ValuePairViewModel valuePairViewModel = new() { Character = splittedValue[0], Value = Convert.ToInt32(splittedValue[1]) };
            valuePairViewModels.Add(valuePairViewModel);
        }
        return valuePairViewModels;
    }

    private static List<ValuePairViewModel> GetPairsFromAlphabet(char[] alphabet)
    {
        List<ValuePairViewModel> valuePairViewModels = [];
        foreach (char character in alphabet)
        {
            ValuePairViewModel valuePairViewModel = new() { Character = character.ToString(), Value = 0 };
            valuePairViewModels.Add(valuePairViewModel);
        }
        return valuePairViewModels;
    }

    private void CipherEditControlsNotify()
    {
        SaveCommand.NotifyCanExecuteChanged();
        CancelEditCommand.NotifyCanExecuteChanged();
        AddPairCommand.NotifyCanExecuteChanged();
        AddGroupCommand.NotifyCanExecuteChanged();
        ClearCipherPairsCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Properties
    private readonly IGematriaMethodDataService gematriaMethodDataService;

    private ObservableCollection<GematriaMethodViewModel> gematriaMethodsViewModels = [];
    public ObservableCollection<GematriaMethodViewModel> GematriaMethodsViewModels
    {
        get => gematriaMethodsViewModels;
        set => SetProperty(ref gematriaMethodsViewModels, value);
    }

    private GematriaMethodViewModel selectedGematriaMethod;
    public GematriaMethodViewModel SelectedGematriaMethod
    {
        get => selectedGematriaMethod;
        set
        {
            SetProperty(ref selectedGematriaMethod, value);
            EditCipherCommand.NotifyCanExecuteChanged();
            DeleteCipherCommand.NotifyCanExecuteChanged();
            MoveUpCipherCommand.NotifyCanExecuteChanged();
            MoveDownCipherCommand.NotifyCanExecuteChanged();
            if (selectedGematriaMethod != null)
            {
                GematriaCipher.Clear();
                SelectedGematriaMethodName = SelectedGematriaMethod.Name;
                SelectedGematriaMethodAddsCharacters = SelectedGematriaMethod.AddsTotalNumberOfCharacters;
                SelectedGematriaMethodAddsWords = SelectedGematriaMethod.AddsTotalNumberOfWords;
            }
        }
    }

    private string selectedGematriaMethodName;
    public string SelectedGematriaMethodName
    {
        get => selectedGematriaMethodName;
        set => SetProperty(ref selectedGematriaMethodName, value);
    }

    private bool selectedGematriaMethodAddsCharacters;
    public bool SelectedGematriaMethodAddsCharacters
    {
        get => selectedGematriaMethodAddsCharacters;
        set => SetProperty(ref selectedGematriaMethodAddsCharacters, value);
    }

    private bool selectedGematriaMethodAddsWords;
    public bool SelectedGematriaMethodAddsWords
    {
        get => selectedGematriaMethodAddsWords;
        set => SetProperty(ref selectedGematriaMethodAddsWords, value);
    }

    private string selectedCipherGroup = "";
    public string SelectedCipherGroup
    {
        get => selectedCipherGroup;
        set => SetProperty(ref selectedCipherGroup, value);
    }

    private ObservableCollection<string> cipherGroups = ["", Constants.Ciphers.EnglishAlphabetName, Constants.Ciphers.EnglishStandardName, Constants.Ciphers.EnglishOrdinalName, Constants.Ciphers.EnglishReversedName, Constants.Ciphers.EnglishReducedName, Constants.Ciphers.EnglishSumerianName, Constants.Ciphers.EnglishPrimesName, Constants.Ciphers.EnglishFibonacciName, Constants.Ciphers.EnglishTetrahedralName, Constants.Ciphers.EnglishTriangularName, Constants.Ciphers.GreekAlphabetName, Constants.Ciphers.GreekStandardName, Constants.Ciphers.GreekOrdinalName, Constants.Ciphers.GreekReversedName, Constants.Ciphers.GreekReducedName, Constants.Ciphers.GreekPrimesName, Constants.Ciphers.GreekFibonacciName, Constants.Ciphers.GreekTetrahedralName, Constants.Ciphers.GreekTriangularName, Constants.Ciphers.HebrewAlphabetName, Constants.Ciphers.HebrewStandardName, Constants.Ciphers.HebrewOrdinalName, Constants.Ciphers.HebrewReversedName, Constants.Ciphers.HebrewReducedName, Constants.Ciphers.HebrewPrimesName, Constants.Ciphers.HebrewFibonacciName, Constants.Ciphers.HebrewTetrahedralName, Constants.Ciphers.HebrewTriangularName, Constants.Ciphers.ArabicAlphabetName, Constants.Ciphers.CyrillicAlphabetName, Constants.Ciphers.CopticAlphabetName];
    public ObservableCollection<string> CipherGroups
    {
        get => cipherGroups;
        set { SetProperty(ref cipherGroups, value); }
    }

    private bool isEditMode = false;
    public bool IsEditMode
    {
        get => isEditMode;
        set
        {
            SetProperty(ref isEditMode, value);
            CipherEditControlsNotify();
        }
    }

    public ObservableCollection<ValuePairViewModel> GematriaCipher { get; set; } = [];
    #endregion
}
