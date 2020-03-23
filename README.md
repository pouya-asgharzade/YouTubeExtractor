# YouTubeExtractor
`ouTubeExtractor is a library for download videos from YouTube or download audio files`

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

