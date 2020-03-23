# YouTubeExtractor
YouTubeExtractor is a library for download videos from YouTube or download audio files

# Value Types
UrlTypes
- Specifies the type of output file
- for example : UrlTypes.VideoMP4 or UrlTypes.AudioMP4

Quality
- Specifies the quality of output file
- for example : Quality.Medium360 or Quality.High720

OutPutType
- Specifies the type to extract
- for example : OutPutType.Muxed or OutPutType.AudioOnly

# Url Types
Muxed
- Sound and image composition
VideoOnly
- Only video
AudioOnly
- Only audio

# Methods
GetVideoAsync
- Get information and urls
ExtractCustomUrl
- Get url with custom (Size,OutputType,Quality,ExtractType)


# Usage 

```c#

var MyClient = new System.Net.Http.HttpClient();
YouTubeExtractor.Extractor Extractor = new YouTubeExtractor.Extractor(MyClient);

var ExtractedVideo = await Extractor.GetVideoAsync("https://www.youtube.com/watch?v=U-tropVp94k");

```
# Get info
```c#

Console.WriteLine("Title : " + ExtractedVideo.Info.Title);
Console.WriteLine("Channel : " + ExtractedVideo.Info.Channel);
Console.WriteLine("Description : " + ExtractedVideo.Info.Description);
Console.WriteLine("Subscribers : " + ExtractedVideo.Info.Subscribers);
Console.WriteLine("ChannelLogo : " + ExtractedVideo.Info.Title);
Console.WriteLine("Thumbnail : " + ExtractedVideo.Info.Title);
Console.WriteLine("View : " + ExtractedVideo.Info.View);
Console.WriteLine("Like : " + ExtractedVideo.Info.Like);
Console.WriteLine("DisLike : " + ExtractedVideo.Info.DisLike);
Console.WriteLine("Duration : " + ExtractedVideo.Info.Duration);

```
# Get custom download url
```c#

Url ExtractedURL = Extractor.ExtractCustomUrl(
  UrlEnum.UrlTypes.VideoMP4,
  UrlEnum.OutPutType.VideoOnly,
  UrlEnum.Quality.Medium360,
  ExtractedVideo.URL);
  
Console.WriteLine(ExtractedURL.URL);
```
- With filter size
```c#

Url ExtractedURL = Extractor.ExtractCustomUrl(
  new Filter(OperatorEnum.Operators.Smaller, 30000000),
  UrlEnum.UrlTypes.VideoMP4,
  UrlEnum.OutPutType.VideoOnly,
  UrlEnum.Quality.Medium360,
  ExtractedVideo.URL);
  
Console.WriteLine(ExtractedURL.URL);
```
