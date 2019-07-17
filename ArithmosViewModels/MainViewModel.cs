/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Services;
using System.Collections.ObjectModel;

namespace ArithmosViewModels
{
    public class MainViewModel : ModelBase
    {
        public MainViewModel()
        {
            this.ChildViews = new ObservableCollection<ModelBase>
            {
                new CalculatorViewModel(new PhraseDataService(), new SettingsService()),
                new ScannerViewModel(new PhraseDataService(), new SettingsService()),
                new ExplorerViewModel(new PhraseDataService(), new SettingsService()),
                new SettingsViewModel(new SettingsService())
            };
        }

        public ObservableCollection<ModelBase> ChildViews { get; }
    }
}