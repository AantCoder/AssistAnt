using GTranslatorAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt
{
    public class TranslatorText
    {
        private string SourceLanguage { get; set; }
        private string TargetLanguage { get; set; }
        
        private Translator TranslatorAPI { get; set; }

        private string LiteConvertLanguage(string language)
        {
            return language
                .Replace("eng", "en")
                .Replace("rus", "ru");
        }

        public TranslatorText(string sourceLanguage, string targetLanguage)
        {
            SourceLanguage = LiteConvertLanguage(sourceLanguage ?? "en");
            TargetLanguage = LiteConvertLanguage(targetLanguage ?? "ru");

            var settings = new Settings()
            {
                GTranslatorAPIURL = "https://translate.googleapis.com/translate_a/single?client=gtx&sl={srcl}&tl={tgtl}&dt=t&q={txt}",
                NetworkQueryTimeout = 2000,
                ParallelizeTranslationOfSegments = true,
                SplitStringBeforeTranslate = false,
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36"
            };
            TranslatorAPI = new Translator(settings);
        }

        public string Translate(string text)
        {
            Task<Translation> task = TranslatorAPI.TranslateAsync(
                SourceLanguage,
                TargetLanguage,
                text
                );
            task.Wait();
            return task.Result.TranslatedText;
        }
    }
}
