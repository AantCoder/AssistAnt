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

        private static Dictionary<string, string> Cache = new Dictionary<string, string>();

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
            try
            {
                var cacheKey = SourceLanguage + TargetLanguage + "|" + text;
                if (Cache.TryGetValue(cacheKey, out var cacheVal))
                {
                    return cacheVal;
                }

                text = Preprocessing(text);

                Task<Translation> task = TranslatorAPI.TranslateAsync(
                    SourceLanguage,
                    TargetLanguage,
                    text
                    );
                task.Wait();
                var val = task.Result.TranslatedText;

                val = Postprocessing(val);

                Cache[cacheKey] = val;

                return val;
            }
            catch
            {
                return "";
            }
        }

        private const string Separator = "##";
        private const string Shielding = "(#)";
        private const string SeparatorSpaсe = " " + Separator + " ";

        /// <summary>
        /// Убираем переносы строк, и вставляем разделитель, который для переводчика не разбивает предложение на насколько, но который он сохраняет в тексте перевода.
        /// </summary>
        private string Preprocessing(string text)
            => text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n")
                .Replace(Separator, Shielding).Replace("\r\n", SeparatorSpaсe)
                .Replace(SeparatorSpaсe + SeparatorSpaсe + SeparatorSpaсe + SeparatorSpaсe, "\r\n").Replace(SeparatorSpaсe + SeparatorSpaсe + SeparatorSpaсe, "\r\n").Replace(SeparatorSpaсe + SeparatorSpaсe, "\r\n");

        /// <summary>
        /// Восстанавливаем переносы строк по разделителю
        /// </summary>
        private string Postprocessing(string text)
            => text.Replace(Separator, "\r\n").Replace("\r\n ", "\r\n").Replace(" \r\n", "\r\n").Replace(Shielding, Separator);

    }
}
