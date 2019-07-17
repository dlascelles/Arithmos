/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace ArithmosViewModels.Services
{
    public class SettingsService : ModelBase, ISettingsService
    {
        private bool showGematria = ArithmosSettings.Default.ShowGematria;
        public bool ShowGematria
        {
            get { return this.showGematria; }
            set { this.SetField(ref this.showGematria, value); }
        }

        private bool showOrdinal = ArithmosSettings.Default.ShowOrdinal;
        public bool ShowOrdinal
        {
            get { return this.showOrdinal; }
            set { this.SetField(ref this.showOrdinal, value); }
        }

        private bool showReduced = ArithmosSettings.Default.ShowReduced;
        public bool ShowReduced
        {
            get { return this.showReduced; }
            set { this.SetField(ref this.showReduced, value); }
        }

        private bool showSumerian = ArithmosSettings.Default.ShowSumerian;
        public bool ShowSumerian
        {
            get { return this.showSumerian; }
            set { this.SetField(ref this.showSumerian, value); }
        }

        private bool showPrimes = ArithmosSettings.Default.ShowPrimes;
        public bool ShowPrimes
        {
            get { return this.showPrimes; }
            set { this.SetField(ref this.showPrimes, value); }
        }

        private bool showSquared = ArithmosSettings.Default.ShowSquared;
        public bool ShowSquared
        {
            get { return this.showSquared; }
            set { this.SetField(ref this.showSquared, value); }
        }

        private bool showMisparGadol = ArithmosSettings.Default.ShowMisparGadol;
        public bool ShowMisparGadol
        {
            get { return this.showMisparGadol; }
            set { this.SetField(ref this.showMisparGadol, value); }
        }

        private bool showMisparShemi = ArithmosSettings.Default.ShowMisparShemi;
        public bool ShowMisparShemi
        {
            get { return this.showMisparGadol; }
            set { this.SetField(ref this.showMisparShemi, value); }
        }

        private bool showAlphabet = ArithmosSettings.Default.ShowAlphabet;
        public bool ShowAlphabet
        {
            get { return this.showAlphabet; }
            set { this.SetField(ref this.showAlphabet, value); }
        }


        private bool selectGematria = ArithmosSettings.Default.SelectGematria;
        public bool SelectGematria
        {
            get { return this.selectGematria; }
            set { this.SetField(ref this.selectGematria, value); }
        }

        private bool selectOrdinal = ArithmosSettings.Default.SelectOrdinal;
        public bool SelectOrdinal
        {
            get { return this.selectOrdinal; }
            set { this.SetField(ref this.selectOrdinal, value); }
        }

        private bool selectReduced = ArithmosSettings.Default.SelectReduced;
        public bool SelectReduced
        {
            get { return this.selectReduced; }
            set { this.SetField(ref this.selectReduced, value); }
        }

        private bool selectSumerian = ArithmosSettings.Default.SelectSumerian;
        public bool SelectSumerian
        {
            get { return this.selectSumerian; }
            set { this.SetField(ref this.selectSumerian, value); }
        }

        private bool selectPrimes = ArithmosSettings.Default.SelectPrimes;
        public bool SelectPrimes
        {
            get { return this.selectPrimes; }
            set { this.SetField(ref this.selectPrimes, value); }
        }

        private bool selectSquared = ArithmosSettings.Default.SelectSquared;
        public bool SelectSquared
        {
            get { return this.selectSquared; }
            set { this.SetField(ref this.selectSquared, value); }
        }

        private bool selectMisparGadol = ArithmosSettings.Default.SelectMisparGadol;
        public bool SelectMisparGadol
        {
            get { return this.selectMisparGadol; }
            set { this.SetField(ref this.selectMisparGadol, value); }
        }

        private bool selectMisparShemi = ArithmosSettings.Default.SelectMisparShemi;
        public bool SelectMisparShemi
        {
            get { return this.selectMisparShemi; }
            set { this.SetField(ref this.selectMisparShemi, value); }
        }


        private bool selectEnglish = ArithmosSettings.Default.SelectEnglish;
        public bool SelectEnglish
        {
            get { return this.selectEnglish; }
            set { this.SetField(ref this.selectEnglish, value); }
        }

        private bool selectHebrew = ArithmosSettings.Default.SelectHebrew;
        public bool SelectHebrew
        {
            get { return this.selectHebrew; }
            set { this.SetField(ref this.selectHebrew, value); }
        }

        private bool selectGreek = ArithmosSettings.Default.SelectGreek;
        public bool SelectGreek
        {
            get { return this.selectGreek; }
            set { this.SetField(ref this.selectGreek, value); }
        }

        private bool selectMixed = ArithmosSettings.Default.SelectMixed;
        public bool SelectMixed
        {
            get { return this.selectMixed; }
            set { this.SetField(ref this.selectMixed, value); }
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
            SettingsUpdatedMessage settingsUpdatedMessage = new SettingsUpdatedMessage(this, "Settings successfully saved");
            Messenger.Default.Send(settingsUpdatedMessage);
        }
    }
}