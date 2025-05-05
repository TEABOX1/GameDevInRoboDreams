using System;
using System.Collections.Generic;
using GlobalSource;

namespace Boot
{
    public interface ILocalizationService : IService
    {
        event Action OnLanguageChanged;
        void SetLanguage(string language);
        string GetCurrentLanguage();
        List<string> GetSupportedLanguages();
        string GetTermValue(string term);
    }
}