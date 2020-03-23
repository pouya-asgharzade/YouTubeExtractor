using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using YouTubeExtractor.ExtractUrl;
using System.Net.Http;

namespace YouTubeExtractor
{
    public class Extractor
    {
        public Extractor(HttpClient Client)
        {
            Tool.Client = Client;
        }

        public async Task<ExtractedVideo> GetVideoAsync(string URL)
        {
            ExtractedVideo Video = new ExtractedVideo();

            URL = UrlResolver.NormalizeUrl(URL);

            string ExtractedHtmlSource = await DataExtractor.ExtractHtmlSourceAsync(URL);

            JObject ExtractedJsonData = DataExtractor.ExtractJsonData(ExtractedHtmlSource);

            Tool.JsPlayerSource = await DataExtractor.ExtractJsPlayerAsync(ExtractedJsonData);

            JObject PlayerResponseData = DataExtractor.ExtractPlayerResponse(ExtractedJsonData);

            Urls ExtractedUrl = await ExtractVideoUrl.UrlExtractorAsync(PlayerResponseData);

            VideoInfo ExtractedInfo = ExtractInfo.ExtractVideoInfo.ExtractInfo(ExtractedHtmlSource);

            Video.Info = ExtractedInfo;
            Video.URL = ExtractedUrl;

            return Video;
        }

        public Url ExtractCustomUrl(UrlEnum.UrlTypes UrlType, UrlEnum.OutPutType OutPutType, UrlEnum.Quality Quality, Urls UrlList)
        {
            foreach (Url i in UrlList.Muxed)
            {
                if (i.UrlType == UrlType && i.UrlOutPut == OutPutType && i.VideoQuality == Quality)
                {
                    return i;
                }
            }

            foreach (Url i in UrlList.Adaptive)
            {
                if (i.UrlType == UrlType && i.UrlOutPut == OutPutType && i.VideoQuality == Quality)
                {
                    return i;
                }
            }

            return null;
        }
        public Url ExtractCustomUrl(FilterSize.Filter Filter, UrlEnum.UrlTypes UrlType, UrlEnum.OutPutType OutPutType, UrlEnum.Quality Quality, Urls UrlList)
        {
            foreach (Url i in UrlList.Muxed)
            {
                if (i.UrlType == UrlType && i.UrlOutPut == OutPutType && i.VideoQuality == Quality && ApplayOprator(Filter,i.Size))
                {
                    return i;
                }
            }

            foreach (Url i in UrlList.Adaptive)
            {
                if (i.UrlType == UrlType && i.UrlOutPut == OutPutType && i.VideoQuality == Quality && ApplayOprator(Filter, i.Size))
                {
                    return i;
                }
            }

            return null;
        }

        private bool ApplayOprator(FilterSize.Filter Filter, long Value)
        {
            if (Filter._Operator == FilterSize.OperatorEnum.Operators.Bigger)
            {
                if (Value >= Filter._Value)
                {
                    return true;
                }
            }
            else if (Filter._Operator == FilterSize.OperatorEnum.Operators.Smaller)
            {
                if (Value <= Filter._Value)
                {
                    return true;
                }
            }
            else if (Filter._Operator == FilterSize.OperatorEnum.Operators.UnKnown)
            {
                return true;
            }

            return false;
        }
    }
}
