using System;
using System.Collections.Generic;

namespace AuthenticationServer.Common.Extentions
{
    public static class DictionaryExtentions
    {
        public static Dictionary<string, object> AddRangeParameters(this Dictionary<string, object> masterDictionary, Dictionary<string, object> appendee)
        {
            foreach ((string key, object value) in appendee)
            {
                try
                {
                    masterDictionary.Add(key, value);
                }
                catch (Exception)
                {
                }
            }

            return masterDictionary;
        }
    }
}
