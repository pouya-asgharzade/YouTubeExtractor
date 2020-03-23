using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace YouTubeExtractor
{
    internal static class DataExtractor
    {
        internal static async Task<string> ExtractHtmlSourceAsync(string URL)
        {
            try
            {
                string HtmlSource = string.Empty;

                HtmlSource = await Tool.Client.GetStringAsync(URL);

                return HtmlSource;
            }
            catch(Exception error)
            {
                throw new Exceptions.DataExtractorException(error.Message);
            }
        }

        internal static JObject ExtractJsonData(string HtmlSource)
        {
            Match JsonDataMatch = Regex.Match(HtmlSource, @"ytplayer\.config\s*=\s*(\{.+?\});", RegexOptions.Multiline);

            string ExtractedJsonData = JsonDataMatch.Result("$1");

            JObject JsonData = JObject.Parse(ExtractedJsonData);

            return JsonData;

        }

        internal static JObject ExtractPlayerResponse(JObject JsonData)
        {
            try
            {
                string JsonDataString = JsonData["args"]["player_response"].ToString();

                JObject PlayerResponseData = JObject.Parse(JsonDataString);

                return PlayerResponseData;
            }
            catch(NullReferenceException)
            {
                throw new Exceptions.DataExtractorException("Null Reference,Please check your url or report this bug at https://github.com/pouya-asgharzade/YouTubeExtractor/issues");
            }
        }

        internal static async Task<string> ExtractJsPlayerAsync(JObject JsonData)
        {
            JToken ExtractedPlayerConfig = JsonData["assets"]["js"];

            string JsPlayerUrl = "https://www.youtube.com/" + ExtractedPlayerConfig;

            string JsPlayerSource = await Tool.Client.GetStringAsync(JsPlayerUrl);

            return JsPlayerSource;
        }

    }
}
