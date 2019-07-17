/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosViewModels.Services;

namespace ArithmosViewModels
{
    public class SettingsViewModel : ModelBase
    {
        public SettingsViewModel(ISettingsService settingsService)
        {
            this.SettingsService = settingsService;
            ((ModelBase)this.SettingsService).PropertyChanged += this.SettingsViewModel_PropertyChanged1;
        }

        private void SettingsViewModel_PropertyChanged1(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SettingsService.Save();
        }

        private ISettingsService settingsService = new SettingsService();
        public ISettingsService SettingsService
        {
            get { return this.settingsService; }
            set { this.SetField(ref this.settingsService, value); }
        }
    }
}