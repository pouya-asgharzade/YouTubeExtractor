using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace YouTubeExtractor.ExtractInfo
{
    internal static class LogoExtractor
    {
        internal static List<LogoInfo> ExtractThumbnail(JToken Thumbnails)
        {
            try
            {
                List<LogoInfo> ExtractedThumbnail = new List<LogoInfo>();

                foreach (JToken i in Thumbnails)
                {
                    LogoInfo Info = new LogoInfo();
                    Info.URL = Convert.ToString(i["url"]);
                    Info.Width = Convert.ToInt32(i["width"]);
                    Info.Height = Convert.ToInt32(i["height"]);
                    ExtractedThumbnail.Add(Info);
                }

                return ExtractedThumbnail;
            }
            catch(Exception error)
            {
                throw new Exception("Extract Thumbnail Error : " + error.Message);
            }
        }


    }
}
