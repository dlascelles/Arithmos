/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/

namespace ArithmosViewModels.Services
{
    public interface ISettingsService
    {
        bool ShowGematria { get; set; }
        bool ShowOrdinal { get; set; }
        bool ShowReduced { get; set; }
        bool ShowSumerian { get; set; }
        bool ShowPrimes { get; set; }
        bool ShowSquared { get; set; }
        bool ShowMisparGadol { get; set; }
        bool ShowMisparShemi { get; set; }

        bool SelectGematria { get; set; }
        bool SelectOrdinal { get; set; }
        bool SelectReduced { get; set; }
        bool SelectSumerian { get; set; }
        bool SelectPrimes { get; set; }
        bool SelectSquared { get; set; }
        bool SelectMisparGadol { get; set; }
        bool SelectMisparShemi { get; set; }

        bool SelectEnglish { get; set; }
        bool SelectHebrew { get; set; }
        bool SelectGreek { get; set; }
        bool SelectMixed { get; set; }

        void Load();
        void Save();
    }
}
