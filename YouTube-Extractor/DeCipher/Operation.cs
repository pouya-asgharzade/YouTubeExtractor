namespace YouTubeExtractor.DeCipher
{
    internal class Operation
    {
        internal enum OperationType
        {
            Slice,
            CharSwap,
            Reverse
        }

        internal OperationType Type { set; get; }
        internal string Index { set; get; }
    }
}
