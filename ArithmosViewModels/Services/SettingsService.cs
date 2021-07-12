/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels.Messages;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace ArithmosViewModels.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        private bool showGematria = ArithmosSettings.Default.ShowGematria;
        public bool ShowGematria
        {
            get => showGematria;
            set => SetProperty(ref showGematria, value);
        }

        private bool showOrdinal = ArithmosSettings.Default.ShowOrdinal;
        public bool ShowOrdinal
        {
            get => showOrdinal;
            set => SetProperty(ref showOrdinal, value);
        }

        private bool showReduced = ArithmosSettings.Default.ShowReduced;
        public bool ShowReduced
        {
            get => showReduced;
            set => SetProperty(ref showReduced, value);
        }

        private bool showSumerian = ArithmosSettings.Default.ShowSumerian;
        public bool ShowSumerian
        {
            get => showSumerian;
            set => SetProperty(ref showSumerian, value);
        }

        private bool showPrimes = ArithmosSettings.Default.ShowPrimes;
        public bool ShowPrimes
        {
            get => showPrimes;
            set => SetProperty(ref showPrimes, value);
        }

        private bool showSquared = ArithmosSettings.Default.ShowSquared;
        public bool ShowSquared
        {
            get => showSquared;
            set => SetProperty(ref showSquared, value);
        }

        private bool showMisparGadol = ArithmosSettings.Default.ShowMisparGadol;
        public bool ShowMisparGadol
        {
            get => showMisparGadol;
            set => SetProperty(ref showMisparGadol, value);
        }

        private bool showMisparShemi = ArithmosSettings.Default.ShowMisparShemi;
        public bool ShowMisparShemi
        {
            get => showMisparShemi;
            set => SetProperty(ref showMisparShemi, value);
        }

        private bool showAlphabet = ArithmosSettings.Default.ShowAlphabet;
        public bool ShowAlphabet
        {
            get => showAlphabet;
            set => SetProperty(ref showAlphabet, value);
        }


        private bool selectGematria = ArithmosSettings.Default.SelectGematria;
        public bool SelectGematria
        {
            get => selectGematria;
            set => SetProperty(ref selectGematria, value);
        }

        private bool selectOrdinal = ArithmosSettings.Default.SelectOrdinal;
        public bool SelectOrdinal
        {
            get => selectOrdinal;
            set => SetProperty(ref selectOrdinal, value);
        }

        private bool selectReduced = ArithmosSettings.Default.SelectReduced;
        public bool SelectReduced
        {
            get => selectReduced;
            set => SetProperty(ref selectReduced, value);
        }

        private bool selectSumerian = ArithmosSettings.Default.SelectSumerian;
        public bool SelectSumerian
        {
            get => selectSumerian;
            set => SetProperty(ref selectSumerian, value);
        }

        private bool selectPrimes = ArithmosSettings.Default.SelectPrimes;
        public bool SelectPrimes
        {
            get => selectPrimes;
            set => SetProperty(ref selectPrimes, value);
        }

        private bool selectSquared = ArithmosSettings.Default.SelectSquared;
        public bool SelectSquared
        {
            get => selectSquared;
            set => SetProperty(ref selectSquared, value);
        }

        private bool selectMisparGadol = ArithmosSettings.Default.SelectMisparGadol;
        public bool SelectMisparGadol
        {
            get => selectMisparGadol;
            set => SetProperty(ref selectMisparGadol, value);
        }

        private bool selectMisparShemi = ArithmosSettings.Default.SelectMisparShemi;
        public bool SelectMisparShemi
        {
            get => selectMisparShemi;
            set => SetProperty(ref selectMisparShemi, value);
        }


        private bool selectEnglish = ArithmosSettings.Default.SelectEnglish;
        public bool SelectEnglish
        {
            get => selectEnglish;
            set => SetProperty(ref selectEnglish, value);
        }

        private bool selectHebrew = ArithmosSettings.Default.SelectHebrew;
        public bool SelectHebrew
        {
            get => selectHebrew;
            set => SetProperty(ref selectHebrew, value);
        }

        private bool selectGreek = ArithmosSettings.Default.SelectGreek;
        public bool SelectGreek
        {
            get => selectGreek;
            set => SetProperty(ref selectGreek, value);
        }

        private bool selectMixed = ArithmosSettings.Default.SelectMixed;
        public bool SelectMixed
        {
            get => selectMixed;
            set => SetProperty(ref selectMixed, value);
        }


        public void Load()
        {
            ShowGematria = ArithmosSettings.Default.ShowGematria;
            ShowOrdinal = ArithmosSettings.Default.ShowOrdinal;
            ShowReduced = ArithmosSettings.Default.ShowReduced;
            ShowSumerian = ArithmosSettings.Default.ShowSumerian;
            ShowPrimes = ArithmosSettings.Default.ShowPrimes;
            ShowSquared = ArithmosSettings.Default.ShowSquared;
            ShowMisparGadol = ArithmosSettings.Default.ShowMisparGadol;
            ShowMisparShemi = ArithmosSettings.Default.ShowMisparShemi;
            ShowAlphabet = ArithmosSettings.Default.ShowAlphabet;
            SelectGematria = ArithmosSettings.Default.SelectGematria;
            SelectOrdinal = ArithmosSettings.Default.SelectOrdinal;
            SelectReduced = ArithmosSettings.Default.SelectReduced;
            SelectSumerian = ArithmosSettings.Default.SelectSumerian;
            SelectPrimes = ArithmosSettings.Default.SelectPrimes;
            SelectSquared = ArithmosSettings.Default.SelectSquared;
            SelectMisparGadol = ArithmosSettings.Default.SelectMisparGadol;
            SelectMisparShemi = ArithmosSettings.Default.SelectMisparShemi;
            SelectEnglish = ArithmosSettings.Default.SelectEnglish;
            SelectHebrew = ArithmosSettings.Default.SelectHebrew;
            SelectGreek = ArithmosSettings.Default.SelectGreek;
            SelectMixed = ArithmosSettings.Default.SelectMixed;
        }

        public void Save()
        {
            ArithmosSettings.Default.ShowGematria = ShowGematria;
            ArithmosSettings.Default.ShowOrdinal = ShowOrdinal;
            ArithmosSettings.Default.ShowReduced = ShowReduced;
            ArithmosSettings.Default.ShowSumerian = ShowSumerian;
            ArithmosSettings.Default.ShowPrimes = ShowPrimes;
            ArithmosSettings.Default.ShowSquared = ShowSquared;
            ArithmosSettings.Default.ShowMisparGadol = ShowMisparGadol;
            ArithmosSettings.Default.ShowMisparShemi = ShowMisparShemi;
            ArithmosSettings.Default.ShowAlphabet = ShowAlphabet;
            ArithmosSettings.Default.SelectGematria = SelectGematria;
            ArithmosSettings.Default.SelectOrdinal = SelectOrdinal;
            ArithmosSettings.Default.SelectReduced = SelectReduced;
            ArithmosSettings.Default.SelectSumerian = SelectSumerian;
            ArithmosSettings.Default.SelectPrimes = SelectPrimes;
            ArithmosSettings.Default.SelectSquared = SelectSquared;
            ArithmosSettings.Default.SelectMisparGadol = SelectMisparGadol;
            ArithmosSettings.Default.SelectMisparShemi = SelectMisparShemi;
            ArithmosSettings.Default.SelectEnglish = SelectEnglish;
            ArithmosSettings.Default.SelectHebrew = SelectHebrew;
            ArithmosSettings.Default.SelectGreek = SelectGreek;
            ArithmosSettings.Default.SelectMixed = SelectMixed;
            ArithmosSettings.Default.Save();
            SettingsUpdatedMessage settingsUpdatedMessage = new("Settings successfully saved");
            WeakReferenceMessenger.Default.Send(settingsUpdatedMessage);
        }
    }
}