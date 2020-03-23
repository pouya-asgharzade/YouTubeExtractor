using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeExtractor.ExtractUrl
{
    internal static class ExtractVideoUrl
    {
        internal static async Task<Urls> UrlExtractorAsync(JObject PlayerResponseJsonData)
        {
            Urls ExtractedUrl = new Urls();

            ExtractedUrl.Muxed = await ExtractMuxedAsync(PlayerResponseJsonData);
            ExtractedUrl.Adaptive = await ExtractAdaptiveAsync(PlayerResponseJsonData);

            return ExtractedUrl;
        }

        private static async Task<List<Url>> ExtractMuxedAsync(JObject MuxedJsonData)
        {
            List<Url> UrlList = new List<Url>();

            foreach (var i in MuxedJsonData["streamingData"]["formats"])
            {
                Url URL = new Url();

                URL.Size = await ConvertSizeAsync(i);
                URL.UrlType = ConvertType(i["mimeType"].ToString());
                URL.VideoQuality = ConvertQuality(i["quality"].ToString());
                URL.UrlOutPut = ConvertOutPut(i);
                URL.URL = ConvertUrl(i);

                UrlList.Add(URL);
            }

            return UrlList;
        }

        private static async Task<List<Url>> ExtractAdaptiveAsync(JObject AdaptiveJsonData)
        {
            List<Url> UrlList = new List<Url>();

            foreach (var i in AdaptiveJsonData["streamingData"]["adaptiveFormats"])
            {
                Url URL = new Url();

                URL.Size = await ConvertSizeAsync(i);
                URL.UrlType = ConvertType(i["mimeType"].ToString());
                URL.VideoQuality = ConvertQuality(i["quality"].ToString());
                URL.UrlOutPut = ConvertOutPut(i);
                URL.URL = ConvertUrl(i);

                UrlList.Add(URL);
            }

            return UrlList;
        }

        private static UrlEnum.UrlTypes ConvertType(string MimType)
        {
            Match Type = Regex.Match(MimType, @"(\w+)/(\w+)");

            switch (Type.Value)
            {
                case "video/mp4":
                    return UrlEnum.UrlTypes.VideoMP4;
                case "video/webm":
                    return UrlEnum.UrlTypes.VideoWebm;
                case "audio/mp4":
                    return UrlEnum.UrlTypes.AudioMP4;
                case "audio/webm":
                    return UrlEnum.UrlTypes.AudioWebm;
                default:
                    return UrlEnum.UrlTypes.UnKnown;
            }
        }

        private static UrlEnum.Quality ConvertQuality(string Quality)
        {
            switch (Quality)
            {
                case "tiny":
                    return UrlEnum.Quality.Low144;
                case "small":
                    return UrlEnum.Quality.Low240;
                case "medium":
                    return UrlEnum.Quality.Medium360;
                case "large":
                    return UrlEnum.Quality.Medium480;
                case "hd720":
                    return UrlEnum.Quality.High720;
                case "hd1080":
                    return UrlEnum.Quality.High1080;
                case "hd1440":
                    return UrlEnum.Quality.High1440;
                case "hd2160":
                    return UrlEnum.Quality.High2160;
                case "hd2880":
                    return UrlEnum.Quality.High2880;
                case "hd3072":
                    return UrlEnum.Quality.High3072;
                case "hd4320":
                    return UrlEnum.Quality.High4320;
                default:
                    return UrlEnum.Quality.UnKnown;
            }
        }


        private static UrlEnum.OutPutType ConvertOutPut(JToken Format)
        {
            if (Format["qualityLabel"] != null && Format["audioQuality"] != null)
            {
                return UrlEnum.OutPutType.Muxed;
            }
            else if (Format["qualityLabel"] != null && Format["audioQuality"] == null)
            {
                return UrlEnum.OutPutType.VideoOnly;
            }
            else if (Format["qualityLabel"] == null && Format["audioQuality"] != null)
            {
                return UrlEnum.OutPutType.AudioOnly;
            }
            else
            {
                return UrlEnum.OutPutType.UnKnown;
            }
        }

        private static string ConvertUrl(JToken Format)
        {
            if (Format["url"] != null)
            {
                return Format["url"].ToString();
            }
            else if (Format["cipher"] != null)
            {
                Dictionary<string, string> SplitQuery = DeCipher.QueryHelper.QuerySplitter(Format["cipher"].ToString());
                return DeCipher.Decipher.DecipherSignature(SplitQuery);
            }
            else
            {
                throw new Exception("Can't extract url from format");
            }
        }

        private static async Task<long> ConvertSizeAsync(JToken Format)
        {
            long ContentLength = 0;

            if (Format["contentLength"] != null)
            {
                ContentLength = Convert.ToInt64(Format["contentLength"]);
            }
            else if (Regex.Match(ConvertUrl(Format), @"clen=(\d+)").Groups[1].Success)
            {
                ContentLength = Convert.ToInt64(Regex.Match(ConvertUrl(Format), @"clen=(\d+)").Groups[1].Value);
            }
            else
            {
                HttpWebRequest Request = HttpWebRequest.CreateHttp(ConvertUrl(Format));

                Request.Method = "HEAD";

                WebResponse Response = await Request.GetResponseAsync();

                ContentLength = Convert.ToInt64(Response.ContentLength);

                Response.Dispose();

            }

            return ContentLength;
        }
    }
}
