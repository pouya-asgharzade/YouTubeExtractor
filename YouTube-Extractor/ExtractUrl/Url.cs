

namespace YouTubeExtractor.ExtractUrl
{
    public class Url
    {
        public UrlEnum.UrlTypes UrlType { get; internal set; }
        public UrlEnum.Quality VideoQuality { get; internal set; }
        public UrlEnum.OutPutType UrlOutPut { get; internal set; }
        public string URL { get; internal set; }
        public long Size { get; internal set; }


    }
}
