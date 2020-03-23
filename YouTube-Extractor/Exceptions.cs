using System;

namespace YouTubeExtractor
{
    public class Exceptions
    {
        public class DataExtractorException : Exception
        {
            public DataExtractorException(string Message) : base(Message) { }
        }


    }
}
