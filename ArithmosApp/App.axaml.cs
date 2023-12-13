/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels;
using ArithmosApp.ViewModels.Services;
using ArithmosApp.Views;
using ArithmosDataAccess;
using ArithmosModels;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

namespace ArithmosApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Database.CreateDatabase();
        SettingDataService settingDataService = new();
        string theme = settingDataService.Retrieve(Constants.Settings.Theme);
        Current.RequestedThemeVariant = new Avalonia.Styling.ThemeVariant(theme, null);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new ViewModelBase()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
