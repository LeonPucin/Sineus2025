using System;
using UnityEngine;
using I2.Loc;

namespace DoubleDCore.Localization.Base
{
    public abstract class BaseLocalizationService : ILocalizationService
    {
        public event Action<string> LanguageChanged;
        
        public virtual void SetAutoLanguage()
        {
            SetLanguage(Application.systemLanguage.ToString());
        }

        public void SetLanguage(string language)
        {
            LocalizationManager.CurrentLanguage = language;
            LanguageChanged?.Invoke(LocalizationManager.CurrentLanguage);
        }

        public string GetLanguage()
        {
            return LocalizationManager.CurrentLanguage;
        }

        public virtual void SetDefaultLanguage()
        {
            SetLanguage("English");
        }

        public string GetTranslation(string key, params object[] smartObjects)
        {
            return LocalizationHelper.GetStringByToken(key, smartObjects);
        }
        
        public string GetTranslation(string key)
        {
            return LocalizationHelper.GetStringByToken(key);
        }
    }
}