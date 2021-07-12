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
    /// Used to convert PhraseSeparator enum values to UI Checkboxes
    /// </summary>
    public class PhraseSeparatorConverter : IValueConverter
    {
        private PhraseSeparator separator;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PhraseSeparator temp = (PhraseSeparator)parameter;
            separator = (PhraseSeparator)value;
            return ((temp & separator) != 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            separator ^= (PhraseSeparator)parameter;
            return separator;
        }
    }
}