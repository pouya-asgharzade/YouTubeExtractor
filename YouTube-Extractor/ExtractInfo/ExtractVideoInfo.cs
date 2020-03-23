using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace YouTubeExtractor.ExtractInfo
{
    internal static class ExtractVideoInfo
    {
        internal static VideoInfo ExtractInfo(string HtmlData)
        {
            VideoInfo Info = new VideoInfo();

            JObject ExtractedPlayerArgs = ExtractPlayerArgsJson(HtmlData);
            Info = ExtractDataFromPlayerArgs(ExtractedPlayerArgs, Info);
            Info = ExtractDataFromPlayerResponse(HtmlData, Info);

            return Info;

        }
        private static JObject ExtractPlayerArgsJson(string HtmlData)
        {
            JObject PlayerArgsResult = null;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(HtmlData);

            var Nodes = htmlDoc.DocumentNode.SelectNodes("//script");

            foreach (var Node in Nodes)
            {
                if (Node.OuterHtml.Contains("RELATED_PLAYER_ARGS"))
                {
                    HtmlData = Regex.Replace(Node.OuterHtml, @"\t|\n|\r|\s", string.Empty);

                    Match PlayerArgsMatch = Regex.Match(HtmlData, @"yt.setConfig(\((.+?)\));", RegexOptions.Multiline);

                    JObject PlayerArgsJsonData = JObject.Parse(PlayerArgsMatch.Groups[2].Value);

                    JObject ExtractedPlayerArgs = ExtractRelatedPlayerArgs(PlayerArgsJsonData);

                    PlayerArgsResult = ExtractedPlayerArgs;
                }
            }
            return PlayerArgsResult;

        }

        private static JObject ExtractRelatedPlayerArgs(JObject JsonData)
        {
            string JsonDataString = JsonData["RELATED_PLAYER_ARGS"]["watch_next_response"].ToString();

            JObject PlayerResponseData = JObject.Parse(JsonDataString);

            return PlayerResponseData;

        }

        private static VideoInfo ExtractDataFromPlayerArgs(JObject JsonData, VideoInfo Info)
        {
            JToken VideoMetaDataRenderer = JsonData["contents"]["twoColumnWatchNextResults"]["results"]["results"]["contents"][0]["itemSectionRenderer"]["contents"][0]["videoMetadataRenderer"];

            Info.Like = Convert.ToUInt32(VideoMetaDataRenderer["likeButton"]["likeButtonRenderer"]["likeCount"]);
            Info.DisLike = Convert.ToUInt32(VideoMetaDataRenderer["likeButton"]["likeButtonRenderer"]["dislikeCount"]);
            Info.ChannelLogo = LogoExtractor.ExtractThumbnail(VideoMetaDataRenderer["owner"]["videoOwnerRenderer"]["thumbnail"]["thumbnails"]);
            Info.Subscribers = Convert.ToString(VideoMetaDataRenderer["owner"]["videoOwnerRenderer"]["subscribeButton"]["subscribeButtonRenderer"]["subscriberCountText"]["simpleText"]);

            return Info;

        }

        private static VideoInfo ExtractDataFromPlayerResponse(string HtmlCode, VideoInfo Info)
        {

            JObject JsonData = DataExtractor.ExtractJsonData(HtmlCode);
            JObject ExtractedPlayerResponseJson = DataExtractor.ExtractPlayerResponse(JsonData);
            JToken VideoDetails = ExtractedPlayerResponseJson["videoDetails"];

            Info.Title = Convert.ToString(VideoDetails["title"]);
            Info.Description = Convert.ToString(VideoDetails["shortDescription"]);
            Info.Channel = Convert.ToString(VideoDetails["author"]);
            Info.Thumbnail = LogoExtractor.ExtractThumbnail(VideoDetails["thumbnail"]["thumbnails"]);
            Info.View = Convert.ToInt32(VideoDetails["viewCount"]);
            Info.Duration = TimeSpan.FromSeconds(Convert.ToDouble(VideoDetails["lengthSeconds"]));

            return Info;

        }

    }
}
