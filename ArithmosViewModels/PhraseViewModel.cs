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

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private bool isMarked;
        public bool IsMarked
        {
            get => isMarked;
            set => SetProperty(ref isMarked, value);
        }

        private string phraseUserText = "";
        public string PhraseUserText
        {
            get => phraseUserText;
            set
            {
                if (SetProperty(ref phraseUserText, value))
                {
                    Phrase = new Phrase(phraseUserText);
                }
            }
        }

        private string phraseText = "";
        public string PhraseText
        {
            get => phraseText;
            private set => SetProperty(ref phraseText, value);
        }

        private Alphabet alphabet;
        public Alphabet Alphabet
        {
            get => alphabet;
            private set => SetProperty(ref alphabet, value);
        }

        private int gematria;
        public int Gematria
        {
            get => gematria;
            private set => SetProperty(ref gematria, value);
        }

        private int ordinal;
        public int Ordinal
        {
            get => ordinal;
            private set => SetProperty(ref ordinal, value);
        }

        private int reduced;
        public int Reduced
        {
            get => reduced;
            private set => SetProperty(ref reduced, value);
        }

        private int sumerian;
        public int Sumerian
        {
            get => sumerian;
            private set => SetProperty(ref sumerian, value);
        }

        private int primes;
        public int Primes
        {
            get => primes;
            private set => SetProperty(ref primes, value);
        }

        private int squared;
        public int Squared
        {
            get => squared;
            private set => SetProperty(ref squared, value);
        }

        private int misparGadol;
        public int MisparGadol
        {
            get => misparGadol;
            private set => SetProperty(ref misparGadol, value);
        }

        private int misparShemi;
        public int MisparShemi
        {
            get => misparShemi;
            private set => SetProperty(ref misparShemi, value);
        }

        private int operationId;
        public int OperationId
        {
            get => operationId;
            private set => SetProperty(ref operationId, value);
        }

        private Phrase phrase;
        public Phrase Phrase
        {
            get => phrase;
            private set
            {
                SetProperty(ref phrase, value);
                PhraseText = phrase.NormalizedText;
                Alphabet = phrase.Alphabet;
                Gematria = Phrase.Gematria;
                Ordinal = Phrase.Ordinal;
                Reduced = Phrase.Reduced;
                Sumerian = Phrase.Sumerian;
                Primes = Phrase.Primes;
                Squared = Phrase.Squared;
                MisparGadol = Phrase.MisparGadol;
                MisparShemi = Phrase.MisparShemi;
                OperationId = Phrase.OperationId;
            }
        }
    }
}