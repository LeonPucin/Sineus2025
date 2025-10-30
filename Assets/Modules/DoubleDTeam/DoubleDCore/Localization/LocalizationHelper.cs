using I2.Loc;
using UnityEngine;

namespace DoubleDCore.Localization
{
    public static class LocalizationHelper
    {
        private static string[] _common = new string[]
        {
            "common_phrases/phrase_1",
            "common_phrases/phrase_2",
            "common_phrases/phrase_3"
        };

        public static string GetStringByToken(string token)
        {
            string result;
            LocalizedString locString = token;
            string locText = locString;
            
            if (locText != null)
            {
                result = locText;
            }
            else
            {
                if (Application.isEditor)
                {
                    locText = token;
                }
                else
                {
                    locString = _common[Random.Range(0, _common.Length)];
                    locText = locString;
                }

                result = locText;
            }

            return result;
        }

        public static string GetStringByToken(string token, params object[] parameters)
        {
            var resultToken = GetStringByToken(token);
            return string.Format(resultToken, parameters);
        }

        public static bool HasLocalization(string token, out string text)
        {
            LocalizedString locString = token;
            text = locString;
            return !string.IsNullOrEmpty(text);
        }
    }
}