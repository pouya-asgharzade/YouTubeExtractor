using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YouTubeExtractor.DeCipher
{
    internal static class QueryHelper
    {
        internal static Dictionary<string, string> QuerySplitter(string Query)
        {
            Dictionary<string, string> SplitQuery = new Dictionary<string, string>();

            if (Query.Contains("?"))
            {
                Query = Query.Substring(Query.IndexOf('?') + 1);
            }

            foreach (string i in Regex.Split(Query, "&"))
            {
                string[] QueryList = Regex.Split(i, "=");

                string Key = QueryList[0];
                string Value = string.Empty;

                if (QueryList.Length == 2)
                {
                    Value = QueryList[1];
                }
                else if (QueryList.Length > 2)
                {
                    Value = string.Join("=", QueryList.Skip(1).ToArray());
                }

                SplitQuery.Add(Key, Value);
            }

            return SplitQuery;

        }

        internal static string ChangeQueryPara(string URL, string Key, string Value)
        {
            var query = QuerySplitter(URL);
            query[Key] = Value;
            var resultQuery = new StringBuilder();
            bool isFirst = true;

            foreach (KeyValuePair<string, string> pair in query)
            {
                if (!isFirst)
                {
                    resultQuery.Append("&");
                }

                resultQuery.Append(pair.Key);
                resultQuery.Append("=");
                resultQuery.Append(pair.Value);

                isFirst = false;
            }

            var uriBuilder = new UriBuilder(URL)
            {
                Query = resultQuery.ToString()
            };

            return uriBuilder.ToString();
        }
    }
}
