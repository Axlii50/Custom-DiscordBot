using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBotApi.Language
{
    internal static class LanguageManager
    {
        private static List<LanguageObject> Langs { get; set; }

        private static LanguageObject LoadLang(LangTypes type)
        {
            string json = File.ReadAllText(DCBotApi.Utility.Directory.GetPath($"Lang/{type.ToString()}.json"));
            System.Diagnostics.Debug.WriteLine(json);
            return JsonConvert.DeserializeObject<LanguageObject>(json);
        }

        public static void LoadLanguages()
        {
            //load all aviable languages
            Langs = new List<LanguageObject>();
            foreach (var x in (LangTypes[])Enum.GetValues(typeof(LangTypes)))
                Langs.Add(LanguageManager.LoadLang(x));
        }

        public static LanguageObject GetLang(LangTypes type)
        {
            return Langs.Find(x => x.type == type);
        }
    }
}
