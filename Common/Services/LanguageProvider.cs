using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highgeek.McWebApp.Common.Models.Language;
using Highgeek.McWebApp.Common.Services.Redis;
using NetTopologySuite.IO;

namespace Highgeek.McWebApp.Common.Services
{
    public interface ILanguageProvider
    {
        public Dictionary<string, string> LanguageCzech { get; set; }
        public Dictionary<string, string> LanguageEnglish { get; set; }

        public string GetLocale(string key);
        public string GetLocale(string key, string locale);
    }
    public class LanguageProvider : ILanguageProvider
    {
        private string defaultLocale = "cs";

        public Dictionary<string, string> LanguageCzech { get; set; }
        public Dictionary<string, string> LanguageEnglish { get; set; }
        public LanguageModel LanguageModel { get; set; }

        private readonly IRedisUpdateService _redisUpdateService;

        public LanguageProvider(IRedisUpdateService redisUpdateService)
        {
            _redisUpdateService = redisUpdateService;
            LanguageCzech = new Dictionary<string, string>();
            LanguageEnglish = new Dictionary<string, string>();
            LanguageModel = LanguageModel.FromJson(RedisService.GetFromRedis("settings:mcwebapp:language"));
            SetCzech();
            SetEnglish();

            _redisUpdateService.LocaleChangeRequested += RefreshLocale;
        }

        public void RefreshLocale()
        {
            LanguageModel = LanguageModel.FromJson(RedisService.GetFromRedis("settings:mcwebapp:language"));
            SetCzech();
            SetEnglish();
            _redisUpdateService.CallLanguageProviderRefresh();
        }

        public void SetEnglish()
        {
            LanguageEnglish.Clear();

            foreach (var item in LanguageModel.LanguageKeys)
            {
                LanguageEnglish.Add(item.Key, item.En);
            }
        }

        public void SetCzech()
        {
            LanguageCzech.Clear();

            foreach (var item in LanguageModel.LanguageKeys)
            {
                LanguageCzech.Add(item.Key, item.Cs);
            }
        }

        public string GetLocale(string key)
        {
            return GetKey(defaultLocale, key);
        }

        public string GetLocale(string key, string locale)
        {

            return GetKey(locale, key);
        }

        private string GetKey(string locale, string key)
        {
            switch (locale)
            {
                case "cs":
                    try
                    {
                        return LanguageCzech[key];
                    }catch(Exception ex)
                    {
                        return "TranslationNotFound \"" + key + "\"";
                    }
                case "en":
                    try
                    {
                        return LanguageEnglish[key];
                    }
                    catch (Exception ex)
                    {
                        return "TranslationNotFound \"" + key + "\"";
                    }
                default:
                    try
                    {
                        return LanguageCzech[key];
                    }
                    catch (Exception ex)
                    {
                        return "TranslationNotFound \"" + key + "\"";
                    }
            }
        }
    }

}
