using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mvc1VaccinDemo.Services.Krisinformation
{
    public class CachedKrisInfoService : IKrisInfoService
    {
        private readonly IKrisInfoService _inner;
        private readonly IMemoryCache _cache;

        public CachedKrisInfoService(IKrisInfoService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }
        public List<KrisInfo> GetAllKrisInformation()
        {
            List<KrisInfo> list;
            if (_cache.TryGetValue("KrisInfo", out list))
                return list;
            list = _inner.GetAllKrisInformation();

            _cache.Set("KrisInfo", list, DateTime.Now.AddSeconds(60));
            return list;
            //if ((DateTime.Now - lastFetched).TotalSeconds > 60)
            //{
            //    cachedValue = _inner.GetAllKrisInformation();
            //    lastFetched = DateTime.Now;
            //}

            //return cachedValue;
        }

        public List<KrisInfo> GetEmergencies()
        {
            return GetAllKrisInformation().Where(r => r.Emergency).ToList();
        }

        public KrisInfo GetKrisInformation(string id)
        {
            return GetAllKrisInformation().FirstOrDefault(r => r.Id == id);
        }
    }


    public class Interest
    {
        public DateTime Datum { get; set; }
        public decimal Value { get; set; }
    }

    public class InterestService
    {
        public List<Interest> GetRepoInterestValues()
        {
            return new List<Interest>
            {
                new Interest{ Datum = new DateTime(2020,3,21),Value=2.22m},
                new Interest{ Datum = new DateTime(2020,3,22),Value=2.24m},
                new Interest{ Datum = new DateTime(2020,3,23),Value=2.18m}
            };
        }
    }


    public class KrisInfoConfig
    {
        public string Url { get; set; }
        public int NrToShow { get; set; }
    }

    public class KrisInfoService : IKrisInfoService
    {
        protected KrisInfoConfig config;
        public KrisInfoService(IOptions<KrisInfoConfig> conf)
        {
            config = conf.Value;
        }
        public class Test
        {
            public List<KrisInfo> ThemeList { get; set; } = new List<KrisInfo>();
        }
        public List<KrisInfo> GetAllKrisInformation()
        {
            var client = new HttpClient();
            string result = client.GetStringAsync(config.Url).Result;

            var listan = JsonConvert.DeserializeObject<Test>(result);
            return listan.ThemeList.Take(config.NrToShow).ToList();
        }

        public KrisInfo GetKrisInformation(string id)
        {
            return GetAllKrisInformation().FirstOrDefault(r => r.Id == id);
        }


        public List<KrisInfo> GetEmergencies()
        {
            return GetAllKrisInformation().Where(r => r.Emergency).ToList();
        }
    }
}