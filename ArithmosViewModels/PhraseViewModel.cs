/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
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
            Phrase = phrase;
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private bool isMarked = false;
        public bool IsMarked
        {
            get => isMarked;
            set => SetProperty(ref isMarked, value);
        }

        private string phraseText = "";
        public string PhraseText
        {
            get => phraseText;
            set
            {
                if (SetProperty(ref phraseText, value))
                {
                    Phrase = new Phrase(phraseText);
                }
            }
        }

        private Phrase phrase;
        public Phrase Phrase
        {
            get { return phrase; }
            set
            {
                SetProperty(ref phrase, value);
                Gematria = Phrase.Values[CalculationMethod.Gematria];
                Ordinal = Phrase.Values[CalculationMethod.Ordinal];
                Reduced = Phrase.Values[CalculationMethod.Reduced];
                Sumerian = Phrase.Values[CalculationMethod.Sumerian];
                Primes = Phrase.Values[CalculationMethod.Primes];
                Squared = Phrase.Values[CalculationMethod.Squared];
                MisparGadol = Phrase.Values[CalculationMethod.MisparGadol];
                MisparShemi = Phrase.Values[CalculationMethod.MisparShemi];
            }
        }

        private int gematria = 0;
        public int Gematria
        {
            get => gematria;
            set => SetProperty(ref gematria, value);
        }

        private int ordinal = 0;
        public int Ordinal
        {
            get => ordinal;
            set => SetProperty(ref ordinal, value);
        }

        private int reduced = 0;
        public int Reduced
        {
            get => reduced;
            set => SetProperty(ref reduced, value);
        }

        private int sumerian = 0;
        public int Sumerian
        {
            get => sumerian;
            set => SetProperty(ref sumerian, value);
        }

        private int primes = 0;
        public int Primes
        {
            get => primes;
            set => SetProperty(ref primes, value);
        }

        private int squared = 0;
        public int Squared
        {
            get => squared;
            set => SetProperty(ref squared, value);
        }

        private int misparGadol = 0;
        public int MisparGadol
        {
            get => misparGadol;
            set => SetProperty(ref misparGadol, value);
        }

        private int misparShemi = 0;
        public int MisparShemi
        {
            get => misparShemi;
            set => SetProperty(ref misparShemi, value);
        }
    }
}