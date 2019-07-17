/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;
using System.IO;
using System.Windows;

namespace Arithmos
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.None), "ArithmosData");
            try
            {
                Directory.CreateDirectory(mainPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
            try
            {
                if (!File.Exists(Path.Combine(mainPath, "arithmosdb.sqlite")))
                {
                    File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arithmosdb.sqlite"), Path.Combine(mainPath, "arithmosdb.sqlite"));
                }
                if (!File.Exists(Path.Combine(mainPath, "arithmosdb.sqlite")))
                {
                    MessageBox.Show("Could not find application's data files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }
        }
    }
}
