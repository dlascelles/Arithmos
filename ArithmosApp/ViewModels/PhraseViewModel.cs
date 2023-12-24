/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Generic;

namespace ArithmosApp.ViewModels;

public class PhraseViewModel : ViewModelBase
{
    public PhraseViewModel(Phrase phrase)
    {
        Phrase = phrase;
    }
    
    public Phrase Phrase { get; set; }
}
