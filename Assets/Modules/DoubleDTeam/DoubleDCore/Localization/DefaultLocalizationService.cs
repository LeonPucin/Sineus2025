using DoubleDCore.Localization.Base;
using UnityEngine;

namespace DoubleDCore.Localization
{
    public class DefaultLocalizationService : BaseLocalizationService
    {
        public override void SetAutoLanguage()
        {
            SetLanguage(Application.systemLanguage.ToString());
        }

        public override void SetDefaultLanguage()
        {
            SetLanguage("English");
        }
    }
}