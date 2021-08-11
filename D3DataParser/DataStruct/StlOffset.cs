namespace D3DataParser.DataStruct
{
    /// <summary>
    /// 文本索引
    /// </summary>
    public struct StlOffset
    {
        public long ZeroHolder { get; set; }    // always 0x00000000
        public int KeyOffset { get; set; }  // file offset for Key (non-NLS key)
        public int KeyLength { get; set; }    // size of Key
        public long Flag1 { get; set; }    // always 0x00000000
        public int StrOffset { get; set; }  // file offset for LocaleText
        public int StrLength { get; set; }    // size of LocaleText
        public long Flag2 { get; set; }    // maybe crc
    }
}
