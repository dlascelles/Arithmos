/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels.Helpers
{
    /// <summary>
    /// A class containing helper methods related to files
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Will create a valid filename based on the current time and the optional parameters
        /// </summary>
        /// <param name="prefix">An optional prefix for the file name</param>
        /// <param name="extension">The optional extension for the file</param>
        /// <returns>A string</returns>
        public static string GetFileNameFromCurrentDate(string prefix = null, string extension = null)
        {
            return
                (string.IsNullOrWhiteSpace(prefix) ? "" : prefix) + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + (string.IsNullOrWhiteSpace(extension) ? "" : '.' + extension);
        }
    }
}