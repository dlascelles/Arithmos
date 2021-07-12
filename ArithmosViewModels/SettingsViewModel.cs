/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ArithmosViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel(ISettingsService settingsService)
        {
            this.SettingsService = settingsService;
            ((ObservableObject)this.SettingsService).PropertyChanged += this.SettingsViewModel_PropertyChanged;
        }

        private void SettingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SettingsService.Save();
        }

        private ISettingsService settingsService = new SettingsService();
        public ISettingsService SettingsService
        {
            get => this.settingsService;
            set => SetProperty(ref this.settingsService, value);
        }
    }
}