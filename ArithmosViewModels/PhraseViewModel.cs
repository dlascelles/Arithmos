/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ArithmosViewModels
{
    public class PhraseViewModel : ObservableObject
    {
        public PhraseViewModel() { }

        public PhraseViewModel(Phrase phrase)
        {
            this.Phrase = phrase;
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => this.isSelected;
            set => SetProperty(ref this.isSelected, value);
        }

        private bool isMarked = false;
        public bool IsMarked
        {
            get => this.isMarked;
            set => SetProperty(ref this.isMarked, value);
        }

        private string phraseText = "";
        public string PhraseText
        {
            get => this.phraseText;
            set
            {
                if (SetProperty(ref this.phraseText, value))
                {
                    this.Phrase = new Phrase(this.phraseText);
                }
            }
        }

        private Phrase phrase;
        public Phrase Phrase
        {
            get { return this.phrase; }
            set
            {
                SetProperty(ref this.phrase, value);
                this.Gematria = this.Phrase.Values[CalculationMethod.Gematria];
                this.Ordinal = this.Phrase.Values[CalculationMethod.Ordinal];
                this.Reduced = this.Phrase.Values[CalculationMethod.Reduced];
                this.Sumerian = this.Phrase.Values[CalculationMethod.Sumerian];
                this.Primes = this.Phrase.Values[CalculationMethod.Primes];
                this.Squared = this.Phrase.Values[CalculationMethod.Squared];
                this.MisparGadol = this.Phrase.Values[CalculationMethod.MisparGadol];
                this.MisparShemi = this.Phrase.Values[CalculationMethod.MisparShemi];
            }
        }

        private int gematria = 0;
        public int Gematria
        {
            get => this.gematria;
            set => SetProperty(ref this.gematria, value);
        }

        private int ordinal = 0;
        public int Ordinal
        {
            get => this.ordinal;
            set => SetProperty(ref this.ordinal, value);
        }

        private int reduced = 0;
        public int Reduced
        {
            get => this.reduced;
            set => SetProperty(ref this.reduced, value);
        }

        private int sumerian = 0;
        public int Sumerian
        {
            get => this.sumerian;
            set => SetProperty(ref this.sumerian, value);
        }

        private int primes = 0;
        public int Primes
        {
            get => this.primes;
            set => SetProperty(ref this.primes, value);
        }

        private int squared = 0;
        public int Squared
        {
            get => this.squared;
            set => SetProperty(ref this.squared, value);
        }

        private int misparGadol = 0;
        public int MisparGadol
        {
            get => this.misparGadol;
            set => SetProperty(ref this.misparGadol, value);
        }

        private int misparShemi = 0;
        public int MisparShemi
        {
            get => this.misparShemi;
            set => SetProperty(ref this.misparShemi, value);
        }
    }
}