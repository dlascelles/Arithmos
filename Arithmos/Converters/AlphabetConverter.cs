/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Arithmos.Converters
{
    /// <summary>
    /// Used to convert Alphabet enum values to UI Checkboxes
    /// </summary>
    public class AlphabetConverter : IValueConverter
    {
        private Alphabet alphabet;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Alphabet temp = (Alphabet)parameter;
            alphabet = (Alphabet)value;
            return ((temp & alphabet) != 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            alphabet ^= (Alphabet)parameter;
            return alphabet;
        }
    }
}