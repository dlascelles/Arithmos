/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Immutable;

namespace ArithmosApp.ViewModels;

public class GematriaMethodViewModel(GematriaMethod gematriaMethod) : ViewModelBase
{
    public GematriaMethod GetModel()
    {
        return new GematriaMethod(Cipher) { Id = Id, Name = Name, Sort = Sort, AddsTotalNumberOfCharacters = AddsTotalNumberOfCharacters, AddsTotalNumberOfWords = AddsTotalNumberOfWords };
    }

    public override string ToString()
    {
        return Name;
    }

    private int value;
    public int Value
    {
        get => value;
        set => SetProperty(ref this.value, value);
    }

    public bool IsSelected { get; set; } = true;

    public int Id { get; set; } = gematriaMethod.Id;

    public string Name { get; set; } = gematriaMethod.Name;

    public int Sort { get; set; } = gematriaMethod.Sort;

    public bool AddsTotalNumberOfCharacters { get; set; } = gematriaMethod.AddsTotalNumberOfCharacters;

    public bool AddsTotalNumberOfWords { get; set; } = gematriaMethod.AddsTotalNumberOfWords;

    public Cipher Cipher { get; private set; } = gematriaMethod.Cipher;

    public readonly ImmutableDictionary<char, int> ValueMapper = gematriaMethod.ValueMapper;
}
