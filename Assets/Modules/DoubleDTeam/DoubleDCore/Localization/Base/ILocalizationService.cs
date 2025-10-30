using System;
using Cysharp.Threading.Tasks;

namespace DoubleDCore.Localization.Base
{
    public interface ILocalizationService
    {
        public event Action<string> LanguageChanged;

        public void SetAutoLanguage();

        public void SetLanguage(string language);

        public string GetLanguage();

        public void SetDefaultLanguage();

        public string GetTranslation(string key, params object[] smartObjects);
        
        public string GetTranslation(string key);
    }
}