/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
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
            get => this.showGematria;
            set => SetProperty(ref this.showGematria, value);
        }

        private bool showOrdinal = ArithmosSettings.Default.ShowOrdinal;
        public bool ShowOrdinal
        {
            get => this.showOrdinal;
            set => SetProperty(ref this.showOrdinal, value);
        }

        private bool showReduced = ArithmosSettings.Default.ShowReduced;
        public bool ShowReduced
        {
            get => this.showReduced;
            set => SetProperty(ref this.showReduced, value);
        }

        private bool showSumerian = ArithmosSettings.Default.ShowSumerian;
        public bool ShowSumerian
        {
            get => this.showSumerian;
            set => SetProperty(ref this.showSumerian, value);
        }

        private bool showPrimes = ArithmosSettings.Default.ShowPrimes;
        public bool ShowPrimes
        {
            get => this.showPrimes;
            set => SetProperty(ref this.showPrimes, value);
        }

        private bool showSquared = ArithmosSettings.Default.ShowSquared;
        public bool ShowSquared
        {
            get => this.showSquared;
            set => SetProperty(ref this.showSquared, value);
        }

        private bool showMisparGadol = ArithmosSettings.Default.ShowMisparGadol;
        public bool ShowMisparGadol
        {
            get => this.showMisparGadol;
            set => SetProperty(ref this.showMisparGadol, value);
        }

        private bool showMisparShemi = ArithmosSettings.Default.ShowMisparShemi;
        public bool ShowMisparShemi
        {
            get => this.showMisparShemi;
            set => SetProperty(ref this.showMisparShemi, value);
        }

        private bool showAlphabet = ArithmosSettings.Default.ShowAlphabet;
        public bool ShowAlphabet
        {
            get => this.showAlphabet;
            set => SetProperty(ref this.showAlphabet, value);
        }


        private bool selectGematria = ArithmosSettings.Default.SelectGematria;
        public bool SelectGematria
        {
            get => this.selectGematria;
            set => SetProperty(ref this.selectGematria, value);
        }

        private bool selectOrdinal = ArithmosSettings.Default.SelectOrdinal;
        public bool SelectOrdinal
        {
            get => this.selectOrdinal;
            set => SetProperty(ref this.selectOrdinal, value);
        }

        private bool selectReduced = ArithmosSettings.Default.SelectReduced;
        public bool SelectReduced
        {
            get => this.selectReduced;
            set => SetProperty(ref this.selectReduced, value);
        }

        private bool selectSumerian = ArithmosSettings.Default.SelectSumerian;
        public bool SelectSumerian
        {
            get => this.selectSumerian;
            set => SetProperty(ref this.selectSumerian, value);
        }

        private bool selectPrimes = ArithmosSettings.Default.SelectPrimes;
        public bool SelectPrimes
        {
            get => this.selectPrimes;
            set => SetProperty(ref this.selectPrimes, value);
        }

        private bool selectSquared = ArithmosSettings.Default.SelectSquared;
        public bool SelectSquared
        {
            get => this.selectSquared;
            set => SetProperty(ref this.selectSquared, value);
        }

        private bool selectMisparGadol = ArithmosSettings.Default.SelectMisparGadol;
        public bool SelectMisparGadol
        {
            get => this.selectMisparGadol;
            set => SetProperty(ref this.selectMisparGadol, value);
        }

        private bool selectMisparShemi = ArithmosSettings.Default.SelectMisparShemi;
        public bool SelectMisparShemi
        {
            get => this.selectMisparShemi;
            set => SetProperty(ref this.selectMisparShemi, value);
        }


        private bool selectEnglish = ArithmosSettings.Default.SelectEnglish;
        public bool SelectEnglish
        {
            get => this.selectEnglish;
            set => SetProperty(ref this.selectEnglish, value);
        }

        private bool selectHebrew = ArithmosSettings.Default.SelectHebrew;
        public bool SelectHebrew
        {
            get => this.selectHebrew;
            set => SetProperty(ref this.selectHebrew, value);
        }

        private bool selectGreek = ArithmosSettings.Default.SelectGreek;
        public bool SelectGreek
        {
            get => this.selectGreek;
            set => SetProperty(ref this.selectGreek, value);
        }

        private bool selectMixed = ArithmosSettings.Default.SelectMixed;
        public bool SelectMixed
        {
            get => this.selectMixed;
            set => SetProperty(ref this.selectMixed, value);
        }


        public void Load()
        {
            this.ShowGematria = ArithmosSettings.Default.ShowGematria;
            this.ShowOrdinal = ArithmosSettings.Default.ShowOrdinal;
            this.ShowReduced = ArithmosSettings.Default.ShowReduced;
            this.ShowSumerian = ArithmosSettings.Default.ShowSumerian;
            this.ShowPrimes = ArithmosSettings.Default.ShowPrimes;
            this.ShowSquared = ArithmosSettings.Default.ShowSquared;
            this.ShowMisparGadol = ArithmosSettings.Default.ShowMisparGadol;
            this.ShowMisparShemi = ArithmosSettings.Default.ShowMisparShemi;
            this.ShowAlphabet = ArithmosSettings.Default.ShowAlphabet;
            this.SelectGematria = ArithmosSettings.Default.SelectGematria;
            this.SelectOrdinal = ArithmosSettings.Default.SelectOrdinal;
            this.SelectReduced = ArithmosSettings.Default.SelectReduced;
            this.SelectSumerian = ArithmosSettings.Default.SelectSumerian;
            this.SelectPrimes = ArithmosSettings.Default.SelectPrimes;
            this.SelectSquared = ArithmosSettings.Default.SelectSquared;
            this.SelectMisparGadol = ArithmosSettings.Default.SelectMisparGadol;
            this.SelectMisparShemi = ArithmosSettings.Default.SelectMisparShemi;
            this.SelectEnglish = ArithmosSettings.Default.SelectEnglish;
            this.SelectHebrew = ArithmosSettings.Default.SelectHebrew;
            this.SelectGreek = ArithmosSettings.Default.SelectGreek;
            this.SelectMixed = ArithmosSettings.Default.SelectMixed;
        }

        public void Save()
        {
            ArithmosSettings.Default.ShowGematria = this.ShowGematria;
            ArithmosSettings.Default.ShowOrdinal = this.ShowOrdinal;
            ArithmosSettings.Default.ShowReduced = this.ShowReduced;
            ArithmosSettings.Default.ShowSumerian = this.ShowSumerian;
            ArithmosSettings.Default.ShowPrimes = this.ShowPrimes;
            ArithmosSettings.Default.ShowSquared = this.ShowSquared;
            ArithmosSettings.Default.ShowMisparGadol = this.ShowMisparGadol;
            ArithmosSettings.Default.ShowMisparShemi = this.ShowMisparShemi;
            ArithmosSettings.Default.ShowAlphabet = this.ShowAlphabet;
            ArithmosSettings.Default.SelectGematria = this.SelectGematria;
            ArithmosSettings.Default.SelectOrdinal = this.SelectOrdinal;
            ArithmosSettings.Default.SelectReduced = this.SelectReduced;
            ArithmosSettings.Default.SelectSumerian = this.SelectSumerian;
            ArithmosSettings.Default.SelectPrimes = this.SelectPrimes;
            ArithmosSettings.Default.SelectSquared = this.SelectSquared;
            ArithmosSettings.Default.SelectMisparGadol = this.SelectMisparGadol;
            ArithmosSettings.Default.SelectMisparShemi = this.SelectMisparShemi;
            ArithmosSettings.Default.SelectEnglish = this.SelectEnglish;
            ArithmosSettings.Default.SelectHebrew = this.SelectHebrew;
            ArithmosSettings.Default.SelectGreek = this.SelectGreek;
            ArithmosSettings.Default.SelectMixed = this.SelectMixed;
            ArithmosSettings.Default.Save();
            SettingsUpdatedMessage settingsUpdatedMessage = new SettingsUpdatedMessage("Settings successfully saved");
            WeakReferenceMessenger.Default.Send(settingsUpdatedMessage);
        }
    }
}