using System;
using System.Runtime.InteropServices;

namespace D3DataParser.DataStruct
{
    /// <summary>
    /// 索引结构 
    /// 长度 40(0x28) 字节
    /// </summary>
    public struct StlHeader
    {
        public uint MagicNumber; // err.....  0xdeadbeef 
        public uint FileTypeId;
        public ulong Unused1;

        public uint StlFileId;     // Stl file Id

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] Unk1;         // always 0x00000000

        public int HeaderSize;     // size (in bytes) of the StlHeader? (always 0x00000028) only one header.
        public int TotalDataSize;    // size (in bytes) of the StlEntries

        public ulong Unused2;         // always 0x00000000
    }
}
