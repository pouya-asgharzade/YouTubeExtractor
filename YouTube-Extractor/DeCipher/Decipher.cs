using System;
using System.Web;
using System.Collections.Generic;

namespace YouTubeExtractor.DeCipher
{
    internal static class Decipher
    {
        internal static string DecipherSignature(Dictionary<string, string> Format)
        {
            string URL = UrlCreator(Format);

            Dictionary<string, string> SplitQuery = QueryHelper.QuerySplitter(URL);

            string Signature = SplitQuery["sig"];

            string DecryptSignature = DeCipher.DecipherSignature.Decrypt(Signature,URL);

            return DecryptSignature;
        }

        private static string ExtractSignature(Dictionary<string,string> Format)
        {
            if(Format.ContainsKey("s") || Format.ContainsKey("sig"))
            {
                string Signature = string.Empty;

                if (Format.ContainsKey("s"))
                {
                    Signature = Format["s"];
                }
                else
                {
                    Signature = Format["sig"];
                }

                Signature  = HttpUtility.UrlDecode(Signature);

                return Signature;
            }
            else
            {
                throw new Exception("Can't Extract Signature");
            }
        }

        private static string CheckFallbackHost(Dictionary<string,string> Format)
        {
            if (Format.ContainsKey("fallback_host"))
            {
                return "&fallback_host=" + Format["fallback_host"];
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CheckRateByPass(Dictionary<string, string> Format)
        {
            if (!Format.ContainsKey("ratebypass"))
            {
                return "&ratebypass=yes";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string CheckPara(string URL)
        {
            Dictionary<string, string> SplitQuery = QueryHelper.QuerySplitter(URL);

            URL += CheckFallbackHost(SplitQuery);

            URL += CheckRateByPass(SplitQuery);

            return URL;
        }

        private static string UrlCreator(Dictionary<string, string> Format)
        {
            string URL = $"{Format["url"]}&sig={ExtractSignature(Format)}";

            URL = HttpUtility.UrlDecode(URL);
            URL = HttpUtility.UrlDecode(URL);

            URL = CheckPara(URL);

            return URL;
        }

    }
}
