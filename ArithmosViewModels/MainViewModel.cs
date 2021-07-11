/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ArithmosViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.ChildViews = new ObservableCollection<ObservableObject>
            {
                new CalculatorViewModel(new PhraseDataService(), new SettingsService()),
                new ScannerViewModel(new PhraseDataService(), new SettingsService()),
                new ExplorerViewModel(new PhraseDataService(), new SettingsService()),
                new SettingsViewModel(new SettingsService())
            };
        }

        public ObservableCollection<ObservableObject> ChildViews { get; }
    }
}