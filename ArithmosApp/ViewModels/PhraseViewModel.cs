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
        Id = phrase.Id;
        OperationId = phrase.OperationId;
        Content = phrase.Content;
        foreach (var val in phrase.Values)
        {
            Values.Add(val.GematriaMethod.Name, val.Value);
        }
        Alphabet = phrase.Alphabet.ToString();
        Phrase = phrase;
    }

    public long Id { get; set; }

    public int OperationId { get; set; }

    public string Content { get; set; }

    public Dictionary<string, int> Values { get; set; } = [];

    public string Alphabet { get; set; }

    public Phrase Phrase { get; set; }
}
