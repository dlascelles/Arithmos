/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosViewModels
{
    public class PhraseViewModel : ModelBase
    {
        public PhraseViewModel() { }

        public PhraseViewModel(Phrase phrase)
        {
            this.Phrase = phrase;
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.SetField(ref this.isSelected, value); }
        }

        private bool isMarked = false;
        public bool IsMarked
        {
            get { return this.isMarked; }
            set { this.SetField(ref this.isMarked, value); }
        }

        private string phraseText = "";
        public string PhraseText
        {
            get { return this.phraseText; }
            set
            {
                if (this.SetField(ref this.phraseText, value))
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
                this.SetField(ref this.phrase, value);
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
            get { return this.gematria; }
            set { this.SetField(ref this.gematria, value); }
        }

        private int ordinal = 0;
        public int Ordinal
        {
            get { return this.ordinal; }
            set { this.SetField(ref this.ordinal, value); }
        }

        private int reduced = 0;
        public int Reduced
        {
            get { return this.reduced; }
            set { this.SetField(ref this.reduced, value); }
        }

        private int sumerian = 0;
        public int Sumerian
        {
            get { return this.sumerian; }
            set { this.SetField(ref this.sumerian, value); }
        }

        private int primes = 0;
        public int Primes
        {
            get { return this.primes; }
            set { this.SetField(ref this.primes, value); }
        }

        private int squared = 0;
        public int Squared
        {
            get { return this.squared; }
            set { this.SetField(ref this.squared, value); }
        }

        private int misparGadol = 0;
        public int MisparGadol
        {
            get { return this.misparGadol; }
            set { this.SetField(ref this.misparGadol, value); }
        }

        private int misparShemi = 0;
        public int MisparShemi
        {
            get { return this.misparShemi; }
            set { this.SetField(ref this.misparShemi, value); }
        }
    }
}