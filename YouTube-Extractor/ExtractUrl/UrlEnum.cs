namespace YouTubeExtractor.ExtractUrl
{
    public static class UrlEnum
    {
        public enum UrlTypes
        {
            VideoMP4,
            VideoWebm,
            AudioMP4,
            AudioWebm,
            UnKnown
        }
        public enum Quality
        {
            Low144,
            Low240,
            Medium360,
            Medium480,
            High720,
            High1080,
            High1440,
            High2160,
            High2880,
            High3072,
            High4320,
            UnKnown
        }
        public enum OutPutType
        {
            Muxed,
            VideoOnly,
            AudioOnly,
            UnKnown
        }
    }
}
