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
    /// Used to convert CalculationMethod enum values to UI Checkboxes
    /// </summary>
    public class CalculationMethodConverter : IValueConverter
    {
        private CalculationMethod method;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CalculationMethod temp = (CalculationMethod)parameter;
            method = (CalculationMethod)value;
            return ((temp & method) != 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            method ^= (CalculationMethod)parameter;
            return method;
        }
    }
}