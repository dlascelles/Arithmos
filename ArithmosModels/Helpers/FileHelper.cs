/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels.Helpers
{
    public static class FileHelper
    {
        public static string GetFileNameFromCurrentDate(string prefix, string extension)
        {
            return
                (string.IsNullOrWhiteSpace(prefix) ? "" : prefix) + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + (string.IsNullOrWhiteSpace(extension) ? "" : '.' + extension);
        }
    }
}